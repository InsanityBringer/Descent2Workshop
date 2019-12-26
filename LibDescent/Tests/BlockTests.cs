using LibDescent.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibDescent.Tests
{
    class BlockTests
    {
        private IBlock blkBlock;
        private IBlock blxBlock;

        [SetUp]
        public void Setup()
        {
            var blkStream = GetType().Assembly.GetManifestResourceStream(GetType(), "test.blk");
            blkBlock = Block.CreateFromStream(blkStream);

            var blxStream = GetType().Assembly.GetManifestResourceStream(GetType(), "test.blx");
            blxBlock = ExtendedBlock.CreateFromStream(blxStream);
        }

        private byte[] GetArrayFromResourceStream(string resourceName)
        {
            var resourceStream = GetType().Assembly.GetManifestResourceStream(GetType(), resourceName);
            var memoryStream = new MemoryStream();
            resourceStream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        [Test]
        public void TestBlockSegments()
        {
            IBlock[] blockFormats = { blkBlock, blxBlock };

            foreach (var block in blockFormats)
            {
                Assert.AreEqual(8, block.Segments.Count);
                Assert.IsNull(block.Segments[0].Sides[0].ConnectedSegment);
                Assert.AreSame(block.Segments[1], block.Segments[0].Sides[4].ConnectedSegment);
            }
        }

        [Test]
        public void TestBlockVertexLocations()
        {
            IBlock[] blockFormats = { blkBlock, blxBlock };

            foreach (var block in blockFormats)
            {
                Assert.AreEqual(new FixVector(0, 20, 0), block.Segments[0].Vertices[0].Location);
                Assert.AreEqual(new FixVector(20, -20, -60), block.Segments[7].Vertices[3].Location);
            }
        }

        [Test]
        public void TestBlockVertexMerging()
        {
            IBlock[] blockFormats = { blkBlock, blxBlock };

            foreach (var block in blockFormats)
            {
                Assert.AreEqual(32, block.GetVertexCount());
                Assert.AreSame(block.Segments[0].Vertices[7], block.Segments[1].Vertices[3]);
                Assert.AreSame(block.Segments[0].Vertices[7], block.Segments[3].Vertices[0]);
                Assert.AreEqual(3, block.Segments[0].Vertices[7].ConnectedSegments.Count);
                Assert.AreEqual(9, block.Segments[0].Vertices[7].ConnectedSides.Count);
            }
        }

        [Test]
        public void TestBlockTextures()
        {
            IBlock[] blockFormats = { blkBlock, blxBlock };

            foreach (var block in blockFormats)
            {
                Assert.AreEqual(42, block.Segments[2].Sides[4].BaseTexture.TextureIndex); // rock160
                Assert.AreEqual(new Uvl(0, 0, 0.41259765625), block.Segments[2].Sides[4].Uvls[0]);
                Assert.AreEqual(new Uvl(0, 1, 0.42266845703125), block.Segments[2].Sides[4].Uvls[1]);
                Assert.AreEqual(new Uvl(-1, 1, 0.53265380859375), block.Segments[2].Sides[4].Uvls[2]);
                Assert.AreEqual(new Uvl(-1, 0, 0.55279541015625), block.Segments[2].Sides[4].Uvls[3]);

                Assert.AreEqual(219, block.Segments[0].Sides[2].BaseTexture.TextureIndex); // metl046 (rotated)
                Assert.AreEqual(new Uvl(0, 0, 0.84368896484375), block.Segments[0].Sides[2].Uvls[0]);
                Assert.AreEqual(new Uvl(1, 0, 0.831085205078125), block.Segments[0].Sides[2].Uvls[1]);
                Assert.AreEqual(new Uvl(1, 1, 0.384918212890625), block.Segments[0].Sides[2].Uvls[2]);
                Assert.AreEqual(new Uvl(0, 1, 0.384918212890625), block.Segments[0].Sides[2].Uvls[3]);

                Assert.AreEqual(0, block.Segments[1].Sides[0].BaseTexture.TextureIndex); // rock021 (no wall)
                Assert.AreEqual(new Uvl(0, 0, 0.214813232421875), block.Segments[1].Sides[0].Uvls[0]);
                Assert.AreEqual(new Uvl(0, 0, 0.214813232421875), block.Segments[1].Sides[0].Uvls[1]);
                Assert.AreEqual(new Uvl(0, 0, 0.302093505859375), block.Segments[1].Sides[0].Uvls[2]);
                Assert.AreEqual(new Uvl(0, 0, 0.302093505859375), block.Segments[1].Sides[0].Uvls[3]);
            }
        }

        [Test]
        public void TestBlockSecondaryTextures()
        {
            IBlock[] blockFormats = { blkBlock, blxBlock };

            foreach (var block in blockFormats)
            {
                Assert.AreEqual(341, block.Segments[2].Sides[4].OverlayTexture.TextureIndex); // misc086 (rotated 180 degrees)
                Assert.AreEqual(OverlayRotation.Rotate180, block.Segments[2].Sides[4].OverlayRotation);

                Assert.AreEqual(268, block.Segments[0].Sides[2].OverlayTexture.TextureIndex); // metl153
                Assert.AreEqual(OverlayRotation.Rotate0, block.Segments[0].Sides[2].OverlayRotation);

                Assert.AreEqual(0, block.Segments[1].Sides[0].OverlayTexture.TextureIndex); // rock021 (no wall)
                Assert.AreEqual(OverlayRotation.Rotate0, block.Segments[1].Sides[0].OverlayRotation);
            }
        }

        [Test]
        public void TestStandardBlockSegmentLightValues()
        {
            IBlock[] blockFormats = { blkBlock, blxBlock };

            foreach (var block in blockFormats)
            {
                Assert.AreEqual(3.501, block.Segments[0].Light, 0.001);
                Assert.AreEqual(1.421, block.Segments[1].Light, 0.001);
                Assert.AreEqual(0.203, block.Segments[7].Light, 0.001);
            }
        }

        [Test]
        public void TestStandardBlockWallsEmpty()
        {
            Assert.IsEmpty(blkBlock.Walls);
        }

        [Test]
        public void TestStandardBlockTriggersEmpty()
        {
            //Assert.IsEmpty(blkBlock.Triggers);
            Assert.Fail();
        }

        [Test]
        public void TestStandardBlockAnimatedLightsEmpty()
        {
            //Assert.IsEmpty(blkBlock.AnimatedLights);
            Assert.Fail();
        }

        [Test]
        public void TestStandardBlockWrite()
        {
            var stream = new MemoryStream();
            Assert.DoesNotThrow(() => blkBlock.WriteToStream(stream));

            var originalFileContents = GetArrayFromResourceStream("test.blk");
            Assert.IsTrue(Enumerable.SequenceEqual(originalFileContents, stream.ToArray()));
        }

        [Test]
        public void TestExtendedBlockWalls()
        {
            Assert.AreEqual(7, blxBlock.Walls.Count);

            // Open wall (trigger) on right side of segment 1
            Assert.IsNotNull(blxBlock.Segments[1].GetSide(SegSide.Right).Wall);
            var wall = blxBlock.Segments[1].GetSide(SegSide.Right).Wall;
            Assert.AreEqual(WallType.Open, wall.Type);
            Assert.Contains(wall, blxBlock.Walls);
            Assert.IsNull(wall.OppositeWall);

            // Opposite side (left side of segment 5) has no wall
            Assert.IsNull(blxBlock.Segments[5].GetSide(SegSide.Left).Wall);

            // Door on left side of segment 3 - opposite side is in segment 7
            Assert.IsNotNull(blxBlock.Segments[3].GetSide(SegSide.Left).Wall);
            wall = blxBlock.Segments[3].GetSide(SegSide.Left).Wall;
            Assert.AreEqual(WallType.Door, wall.Type);
            Assert.Contains(wall, blxBlock.Walls);
            Assert.AreSame(blxBlock.Segments[7].GetSide(SegSide.Left).Wall, wall.OppositeWall);

            // Check some basic wall properties
            Assert.AreEqual(0, wall.HitPoints);
            Assert.AreEqual(WallFlags.DoorLocked | WallFlags.DoorAuto, wall.Flags);
            Assert.AreEqual((WallState)0, wall.State);
            Assert.AreEqual(18, wall.DoorClipNumber);
            Assert.AreEqual(WallKeyFlags.None, wall.Keys);
            Assert.AreEqual(0, wall.CloakOpacity);
        }

        [Test]
        public void TestExtendedBlockTriggers()
        {
            Assert.Fail();
        }

        [Test]
        public void TestExtendedBlockAnimatedLights()
        {
            Assert.Fail();
        }

        [Test]
        public void TestExtendedBlockMatcens()
        {
            Assert.Fail();
        }

        [Test]
        public void TestExtendedBlockSpecial()
        {
            Assert.Fail();
        }

        [Test]
        public void TestExtendedBlockWrite()
        {
            var stream = new MemoryStream();
            Assert.DoesNotThrow(() => blxBlock.WriteToStream(stream));

            var originalFileContents = GetArrayFromResourceStream("test.blx");
            Assert.IsTrue(Enumerable.SequenceEqual(originalFileContents, stream.ToArray()));
        }
    }
}
