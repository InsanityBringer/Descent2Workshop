using System;

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
        public Side(uint numVertices = 4)
        {
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
        public FixVector Center { get; }

        public FixVector Normal { get; }

        // Indicates if there is a visible texture on this side
        public bool IsVisible { get; }

        // Indicates if there is a transparent texture on this side
        public bool IsTransparent { get; }
        #endregion

        public double GetNumVertices() => Uvls.Length;

        public FixVector GetVertex(int v)
        {
            throw new NotImplementedException();
        }

        public Side GetOppositeSide()
        {
            throw new NotImplementedException();
        }

        public Side GetJoinedSide()
        {
            throw new NotImplementedException();
        }

        public Side GetNeighbor(Edge atEdge)
        {
            throw new NotImplementedException();
        }

        public Side GetNeighbor(Edge atEdge, Func<Side, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Side GetVisibleNeighbor(Edge atEdge) => GetNeighbor(atEdge, side => side.IsVisible);
    }
}
