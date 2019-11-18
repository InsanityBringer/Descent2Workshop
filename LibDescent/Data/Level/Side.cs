using System;
using System.Collections.Generic;
using System.Linq;

namespace LibDescent.Data
{
    public enum Edge
    {
        Right,
        Bottom,
        Left,
        Top
    }

    public class Side
    {
        private readonly Segment parentSegment;
        private readonly uint parentSegmentSideNum;

        public Side(Segment parent, uint sideNum, uint numVertices = 4)
        {
            parentSegment = parent;
            parentSegmentSideNum = sideNum;
            Uvls = new FixVector[numVertices];
        }

        public Wall Wall { get; set; }
        public Segment ConnectedSegment { get; set; }
        public LevelTexture BaseTexture { get; set; }
        public LevelTexture OverlayTexture { get; set; }
        public FixVector[] Uvls { get; }

        // Indicates if this side is the end of an exit tunnel (only valid in D1 and D2 levels)
        public bool Exit { get; set; } = false;

        #region Read-only convenience properties
        public FixVector Center
        {
            get
            {
                var vertices = GetAllVertices();
                return new FixVector(
                    x: vertices.Average(v => v.X),
                    y: vertices.Average(v => v.Y),
                    z: vertices.Average(v => v.Z)
                    );
            }
        }

        public FixVector Normal { get; }

        // Indicates if there is a visible texture on this side
        public bool IsVisible { get; }

        // Indicates if there is a transparent texture on this side
        public bool IsTransparent { get; }
        #endregion

        public int GetNumVertices() => Uvls.Length;

        public LevelVertex GetVertex(int v) => parentSegment.GetVertex(parentSegmentSideNum, v);

        // Slow, consider optimizing if it's needed often
        internal LevelVertex[] GetAllVertices()
        {
            LevelVertex[] vertices = new LevelVertex[GetNumVertices()];
            for (int v = 0; v < vertices.Length; v++)
            {
                vertices[v] = GetVertex(v);
            }
            return vertices;
        }

        public Side GetOppositeSide() => parentSegment.GetOppositeSide(parentSegmentSideNum);

        public Side GetJoinedSide()
        {
            if (ConnectedSegment == null)
            {
                return null;
            }

            var vertices = GetAllVertices();

            // An exception will be thrown if only this side is connected.
            return ConnectedSegment.Sides.First(otherSide =>
            {
                if (otherSide.ConnectedSegment != parentSegment || GetNumVertices() != otherSide.GetNumVertices())
                {
                    return false;
                }

                // Do a vertex test to handle cases where multiple sides are joined (the segment will be illegal,
                // but we still want predictable behavior)
                for (int v = 0; v < otherSide.GetNumVertices(); v++)
                {
                    if (!vertices.Contains(otherSide.GetVertex(v)))
                    {
                        return false;
                    }
                }
                return true;
            });
        }

        public Side GetNeighbor(Edge atEdge) => parentSegment.GetSideNeighbor(parentSegmentSideNum, atEdge);

        public Side GetNeighbor(Edge atEdge, Func<Side, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Side GetVisibleNeighbor(Edge atEdge) => GetNeighbor(atEdge, side => side.IsVisible);

        public IEnumerable<LevelVertex> GetSharedVertices(Side other)
        {
            throw new NotImplementedException();
        }
    }
}
