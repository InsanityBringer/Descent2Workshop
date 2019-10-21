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
            var xmlStream = GetType().Assembly.GetManifestResourceStream("DefaultSegment.xml");
            return Segment.FromXML(XDocument.Load(xmlStream).Root);
        }

        [Test]
        public void TestVertexPositions()
        {
            var segment = CreateDefaultSegment();
            Assert.AreEqual(segment.Vertices[0], new FixVector(20, 20, 20));
        }
    }
}