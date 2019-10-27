using System.Collections.Generic;

namespace LibDescent.Data
{
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
    }
}
