using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace LibDescent.Data
{
    public enum Edge
    {
        Right,
        Bottom,
        Left,
        Top
    }

    public enum TriangulationType
    {
        None,
        Tri012_230,
        Tri013_123
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

        public TriangulationType Triangulation
        {
            get
            {
                if (GetNumVertices() < 3)
                    throw new InvalidOperationException($"Illegal vertex count {GetNumVertices()}");
                else if (GetNumVertices() == 3)
                    return TriangulationType.None;

                var vertices = GetAllVertices().ToList().ConvertAll(v => (Vector3)v.Location);
                var triangle012 = Plane.CreateFromVertices(vertices[0], vertices[1], vertices[2]);
                double dot = Vector3.Dot(triangle012.Normal, vertices[3] - vertices[1]);
                if (Math.Abs(dot) < 0.0001)
                    return TriangulationType.None;
                else if (dot > 0)
                    return TriangulationType.Tri012_230;
                else
                    return TriangulationType.Tri013_123;
            }
        }

        public FixVector Normal
        {
            get
            {
                var normals = Normals;
                return (normals.Item1 + normals.Item2).Scale(0.5);
            }
        }

        public Tuple<FixVector, FixVector> Normals
        {
            get
            {
                Tuple<FixVector, FixVector> result;
                var vertices = GetAllVertices().ToList().ConvertAll(v => (Vector3)v.Location);
                switch (Triangulation)
                {
                    case TriangulationType.None:
                        {
                            var normal = Plane.CreateFromVertices(vertices[0], vertices[1], vertices[2]).Normal;
                            result = new Tuple<FixVector, FixVector>(normal, normal);
                        }
                        break;

                    case TriangulationType.Tri012_230:
                        {
                            var normal1 = Plane.CreateFromVertices(vertices[0], vertices[1], vertices[2]).Normal;
                            var normal2 = Plane.CreateFromVertices(vertices[2], vertices[3], vertices[0]).Normal;
                            result = new Tuple<FixVector, FixVector>(normal1, normal2);
                        }
                        break;

                    case TriangulationType.Tri013_123:
                        {
                            var normal1 = Plane.CreateFromVertices(vertices[0], vertices[1], vertices[3]).Normal;
                            var normal2 = Plane.CreateFromVertices(vertices[1], vertices[2], vertices[3]).Normal;
                            result = new Tuple<FixVector, FixVector>(normal1, normal2);
                        }
                        break;

                    default:
                        throw new InvalidOperationException("Received bad value from Triangulation property");
                }
                return result;
            }
        }

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
