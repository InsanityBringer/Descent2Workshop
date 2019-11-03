using LibDescent.Data;
using NUnit.Framework;
using System.Xml.Linq;

namespace LibDescent.Tests
{
    public class SegmentTests
    {
        [SetUp]
        public void Setup()
        {
        }

        private Segment CreateDefaultSegment()
        {
            var xmlStream = GetType().Assembly.GetManifestResourceStream(GetType(), "DefaultSegment.xml");
            return Segment.FromXML(XDocument.Load(xmlStream).Root);
        }

        [Test]
        public void TestVertexPositions()
        {
            var segment = CreateDefaultSegment();
            Assert.AreEqual(segment.Vertices.Length, 8);
            Assert.AreEqual(segment.Vertices[0], new FixVector(10d, 10d, -10d));
            Assert.AreEqual(segment.Vertices[1], new FixVector(10d, -10d, -10d));
            Assert.AreEqual(segment.Vertices[2], new FixVector(-10d, -10d, -10d));
            Assert.AreEqual(segment.Vertices[3], new FixVector(-10d, 10d, -10d));
            Assert.AreEqual(segment.Vertices[4], new FixVector(10d, 10d, 10d));
            Assert.AreEqual(segment.Vertices[5], new FixVector(10d, -10d, 10d));
            Assert.AreEqual(segment.Vertices[6], new FixVector(-10d, -10d, 10d));
            Assert.AreEqual(segment.Vertices[7], new FixVector(-10d, 10d, 10d));
        }

        [Test]
        public void TestSideVertices()
        {
            var segment = CreateDefaultSegment();
            Assert.AreEqual(segment.Sides.Length, 6);
            Assert.AreEqual(segment.Sides[0].GetNumVertices(), 4);
            Assert.AreSame(segment.Sides[0].GetVertex(0), segment.Vertices[7]);
            Assert.AreSame(segment.Sides[0].GetVertex(1), segment.Vertices[6]);
            Assert.AreSame(segment.Sides[0].GetVertex(2), segment.Vertices[2]);
            Assert.AreSame(segment.Sides[0].GetVertex(3), segment.Vertices[3]);
            Assert.AreEqual(segment.Sides[1].GetNumVertices(), 4);
            Assert.AreSame(segment.Sides[1].GetVertex(0), segment.Vertices[0]);
            Assert.AreSame(segment.Sides[1].GetVertex(1), segment.Vertices[4]);
            Assert.AreSame(segment.Sides[1].GetVertex(2), segment.Vertices[7]);
            Assert.AreSame(segment.Sides[1].GetVertex(3), segment.Vertices[3]);
            Assert.AreEqual(segment.Sides[2].GetNumVertices(), 4);
            Assert.AreSame(segment.Sides[2].GetVertex(0), segment.Vertices[0]);
            Assert.AreSame(segment.Sides[2].GetVertex(1), segment.Vertices[1]);
            Assert.AreSame(segment.Sides[2].GetVertex(2), segment.Vertices[5]);
            Assert.AreSame(segment.Sides[2].GetVertex(3), segment.Vertices[4]);
            Assert.AreEqual(segment.Sides[3].GetNumVertices(), 4);
            Assert.AreSame(segment.Sides[3].GetVertex(0), segment.Vertices[2]);
            Assert.AreSame(segment.Sides[3].GetVertex(1), segment.Vertices[6]);
            Assert.AreSame(segment.Sides[3].GetVertex(2), segment.Vertices[5]);
            Assert.AreSame(segment.Sides[3].GetVertex(3), segment.Vertices[1]);
            Assert.AreEqual(segment.Sides[4].GetNumVertices(), 4);
            Assert.AreSame(segment.Sides[4].GetVertex(0), segment.Vertices[4]);
            Assert.AreSame(segment.Sides[4].GetVertex(1), segment.Vertices[5]);
            Assert.AreSame(segment.Sides[4].GetVertex(2), segment.Vertices[6]);
            Assert.AreSame(segment.Sides[4].GetVertex(3), segment.Vertices[7]);
            Assert.AreEqual(segment.Sides[5].GetNumVertices(), 4);
            Assert.AreSame(segment.Sides[5].GetVertex(0), segment.Vertices[3]);
            Assert.AreSame(segment.Sides[5].GetVertex(1), segment.Vertices[2]);
            Assert.AreSame(segment.Sides[5].GetVertex(2), segment.Vertices[1]);
            Assert.AreSame(segment.Sides[5].GetVertex(3), segment.Vertices[0]);
        }

