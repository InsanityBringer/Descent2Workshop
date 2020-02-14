using LibDescent.Data;
using NUnit.Framework;

namespace LibDescent.Tests
{
    class RdlTests
    {
        private D1Level level;

        [SetUp]
        public void Setup()
        {
            var stream = GetType().Assembly.GetManifestResourceStream(GetType(), "test.rdl");
            level = D1Level.CreateFromStream(stream);
        }

        [Test]
        public void TestLoadLevelSucceeds()
        {
            // Level already loaded by Setup, just check it's there
            Assert.NotNull(level);
        }

        [Test]
        public void TestLevelName()
        {
            Assert.AreEqual("test", level.LevelName);
        }

        [Test]
        public void TestVertices()
        {
            Assert.AreEqual(68, level.Vertices.Count);

            // Vertex 0
            Assert.AreEqual(new FixVector(10d, 0d, -80d), level.Vertices[0].Location);
            Assert.AreEqual(1, level.Vertices[0].ConnectedSegments.Count);
            var connectedSegments = level.Vertices[0].ConnectedSegments;
            Assert.AreEqual(0, level.Segments.IndexOf(connectedSegments[0].segment));
            Assert.AreEqual(0, connectedSegments[0].vertexNum);
            Assert.AreEqual(3, level.Vertices[0].ConnectedSides.Count);

            // Vertex 30
            Assert.AreEqual(new FixVector(70d, 0d, 0d), level.Vertices[30].Location);
            Assert.AreEqual(3, level.Vertices[30].ConnectedSegments.Count);
            Assert.AreEqual(9, level.Vertices[30].ConnectedSides.Count);
            connectedSegments = level.Vertices[30].ConnectedSegments;
            Assert.AreEqual(7, level.Segments.IndexOf(connectedSegments[0].segment));
            Assert.AreEqual(8, level.Segments.IndexOf(connectedSegments[1].segment));
            Assert.AreEqual(15, level.Segments.IndexOf(connectedSegments[2].segment));

            // Vertex 57
            Assert.AreEqual(new FixVector(50d, -20d, 60d), level.Vertices[57].Location);
            Assert.AreEqual(1, level.Vertices[57].ConnectedSegments.Count);
            Assert.AreEqual(3, level.Vertices[57].ConnectedSides.Count);
            connectedSegments = level.Vertices[57].ConnectedSegments;
            Assert.AreEqual(14, level.Segments.IndexOf(connectedSegments[0].segment));

            // Vertex 67
            Assert.AreEqual(new FixVector(-50d, 20d, 60d), level.Vertices[67].Location);
            Assert.AreEqual(1, level.Vertices[67].ConnectedSegments.Count);
            connectedSegments = level.Vertices[67].ConnectedSegments;
            Assert.AreEqual(17, level.Segments.IndexOf(connectedSegments[0].segment));
            Assert.AreEqual(7, connectedSegments[0].vertexNum);
            Assert.AreEqual(3, level.Vertices[67].ConnectedSides.Count);
            var connectedSides = level.Vertices[67].ConnectedSides;
            Assert.AreEqual(level.Segments[17].GetSide(SegSide.Left), connectedSides[0].side);
            Assert.AreEqual(0, connectedSides[0].vertexNum);
            Assert.AreEqual(level.Segments[17].GetSide(SegSide.Up), connectedSides[1].side);
            Assert.AreEqual(2, connectedSides[1].vertexNum);
            Assert.AreEqual(level.Segments[17].GetSide(SegSide.Back), connectedSides[2].side);
            Assert.AreEqual(3, connectedSides[2].vertexNum);
        }

        [Test]
        public void TestSegments()
        {
            Assert.AreEqual(18, level.Segments.Count);

            Assert.AreSame(level.Vertices[0], level.Segments[0].Vertices[0]);
            Assert.AreEqual(SegFunction.None, level.Segments[0].Function);
            Assert.AreEqual(9.938d, level.Segments[0].Light, 0.001);

            Assert.AreEqual(SegFunction.FuelCenter, level.Segments[13].Function);
            Assert.AreEqual(12.0d, level.Segments[13].Light, 0.001);

            Assert.AreEqual(SegFunction.Reactor, level.Segments[14].Function);
            Assert.AreEqual(6.115d, level.Segments[14].Light, 0.001);
        }

        [Test]
        public void TestEndOfExitTunnel()
        {
            // Exit tunnel side - true
            Assert.IsTrue(level.Segments[17].GetSide(SegSide.Back).Exit);

            // Joined side - false
            Assert.IsFalse(level.Segments[0].GetSide(SegSide.Back).Exit);

            // Unjoined side - false
            Assert.IsFalse(level.Segments[17].GetSide(SegSide.Left).Exit);
        }

        [Test]
        public void TestObjects()
        {
            Assert.AreEqual(5, level.Objects.Count);

            // Player
            Assert.AreEqual(ObjectType.Player, level.Objects[0].type);
            Assert.AreEqual(0, level.Objects[0].id);
            Assert.AreEqual(MovementType.None, level.Objects[0].moveType);
            Assert.AreEqual(ControlType.Slew, level.Objects[0].controlType);
            Assert.AreEqual(new FixVector(0, -10, -70), level.Objects[0].position);
            // Point directly forward
            var expectedOrientation = new FixMatrix(new FixVector(0, 0, 1), new FixVector(0, 1, 0), new FixVector(1, 0, 0));
            Assert.AreEqual(expectedOrientation, level.Objects[0].orientation);
            Assert.AreEqual(43, level.Objects[0].modelInfo.modelNum);

            // Powerup
            Assert.AreEqual(ObjectType.Powerup, level.Objects[1].type);

            // Reactor
            Assert.AreEqual(ObjectType.ControlCenter, level.Objects[2].type);

            // Hostage
            Assert.AreEqual(ObjectType.Hostage, level.Objects[3].type);

            // Robot
            Assert.AreEqual(ObjectType.Robot, level.Objects[4].type);
        }

        [Test]
        [Ignore("Not completed yet")]
        public void TestWalls()
        {

        }

        [Test]
        [Ignore("Not completed yet")]
        public void TestTriggers()
        {

        }

        [Test]
        [Ignore("Not completed yet")]
        public void TestReactorTriggers()
        {

        }

        [Test]
        [Ignore("Not completed yet")]
        public void TestMatcens()
        {

        }
    }
}
