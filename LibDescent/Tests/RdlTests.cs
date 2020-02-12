using LibDescent.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
