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
        public void TestEndOfExitTunnel()
        {
            // Exit tunnel side - true
            Assert.IsTrue(level.Segments[17].GetSide(SegSide.Back).Exit);

            // Joined side - false
            Assert.IsFalse(level.Segments[0].GetSide(SegSide.Back).Exit);

            // Unjoined side - false
            Assert.IsFalse(level.Segments[17].GetSide(SegSide.Left).Exit);
        }
    }
}