        [Test]
        public void TestSideNeighbors()
        {
            var segment = CreateDefaultSegment();

            Assert.AreSame(segment.GetSide(SegSide.Left).GetNeighbor(Edge.Right), segment.GetSide(SegSide.Front));
            Assert.AreSame(segment.GetSide(SegSide.Left).GetNeighbor(Edge.Bottom), segment.GetSide(SegSide.Down));
            Assert.AreSame(segment.GetSide(SegSide.Left).GetNeighbor(Edge.Left), segment.GetSide(SegSide.Back));
            Assert.AreSame(segment.GetSide(SegSide.Left).GetNeighbor(Edge.Top), segment.GetSide(SegSide.Up));

            Assert.AreSame(segment.GetSide(SegSide.Up).GetNeighbor(Edge.Right), segment.GetSide(SegSide.Left));
            Assert.AreSame(segment.GetSide(SegSide.Up).GetNeighbor(Edge.Bottom), segment.GetSide(SegSide.Back));
            Assert.AreSame(segment.GetSide(SegSide.Up).GetNeighbor(Edge.Left), segment.GetSide(SegSide.Right));
            Assert.AreSame(segment.GetSide(SegSide.Up).GetNeighbor(Edge.Top), segment.GetSide(SegSide.Front));

            Assert.AreSame(segment.GetSide(SegSide.Right).GetNeighbor(Edge.Right), segment.GetSide(SegSide.Back));
            Assert.AreSame(segment.GetSide(SegSide.Right).GetNeighbor(Edge.Bottom), segment.GetSide(SegSide.Down));
            Assert.AreSame(segment.GetSide(SegSide.Right).GetNeighbor(Edge.Left), segment.GetSide(SegSide.Front));
            Assert.AreSame(segment.GetSide(SegSide.Right).GetNeighbor(Edge.Top), segment.GetSide(SegSide.Up));

            Assert.AreSame(segment.GetSide(SegSide.Down).GetNeighbor(Edge.Right), segment.GetSide(SegSide.Right));
            Assert.AreSame(segment.GetSide(SegSide.Down).GetNeighbor(Edge.Bottom), segment.GetSide(SegSide.Back));
            Assert.AreSame(segment.GetSide(SegSide.Down).GetNeighbor(Edge.Left), segment.GetSide(SegSide.Left));
            Assert.AreSame(segment.GetSide(SegSide.Down).GetNeighbor(Edge.Top), segment.GetSide(SegSide.Front));

            Assert.AreSame(segment.GetSide(SegSide.Back).GetNeighbor(Edge.Right), segment.GetSide(SegSide.Left));
            Assert.AreSame(segment.GetSide(SegSide.Back).GetNeighbor(Edge.Bottom), segment.GetSide(SegSide.Down));
            Assert.AreSame(segment.GetSide(SegSide.Back).GetNeighbor(Edge.Left), segment.GetSide(SegSide.Right));
            Assert.AreSame(segment.GetSide(SegSide.Back).GetNeighbor(Edge.Top), segment.GetSide(SegSide.Up));

            Assert.AreSame(segment.GetSide(SegSide.Front).GetNeighbor(Edge.Right), segment.GetSide(SegSide.Right));
            Assert.AreSame(segment.GetSide(SegSide.Front).GetNeighbor(Edge.Bottom), segment.GetSide(SegSide.Down));
            Assert.AreSame(segment.GetSide(SegSide.Front).GetNeighbor(Edge.Left), segment.GetSide(SegSide.Left));
            Assert.AreSame(segment.GetSide(SegSide.Front).GetNeighbor(Edge.Top), segment.GetSide(SegSide.Up));
        }

        [Test]
        public void TestOppositeSides()
        {
            var segment = CreateDefaultSegment();

            Assert.AreSame(segment.GetSide(SegSide.Left).GetOppositeSide(), segment.GetSide(SegSide.Right));
            Assert.AreSame(segment.GetSide(SegSide.Up).GetOppositeSide(), segment.GetSide(SegSide.Down));
            Assert.AreSame(segment.GetSide(SegSide.Right).GetOppositeSide(), segment.GetSide(SegSide.Left));
            Assert.AreSame(segment.GetSide(SegSide.Down).GetOppositeSide(), segment.GetSide(SegSide.Up));
            Assert.AreSame(segment.GetSide(SegSide.Back).GetOppositeSide(), segment.GetSide(SegSide.Front));
            Assert.AreSame(segment.GetSide(SegSide.Front).GetOppositeSide(), segment.GetSide(SegSide.Back));
        }

        [Test]
        public void TestCenters()
        {
            var segment = CreateDefaultSegment();

            // Center of the segment
            Assert.AreEqual(segment.Center, new FixVector(0d, 0d, 0d));

            // Center of each side
            Assert.AreEqual(segment.Sides[0].Center, new FixVector(-10d, 0d, 0d));
            Assert.AreEqual(segment.Sides[1].Center, new FixVector(0d, 10d, 0d));
            Assert.AreEqual(segment.Sides[2].Center, new FixVector(10d, 0d, 0d));
            Assert.AreEqual(segment.Sides[3].Center, new FixVector(0d, -10d, 0d));
            Assert.AreEqual(segment.Sides[4].Center, new FixVector(0d, 0d, 10d));
            Assert.AreEqual(segment.Sides[5].Center, new FixVector(0d, 0d, -10d));
        }

