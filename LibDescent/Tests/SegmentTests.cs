using LibDescent.Data;
using NUnit.Framework;
using System.Linq;
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
            Assert.AreEqual(8, segment.Vertices.Length);
            Assert.AreEqual(new FixVector(10d, 10d, -10d), segment.Vertices[0].Location);
            Assert.AreEqual(new FixVector(10d, -10d, -10d), segment.Vertices[1].Location);
            Assert.AreEqual(new FixVector(-10d, -10d, -10d), segment.Vertices[2].Location);
            Assert.AreEqual(new FixVector(-10d, 10d, -10d), segment.Vertices[3].Location);
            Assert.AreEqual(new FixVector(10d, 10d, 10d), segment.Vertices[4].Location);
            Assert.AreEqual(new FixVector(10d, -10d, 10d), segment.Vertices[5].Location);
            Assert.AreEqual(new FixVector(-10d, -10d, 10d), segment.Vertices[6].Location);
            Assert.AreEqual(new FixVector(-10d, 10d, 10d), segment.Vertices[7].Location);
        }

        [Test]
        public void TestSideVertices()
        {
            var segment = CreateDefaultSegment();
            Assert.AreEqual(6, segment.Sides.Length);
            Assert.AreEqual(4, segment.Sides[0].GetNumVertices());
            Assert.AreSame(segment.Vertices[7], segment.Sides[0].GetVertex(0));
            Assert.AreSame(segment.Vertices[6], segment.Sides[0].GetVertex(1));
            Assert.AreSame(segment.Vertices[2], segment.Sides[0].GetVertex(2));
            Assert.AreSame(segment.Vertices[3], segment.Sides[0].GetVertex(3));
            Assert.AreEqual(4, segment.Sides[1].GetNumVertices());
            Assert.AreSame(segment.Vertices[0], segment.Sides[1].GetVertex(0));
            Assert.AreSame(segment.Vertices[4], segment.Sides[1].GetVertex(1));
            Assert.AreSame(segment.Vertices[7], segment.Sides[1].GetVertex(2));
            Assert.AreSame(segment.Vertices[3], segment.Sides[1].GetVertex(3));
            Assert.AreEqual(4, segment.Sides[2].GetNumVertices());
            Assert.AreSame(segment.Vertices[0], segment.Sides[2].GetVertex(0));
            Assert.AreSame(segment.Vertices[1], segment.Sides[2].GetVertex(1));
            Assert.AreSame(segment.Vertices[5], segment.Sides[2].GetVertex(2));
            Assert.AreSame(segment.Vertices[4], segment.Sides[2].GetVertex(3));
            Assert.AreEqual(4, segment.Sides[3].GetNumVertices());
            Assert.AreSame(segment.Vertices[2], segment.Sides[3].GetVertex(0));
            Assert.AreSame(segment.Vertices[6], segment.Sides[3].GetVertex(1));
            Assert.AreSame(segment.Vertices[5], segment.Sides[3].GetVertex(2));
            Assert.AreSame(segment.Vertices[1], segment.Sides[3].GetVertex(3));
            Assert.AreEqual(4, segment.Sides[4].GetNumVertices());
            Assert.AreSame(segment.Vertices[4], segment.Sides[4].GetVertex(0));
            Assert.AreSame(segment.Vertices[5], segment.Sides[4].GetVertex(1));
            Assert.AreSame(segment.Vertices[6], segment.Sides[4].GetVertex(2));
            Assert.AreSame(segment.Vertices[7], segment.Sides[4].GetVertex(3));
            Assert.AreEqual(4, segment.Sides[5].GetNumVertices());
            Assert.AreSame(segment.Vertices[3], segment.Sides[5].GetVertex(0));
            Assert.AreSame(segment.Vertices[2], segment.Sides[5].GetVertex(1));
            Assert.AreSame(segment.Vertices[1], segment.Sides[5].GetVertex(2));
            Assert.AreSame(segment.Vertices[0], segment.Sides[5].GetVertex(3));
        }

        // This test could really do with a diagram to illustrate the expected layout of a segment...
        [Test]
        public void TestSideNeighbors()
        {
            var segment = CreateDefaultSegment();

            Assert.AreSame(segment.GetSide(SegSide.Back), segment.GetSide(SegSide.Left).GetNeighbor(Edge.Right));
            Assert.AreSame(segment.GetSide(SegSide.Down), segment.GetSide(SegSide.Left).GetNeighbor(Edge.Bottom));
            Assert.AreSame(segment.GetSide(SegSide.Front), segment.GetSide(SegSide.Left).GetNeighbor(Edge.Left));
            Assert.AreSame(segment.GetSide(SegSide.Up), segment.GetSide(SegSide.Left).GetNeighbor(Edge.Top));

            Assert.AreSame(segment.GetSide(SegSide.Right), segment.GetSide(SegSide.Up).GetNeighbor(Edge.Right));
            Assert.AreSame(segment.GetSide(SegSide.Back), segment.GetSide(SegSide.Up).GetNeighbor(Edge.Bottom));
            Assert.AreSame(segment.GetSide(SegSide.Left), segment.GetSide(SegSide.Up).GetNeighbor(Edge.Left));
            Assert.AreSame(segment.GetSide(SegSide.Front), segment.GetSide(SegSide.Up).GetNeighbor(Edge.Top));

            Assert.AreSame(segment.GetSide(SegSide.Front), segment.GetSide(SegSide.Right).GetNeighbor(Edge.Right));
            Assert.AreSame(segment.GetSide(SegSide.Down), segment.GetSide(SegSide.Right).GetNeighbor(Edge.Bottom));
            Assert.AreSame(segment.GetSide(SegSide.Back), segment.GetSide(SegSide.Right).GetNeighbor(Edge.Left));
            Assert.AreSame(segment.GetSide(SegSide.Up), segment.GetSide(SegSide.Right).GetNeighbor(Edge.Top));

            Assert.AreSame(segment.GetSide(SegSide.Left), segment.GetSide(SegSide.Down).GetNeighbor(Edge.Right));
            Assert.AreSame(segment.GetSide(SegSide.Back), segment.GetSide(SegSide.Down).GetNeighbor(Edge.Bottom));
            Assert.AreSame(segment.GetSide(SegSide.Right), segment.GetSide(SegSide.Down).GetNeighbor(Edge.Left));
            Assert.AreSame(segment.GetSide(SegSide.Front), segment.GetSide(SegSide.Down).GetNeighbor(Edge.Top));

            Assert.AreSame(segment.GetSide(SegSide.Right), segment.GetSide(SegSide.Back).GetNeighbor(Edge.Right));
            Assert.AreSame(segment.GetSide(SegSide.Down), segment.GetSide(SegSide.Back).GetNeighbor(Edge.Bottom));
            Assert.AreSame(segment.GetSide(SegSide.Left), segment.GetSide(SegSide.Back).GetNeighbor(Edge.Left));
            Assert.AreSame(segment.GetSide(SegSide.Up), segment.GetSide(SegSide.Back).GetNeighbor(Edge.Top));

            Assert.AreSame(segment.GetSide(SegSide.Left), segment.GetSide(SegSide.Front).GetNeighbor(Edge.Right));
            Assert.AreSame(segment.GetSide(SegSide.Down), segment.GetSide(SegSide.Front).GetNeighbor(Edge.Bottom));
            Assert.AreSame(segment.GetSide(SegSide.Right), segment.GetSide(SegSide.Front).GetNeighbor(Edge.Left));
            Assert.AreSame(segment.GetSide(SegSide.Up), segment.GetSide(SegSide.Front).GetNeighbor(Edge.Top));
        }

        [Test]
        public void TestOppositeSides()
        {
            var segment = CreateDefaultSegment();

            Assert.AreSame(segment.GetSide(SegSide.Right), segment.GetSide(SegSide.Left).GetOppositeSide());
            Assert.AreSame(segment.GetSide(SegSide.Down), segment.GetSide(SegSide.Up).GetOppositeSide());
            Assert.AreSame(segment.GetSide(SegSide.Left), segment.GetSide(SegSide.Right).GetOppositeSide());
            Assert.AreSame(segment.GetSide(SegSide.Up), segment.GetSide(SegSide.Down).GetOppositeSide());
            Assert.AreSame(segment.GetSide(SegSide.Front), segment.GetSide(SegSide.Back).GetOppositeSide());
            Assert.AreSame(segment.GetSide(SegSide.Back), segment.GetSide(SegSide.Front).GetOppositeSide());
        }

        [Test]
        public void TestCenters()
        {
            var segment = CreateDefaultSegment();

            // Move 5 units to the right to force the properties to return non-default values
            foreach (var vertex in segment.Vertices)
            {
                vertex.X += 5.0d;
            }

            // Center of the segment
            Assert.AreEqual(new FixVector(5d, 0d, 0d), segment.Center);

            // Center of each side
            Assert.AreEqual(new FixVector(-5d, 0d, 0d), segment.Sides[0].Center);
            Assert.AreEqual(new FixVector(5d, 10d, 0d), segment.Sides[1].Center);
            Assert.AreEqual(new FixVector(15d, 0d, 0d), segment.Sides[2].Center);
            Assert.AreEqual(new FixVector(5d, -10d, 0d), segment.Sides[3].Center);
            Assert.AreEqual(new FixVector(5d, 0d, 10d), segment.Sides[4].Center);
            Assert.AreEqual(new FixVector(5d, 0d, -10d), segment.Sides[5].Center);
        }

        [Test]
        public void TestNormals()
        {
            // Default segment
            var segment = CreateDefaultSegment();

            Assert.AreEqual(new FixVector(1d, 0d, 0d), segment.Sides[0].Normal);
            Assert.AreEqual(new FixVector(0d, -1d, 0d), segment.Sides[1].Normal);
            Assert.AreEqual(new FixVector(-1d, 0d, 0d), segment.Sides[2].Normal);
            Assert.AreEqual(new FixVector(0d, 1d, 0d), segment.Sides[3].Normal);
            Assert.AreEqual(new FixVector(0d, 0d, -1d), segment.Sides[4].Normal);
            Assert.AreEqual(new FixVector(0d, 0d, 1d), segment.Sides[5].Normal);

            // Warped segment
            segment.Vertices[6].X = -30d;
            segment.Vertices[7].X = -30d;

            Assert.AreEqual(new FixVector(0d, 0d, 1d), segment.GetSide(SegSide.Front).Normal);
            Assert.AreEqual(new FixVector(0d, 0d, -1d), segment.GetSide(SegSide.Back).Normal);
            var expectedNormal = new FixVector(0.7071d, 0d, -0.7071d);
            Assert.AreEqual(expectedNormal.x, segment.GetSide(SegSide.Left).Normal.x, 0.01);
            Assert.AreEqual(expectedNormal.y, segment.GetSide(SegSide.Left).Normal.y, 0.01);
            Assert.AreEqual(expectedNormal.z, segment.GetSide(SegSide.Left).Normal.z, 0.01);
        }

        [Test]
        public void TestMeasurements()
        {
            // Default segment
            var segment = CreateDefaultSegment();

            Assert.AreEqual(20d, (double)segment.Length);
            Assert.AreEqual(20d, (double)segment.Width);
            Assert.AreEqual(20d, (double)segment.Height);

            // Warped segment
            segment.Vertices[6].X = -30d;
            segment.Vertices[7].X = -30d;

            Assert.AreEqual(22.36d, segment.Length, 0.01);
            Assert.AreEqual(30d, (double)segment.Width);
            Assert.AreEqual(20d, (double)segment.Height);
        }

        [Test]
        public void TestSegmentType()
        {
            var segment = CreateDefaultSegment();

            segment.special = 1;
            Assert.AreEqual(SegFunction.FuelCenter, segment.Function);

            segment.Function = SegFunction.BlueGoal;
            Assert.AreEqual(5, segment.special);
        }

        private Block CreateTestBlock()
        {
            var blockStream = GetType().Assembly.GetManifestResourceStream(GetType(), "test.blx");
            return Block.FromStream(blockStream);
        }

        [Test]
        public void TestGetConnectedSegment()
        {
            var segments = CreateTestBlock().Segments;

            Assert.AreSame(segments[1], segments[0].GetSide(SegSide.Back).ConnectedSegment);
            Assert.IsNull(segments[0].GetSide(SegSide.Left).ConnectedSegment);
        }

        [Test]
        public void TestGetJoinedSide()
        {
            var segments = CreateTestBlock().Segments;

            Assert.AreSame(segments[1].GetSide(SegSide.Front), segments[0].GetSide(SegSide.Back).GetJoinedSide());
            Assert.IsNull(segments[0].GetSide(SegSide.Left).GetJoinedSide());
        }

        [Test]
        public void TestGetVisibleNeighbor()
        {
            var segments = CreateTestBlock().Segments;

            // Basic stuff
            Assert.AreSame(segments[1].GetSide(SegSide.Down), segments[0].GetSide(SegSide.Down).GetVisibleNeighbor(Edge.Bottom));
            Assert.AreSame(segments[0].GetSide(SegSide.Down), segments[0].GetSide(SegSide.Back).GetVisibleNeighbor(Edge.Bottom));
            Assert.AreSame(segments[0].GetSide(SegSide.Down), segments[0].GetSide(SegSide.Left).GetVisibleNeighbor(Edge.Bottom));

            // Wrap around corners
            Assert.AreSame(segments[3].GetSide(SegSide.Front), segments[0].GetSide(SegSide.Left).GetVisibleNeighbor(Edge.Right));

            // Quad-join
            Assert.IsNull(segments[1].GetSide(SegSide.Back).GetVisibleNeighbor(Edge.Left));

            // Visible walls
            Assert.AreSame(segments[3].GetSide(SegSide.Left), segments[3].GetSide(SegSide.Front).GetVisibleNeighbor(Edge.Right));

            // Invisible walls
            Assert.AreSame(segments[5].GetSide(SegSide.Front), segments[0].GetSide(SegSide.Right).GetVisibleNeighbor(Edge.Left));
        }

        [Test]
        public void TestGetNeighborWithFilter()
        {
            var segments = CreateTestBlock().Segments;

            Assert.AreSame(segments[6].GetSide(SegSide.Right),
                segments[5].GetSide(SegSide.Right).GetNeighbor(Edge.Left, side => side.IsVisible && !side.IsTransparent));
        }

        [Test]
        public void TestGetSharedVerticesSegment()
        {
            var segments = CreateTestBlock().Segments;

            // Segments 0 and 1 (connected by one side)
            var expectedVertexNumbers = new int[] { 4, 5, 6, 7 };
            var expectedVertices = expectedVertexNumbers.ToList().ConvertAll(v => segments[0].Vertices[v]);
            Assert.That(Enumerable.SequenceEqual(expectedVertices, segments[0].GetSharedVertices(segments[1])));

            // Segments 0 and 3 (connected by one edge)
            expectedVertexNumbers = new int[] { 6, 7 };
            expectedVertices = expectedVertexNumbers.ToList().ConvertAll(v => segments[0].Vertices[v]);
            Assert.That(Enumerable.SequenceEqual(expectedVertices, segments[0].GetSharedVertices(segments[3])));

            // Segments 0 and 4 (not connected)
            Assert.IsEmpty(segments[0].GetSharedVertices(segments[4]));
        }

        [Test]
        public void TestGetSharedVerticesSide()
        {
            var segments = CreateTestBlock().Segments;

            // Same segment, adjacent
            var side = segments[0].GetSide(SegSide.Left);
            var otherSide = segments[0].GetSide(SegSide.Front);
            var expectedVertexNumbers = new int[] { 2, 3 };
            var expectedVertices = expectedVertexNumbers.ToList().ConvertAll(v => side.GetVertex(v));
            Assert.That(Enumerable.SequenceEqual(expectedVertices, side.GetSharedVertices(otherSide)));

            // Same segment, non-adjacent
            otherSide = segments[0].GetSide(SegSide.Right);
            Assert.IsEmpty(side.GetSharedVertices(otherSide));

            // Different segments, joined
            side = segments[0].GetSide(SegSide.Back);
            otherSide = segments[1].GetSide(SegSide.Front);
            expectedVertexNumbers = new int[] { 0, 1, 2, 3 };
            expectedVertices = expectedVertexNumbers.ToList().ConvertAll(v => side.GetVertex(v));
            Assert.That(Enumerable.SequenceEqual(expectedVertices, side.GetSharedVertices(otherSide)));

            // Different segments, adjacent
            side = segments[0].GetSide(SegSide.Right);
            otherSide = segments[1].GetSide(SegSide.Right);
            expectedVertexNumbers = new int[] { 2, 3 };
            expectedVertices = expectedVertexNumbers.ToList().ConvertAll(v => side.GetVertex(v));
            Assert.That(Enumerable.SequenceEqual(expectedVertices, side.GetSharedVertices(otherSide)));

            // Different segments, non-adjacent
            side = segments[0].GetSide(SegSide.Left);
            otherSide = segments[1].GetSide(SegSide.Back);
            Assert.IsEmpty(side.GetSharedVertices(otherSide));

            // Different segments, adjacent but not joined
            // First we have to disconnect the sides (note this works differently with levels; we're using a block here)
            segments[2].GetSide(SegSide.Left).ConnectedSegment = null;
            segments[4].GetSide(SegSide.Right).ConnectedSegment = null;
            side = segments[2].GetSide(SegSide.Back);
            otherSide = segments[4].GetSide(SegSide.Back);
            Assert.IsEmpty(side.GetSharedVertices(otherSide));
        }
    }
}