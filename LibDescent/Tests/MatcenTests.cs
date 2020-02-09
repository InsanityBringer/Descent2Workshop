using LibDescent.Data;
using NUnit.Framework;

namespace LibDescent.Tests
{
    class MatcenTests
    {
        private IBlock block;

        [SetUp]
        public void Setup()
        {
            var stream = GetType().Assembly.GetManifestResourceStream(GetType(), "test.blx");
            block = ExtendedBlock.CreateFromStream(stream);
        }

        [Test]
        public void TestMatcenLinking()
        {
            var matcen = block.MatCenters[0];
            Assert.AreSame(matcen, block.Segments[8].MatCenter);
            Assert.AreSame(block.Segments[8], matcen.Segment);
            Assert.AreEqual(SegFunction.MatCenter, block.Segments[8].Function);
        }

        [Test]
        public void TestMatcenProperties()
        {
            var matcen = block.MatCenters[0];
            Assert.AreEqual(5.0, (double)matcen.Interval);
            Assert.AreEqual(500, (double)matcen.HitPoints);
        }

        [Test]
        public void TestMatcenRobotListDefaults()
        {
            var matcen = block.MatCenters[0];
            Assert.IsEmpty(matcen.SpawnedRobotIds);
        }
    }
}