        [Test]
        public void TestNormals()
        {
            // Default segment
            var segment = CreateDefaultSegment();

            Assert.AreEqual(segment.Sides[0].Normal, new FixVector(1d, 0d, 0d));
            Assert.AreEqual(segment.Sides[1].Normal, new FixVector(0d, -1d, 0d));
            Assert.AreEqual(segment.Sides[2].Normal, new FixVector(-1d, 0d, 0d));
            Assert.AreEqual(segment.Sides[3].Normal, new FixVector(0d, 1d, 0d));
            Assert.AreEqual(segment.Sides[4].Normal, new FixVector(0d, 0d, -1d));
            Assert.AreEqual(segment.Sides[5].Normal, new FixVector(0d, 0d, 1d));

            // Warped segment
            segment.Vertices[2].x = -20d;
            segment.Vertices[6].x = -20d;

            Assert.AreEqual(segment.GetSide(SegSide.Front).Normal, new FixVector(0d, 0d, 1d));
            Assert.AreEqual(segment.GetSide(SegSide.Back).Normal, new FixVector(0d, 0d, -1d));
            var expectedNormal = new FixVector(0.7071d, 0d, -0.7071d);
            Assert.AreEqual(segment.GetSide(SegSide.Left).Normal.x, expectedNormal.x, 0.01);
            Assert.AreEqual(segment.GetSide(SegSide.Left).Normal.y, expectedNormal.y, 0.01);
            Assert.AreEqual(segment.GetSide(SegSide.Left).Normal.z, expectedNormal.z, 0.01);
        }

        [Test]
        public void TestMeasurements()
        {
            // Default segment
            var segment = CreateDefaultSegment();

            // Warped segment
        }

        [Test]
        public void TestSegmentType()
        {
            var segment = CreateDefaultSegment();

            // Simple set/get (still undecided exactly what the interface will look like)
            //segment.special = 1;
            //Assert.AreEqual(segment.Type, FuelCen);
        }

        private Block CreateTestBlock()
        {
            throw new System.NotImplementedException();
        }

        [Test]
        public void TestGetConnectedSegment()
        {
            var segments = CreateTestBlock().Segments;

            Assert.AreSame(segments[0].GetSide(SegSide.Front).ConnectedSegment, segments[1]);
            Assert.IsNull(segments[0].GetSide(SegSide.Left).ConnectedSegment);
        }

        [Test]
        public void TestGetJoinedSide()
        {
            var segments = CreateTestBlock().Segments;

            Assert.AreSame(segments[0].GetSide(SegSide.Front).GetJoinedSide(), segments[1].GetSide(SegSide.Back));
            Assert.IsNull(segments[0].GetSide(SegSide.Left).GetJoinedSide());
        }

        [Test]
        public void TestGetVisibleNeighbor()
        {
            var segments = CreateTestBlock().Segments;

            // Basic stuff
            Assert.AreSame(segments[0].GetSide(SegSide.Down).GetVisibleNeighbor(Edge.Top), segments[1].GetSide(SegSide.Down));
            Assert.AreSame(segments[0].GetSide(SegSide.Front).GetVisibleNeighbor(Edge.Bottom), segments[0].GetSide(SegSide.Down));
            Assert.AreSame(segments[0].GetSide(SegSide.Left).GetVisibleNeighbor(Edge.Bottom), segments[0].GetSide(SegSide.Down));

            // Wrap around corners
            Assert.AreSame(segments[0].GetSide(SegSide.Left).GetVisibleNeighbor(Edge.Right), segments[3].GetSide(SegSide.Back));

            // Quad-join
            Assert.IsNull(segments[1].GetSide(SegSide.Front).GetVisibleNeighbor(Edge.Left));

            // Visible walls
            Assert.AreSame(segments[3].GetSide(SegSide.Back).GetVisibleNeighbor(Edge.Right), segments[3].GetSide(SegSide.Left));

            // Invisible walls
            Assert.AreSame(segments[0].GetSide(SegSide.Right).GetVisibleNeighbor(Edge.Left), segments[5].GetSide(SegSide.Back));
        }

        [Test]
        public void TestGetNeighborWithFilter()
        {
            var segments = CreateTestBlock().Segments;

            Assert.AreSame(segments[5].GetSide(SegSide.Right).GetNeighbor(Edge.Left, side => side.IsVisible && !side.IsTransparent),
                segments[6].GetSide(SegSide.Right));
        }

        [Test]
        public void TestGetSharedVerticesSegment()
        {
            var segments = CreateTestBlock().Segments;

            // Segments 0 and 1 (connected by one side)

            // Segments 0 and 3 (connected by one edge)

            // Segments 0 and 4 (not connected)
        }

        [Test]
        public void TestGetSharedVerticesSide()
        {
            var segments = CreateTestBlock().Segments;

            // Same segment, adjacent

            // Same segment, non-adjacent

            // Different segments, joined

            // Different segments, adjacent

            // Different segments, non-adjacent
        }
    }
}