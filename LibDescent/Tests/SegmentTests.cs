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
    }
}