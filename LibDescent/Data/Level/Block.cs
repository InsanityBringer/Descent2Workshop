using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LibDescent.Data
{
    public interface IBlock
    {
        List<Segment> Segments { get; }
        List<Wall> Walls { get; }
        //List<Trigger> Triggers { get; }
        //List<FlickeringLight> AnimatedLights { get; }

        uint GetVertexCount();
        void WriteToStream(Stream stream);
    }

    public class Block : IBlock
    {
        public List<Segment> Segments { get; } = new List<Segment>();

        // Walls is always empty for a regular block
        public List<Wall> Walls => new List<Wall>();

        public uint GetVertexCount()
        {
            return BlockCommon.GetVertexCount(this);
        }

        public static Block CreateFromStream(Stream stream)
        {
            var reader = new BlockStreamReader(stream);
            if (reader.ReadLine() != "DMB_BLOCK_FILE")
            {
                throw new InvalidDataException("DMB block header is missing or incorrect.");
            }

            var segments = new Dictionary<uint, Segment>();
            // We read segment children before we have all the segments, so we need to connect them in a subsequent pass
            var segmentConnections = new Dictionary<uint, int[]>();

            while (!reader.EndOfStream)
            {
                uint segmentId = BlockCommon.ReadValue<uint>(reader, "segment");
                if (segments.ContainsKey(segmentId))
                {
                    throw new InvalidDataException($"Encountered duplicate definition of segment {segmentId} at line {reader.LastLineNumber}.");
                }
                var segment = new Segment(Segment.MaxSegmentSides, Segment.MaxSegmentVerts);
                segments[segmentId] = segment;

                // Read sides
                for (uint sideNum = 0; sideNum < segment.Sides.Length; sideNum++)
                {
                    int fileSideNum = BlockCommon.ReadValue<int>(reader, "side");
                    if (fileSideNum != sideNum)
                    {
                        throw new InvalidDataException($"Unexpected side number {fileSideNum} at line {reader.LastLineNumber}.");
                    }

                    var side = new Side(segment, sideNum);
                    segment.Sides[sideNum] = side;
                    side.BaseTexture = LevelTexture.FromTextureIndex(BlockCommon.ReadPrimaryTextureIndex(reader, false));
                    (var overlayIndex, var overlayRotation) = BlockCommon.ReadSecondaryTexture(reader, false);
                    side.OverlayTexture = LevelTexture.FromTextureIndex(overlayIndex);
                    side.OverlayRotation = overlayRotation;
                    for (int i = 0; i < side.Uvls.Length; i++)
                    {
                        side.Uvls[i] = BlockCommon.ReadUvl(reader);
                    }
                }

                segmentConnections[segmentId] = BlockCommon.ReadSegmentChildren(reader);

                // Read vertices
                for (uint vertexNum = 0; vertexNum < Segment.MaxSegmentVerts; vertexNum++)
                {
                    uint fileVertexNum;
                    var vertexLocation = BlockCommon.ReadVertex(reader, out fileVertexNum, false);
                    if (fileVertexNum != vertexNum)
                    {
                        throw new InvalidDataException($"Unexpected vertex number {fileVertexNum} at line {reader.LastLineNumber}.");
                    }

                    segment.Vertices[vertexNum] = new LevelVertex(vertexLocation);
                }

                segment.Light = Fix.FromRawValue(BlockCommon.ReadValue<int>(reader, "static_light"));
                segment.Function = SegFunction.None;
                segment.MatCenter = null;
                segment.value = 0xFF; // related to matcens, might be able to get rid of this
            }

            // Now set up segment connections
            foreach (var connection in segmentConnections)
            {
                for (int i = 0; i < connection.Value.Length; i++)
                {
                    var connectedSegmentId = connection.Value[i];
                    if (connectedSegmentId < 0)
                    {
                        continue;
                    }
                    segments[connection.Key].Sides[i].ConnectedSegment = segments[(uint)connectedSegmentId];
                }
            }

            Block block = new Block();
            foreach (Segment segment in segments.Values)
            {
                block.Segments.Add(segment);
            }
            BlockCommon.SetupVertexConnections(block);
            block.RemoveDuplicateVertices();

            return block;
        }

        private void RemoveDuplicateVertices()
        {
            var processedSides = new SortedSet<(int segmentNum, int sideNum)>();

            for (int segmentNum = 0; segmentNum < Segments.Count; segmentNum++)
            {
                var segment = Segments[segmentNum];

                for (int sideNum = 0; sideNum < Segments[segmentNum].Sides.Length; sideNum++)
                {
                    var side = segment.Sides[sideNum];

                    if (side.ConnectedSegment == null)
                    {
                        // Nothing to do
                        continue;
                    }

                    if (processedSides.Contains((segmentNum, sideNum)))
                    {
                        // Already handled this side
                        continue;
                    }

                    // Find the connected side. GetJoinedSide won't work yet so we have to do it the long way
                    var otherSegment = side.ConnectedSegment;
                    var otherSegmentNum = Segments.IndexOf(otherSegment);
                    Side otherSide = null;
                    int otherSideNum;
                    for (otherSideNum = 0; otherSideNum < otherSegment.Sides.Length; otherSideNum++)
                    {
                        otherSide = otherSegment.Sides[otherSideNum];
                        if (side.IsJoinedTo(otherSide, checkVertices: false))
                        {
                            break;
                        }
                    }
                    if (otherSideNum >= otherSegment.Sides.Length)
                    {
                        // This means the sides are only connected in one direction. That could be a problem
                        continue;
                    }

                    // Now match up the vertices
                    // Find the vertex of the other side that matches the first vertex of this side
                    int? matchingVertexNum = null;
                    for (int vertexNum = 0; vertexNum < otherSide.GetNumVertices(); vertexNum++)
                    {
                        if (side.GetVertex(0).Location == otherSide.GetVertex(vertexNum).Location)
                        {
                            matchingVertexNum = vertexNum;
                            break;
                        }
                    }

                    if (!matchingVertexNum.HasValue)
                    {
                        // We could try to find another match... but the level geometry is broken
                        // anyway, so don't worry about it too much
                        continue;
                    }

                    // Walk through vertices in opposite directions - vertex n+1 of this side is joined
                    // to vertex n-1 of the other side
                    for (int vertexNum = 0; vertexNum < side.GetNumVertices(); vertexNum++)
                    {
                        int otherVertexNum = matchingVertexNum.Value - vertexNum;
                        if (otherVertexNum < 0) { otherVertexNum += otherSide.GetNumVertices(); }
                        var vertex = side.GetVertex(vertexNum);
                        var otherVertex = otherSide.GetVertex(otherVertexNum);
                        if (vertex.Location == otherVertex.Location)
                        {
                            MergeVertices(vertex, otherVertex);
                        }
                    }

                    // Add the other side to the processed set (this one won't be seen again anyway)
                    processedSides.Add((otherSegmentNum, otherSideNum));
                }
            }
        }

        private void MergeVertices(LevelVertex vertex, LevelVertex otherVertex)
        {
            vertex.ConnectedSegments.AddRange(otherVertex.ConnectedSegments);
            vertex.ConnectedSides.AddRange(otherVertex.ConnectedSides);

            // Replace all references to otherVertex with vertex
            // Sides don't directly hold references so we just handle the segments
            foreach ((var segment, var vertexNum) in otherVertex.ConnectedSegments)
            {
                segment.Vertices[vertexNum] = vertex;
            }
        }

        public void WriteToStream(Stream stream)
        {
            throw new NotImplementedException();
        }
    }

    public class ExtendedBlock : IBlock
    {
        private const int BLX_SIDE_VERTEX_ID_NONE = 0xFF;
        private const int BLX_WALL_ID_NONE = 2047;
        private const int BLX_MAX_VERTEX_NUM = 0xFFF7;

        public List<Segment> Segments { get; } = new List<Segment>();
        public List<Wall> Walls { get; } = new List<Wall>();

        public uint GetVertexCount()
        {
            return BlockCommon.GetVertexCount(this);
        }

        public static ExtendedBlock CreateFromStream(Stream stream)
        {
            var reader = new BlockStreamReader(stream);
            if (reader.ReadLine() != "DMB_EXT_BLOCK_FILE")
            {
                throw new InvalidDataException("DLE block header is missing or incorrect.");
            }

            ExtendedBlock block = new ExtendedBlock();
            var segments = new Dictionary<uint, Segment>();
            // We read segment children before we have all the segments, so we need to connect them in a subsequent pass
            var segmentConnections = new Dictionary<uint, int[]>();
            var vertices = new Dictionary<uint, LevelVertex>();

            while (!reader.EndOfStream)
            {
                uint segmentId = BlockCommon.ReadValue<uint>(reader, "segment");
                if (segments.ContainsKey(segmentId))
                {
                    throw new InvalidDataException($"Encountered duplicate definition of segment {segmentId} at line {reader.LastLineNumber}.");
                }
                var segment = new Segment(Segment.MaxSegmentSides, Segment.MaxSegmentVerts);
                segments[segmentId] = segment;

                // Read sides
                for (uint sideNum = 0; sideNum < segment.Sides.Length; sideNum++)
                {
                    int fileSideNum = BlockCommon.ReadValue<int>(reader, "side");

                    // Negative side numbers in .blx format indicate animated lights - weird, but that's how it is
                    bool hasVariableLight = fileSideNum < 0;
                    if (hasVariableLight) { fileSideNum = -fileSideNum; }
                    if (fileSideNum != sideNum)
                    {
                        throw new InvalidDataException($"Unexpected side number {fileSideNum} at line {reader.LastLineNumber}.");
                    }

                    var side = new Side(segment, sideNum);
                    segment.Sides[sideNum] = side;

                    // Textures
                    side.BaseTexture = LevelTexture.FromTextureIndex(BlockCommon.ReadPrimaryTextureIndex(reader, true));
                    (var overlayIndex, var overlayRotation) = BlockCommon.ReadSecondaryTexture(reader, true);
                    side.OverlayTexture = LevelTexture.FromTextureIndex(overlayIndex);
                    side.OverlayRotation = overlayRotation;
                    for (int i = 0; i < side.Uvls.Length; i++)
                    {
                        side.Uvls[i] = BlockCommon.ReadUvl(reader);
                    }

                    // Vertex mapping
                    uint[] sideVertexIds = BlockCommon.ReadExtendedSideVertexIds(reader);
                    foreach (uint sideVertexId in sideVertexIds)
                    {
                        // BLX_SIDE_VERTEX_ID_NONE (0xFF) = no vertex
                        // D2X-XL uses these for non-cuboid segment support. We don't have that yet - more design work needed
                        if (sideVertexId == BLX_SIDE_VERTEX_ID_NONE)
                        {
                            throw new NotSupportedException($"Found non-quadrilateral side {sideNum} at line {reader.LastLineNumber}, "
                                + "which is currently unsupported.");
                        }

                        // Except for non-cuboid segments, side vertex ids appear to match Segment.SideVerts anyway, so we can ignore them
                    }
                    // This is what DLE does here - for future reference
                    /*for (int j = 0; j < 4; j++)
                        pSide->m_vertexIdIndex[j] = ubyte(sideVertexIds[j]);
                    pSide->DetectShape();*/

                    // Animated lights
                    if (hasVariableLight)
                    {
                        Tuple<uint, int, int> variableLight = BlockCommon.ReadExtendedVariableLight(reader);
                        // Not implemented yet. Below is DLE code
                        /*tVariableLight vl;
                        lightManager.AddVariableLight(CSideKey(nSegment, nSide), vl.mask, vl.timer);*/
                    }

                    // Walls and triggers
                    uint wallId = BlockCommon.ReadValue<uint>(reader, "wall");
                    if (wallId != BLX_WALL_ID_NONE)
                    {
                        var wall = new Wall();
                        block.Walls.Add(wall);
                        wall.segnum = BlockCommon.ReadValue<int>(reader, "segment");
                        wall.sidenum = BlockCommon.ReadValue<int>(reader, "side");
                        wall.hp = BlockCommon.ReadValue<int>(reader, "hps");
                        wall.type = (WallType)BlockCommon.ReadValue<int>(reader, "type");
                        wall.flags = BlockCommon.ReadValue<byte>(reader, "flags");
                        wall.state = BlockCommon.ReadValue<byte>(reader, "state");
                        wall.clipNum = (byte)BlockCommon.ReadValue<sbyte>(reader, "clip");
                        wall.keys = BlockCommon.ReadValue<byte>(reader, "keys");
                        wall.cloakValue = BlockCommon.ReadValue<byte>(reader, "cloak");
                        wall.trigger = BlockCommon.ReadValue<byte>(reader, "trigger");

                        // 255 (0xFF) means no trigger on this wall
                        if (wall.trigger != 0xFF)
                        {
                            // Still need to port this
                            //var trigger = new Trigger();
                            //block.Triggers.Add(trigger);
                            BlockCommon.ReadValue<byte>(reader, "type");
                            BlockCommon.ReadValue<ushort>(reader, "flags");
                            BlockCommon.ReadValue<int>(reader, "value");
                            BlockCommon.ReadValue<int>(reader, "timer");
                            var targetCount = BlockCommon.ReadValue<short>(reader, "count");
                            for (int targetNum = 0; targetNum < targetCount; targetNum++)
                            {
                                BlockCommon.ReadValue<short>(reader, "segment");
                                BlockCommon.ReadValue<short>(reader, "side");
                            }
                        }

                        // Link wall to trigger, and link trigger to targets
                        // (not implemented yet)
                    }
                }

                segmentConnections[segmentId] = BlockCommon.ReadSegmentChildren(reader);

                // Read vertices
                /*pSegment->m_nShape = 0;*/ // DLE non-cuboid - for reference
                for (uint vertexNum = 0; vertexNum < Segment.MaxSegmentVerts; vertexNum++)
                {
                    uint fileVertexNum;
                    var vertexLocation = BlockCommon.ReadVertex(reader, out fileVertexNum, true);

                    // Non-cuboid segment support
                    /*pSegment->m_info.vertexIds[i] = fileVertexNum;*/
                    if (fileVertexNum > BLX_MAX_VERTEX_NUM)
                    {
                        throw new NotSupportedException($"Found non-cuboid segment {segmentId} at line {reader.LastLineNumber}, "
                            + "which is currently unsupported.");
                        /*pSegment->m_nShape++;
                        continue;*/
                    }

                    if (!vertices.ContainsKey(fileVertexNum))
                    {
                        vertices[fileVertexNum] = new LevelVertex(vertexLocation);
                    }
                    segment.Vertices[vertexNum] = vertices[fileVertexNum];
                }

                segment.Light = Fix.FromRawValue(BlockCommon.ReadValue<int>(reader, "static_light"));
                segment.special = BlockCommon.ReadValue<byte>(reader, "special");
                var nProducer = BlockCommon.ReadValue<sbyte>(reader, "matcen_num"); // not sure what DLE uses this for yet
                segment.value = (byte)BlockCommon.ReadValue<sbyte>(reader, "value");
                // Child/wall bitmasks are used internally by DLE but we don't really need them
                // - they can be recalculated
                BlockCommon.ReadValue<byte>(reader, "child_bitmask");
                BlockCommon.ReadValue<byte>(reader, "wall_bitmask");

                switch (segment.special)
                {
                    // DLE code. Might only need to adapt robot maker and equipment maker
                    /*case SEGMENT_FUNC_PRODUCER:
                        if (!segmentManager.CreateProducer(nSegment, SEGMENT_FUNC_PRODUCER, false, false))
                            pSegment->m_info.function = 0;
                        break;
                    case SEGMENT_FUNC_REPAIRCEN:
                        if (!segmentManager.CreateProducer(nSegment, SEGMENT_FUNC_REPAIRCEN, false, false))
                            pSegment->m_info.function = 0;
                        break;
                    case SEGMENT_FUNC_ROBOTMAKER:
                        if (!segmentManager.CreateRobotMaker(nSegment, false, false))
                            pSegment->m_info.function = 0;
                        break;
                    case SEGMENT_FUNC_EQUIPMAKER:
                        if (!segmentManager.CreateEquipMaker(nSegment, false, false))
                            pSegment->m_info.function = 0;
                        break;
                    case SEGMENT_FUNC_REACTOR:
                        if (!segmentManager.CreateReactor(nSegment, false, false))
                            pSegment->m_info.function = 0;
                        break;*/
                    default:
                        break;
                }
            }

            // Now set up segment connections
            foreach (var connection in segmentConnections)
            {
                for (int i = 0; i < connection.Value.Length; i++)
                {
                    var connectedSegmentId = connection.Value[i];
                    if (connectedSegmentId < 0)
                    {
                        continue;
                    }
                    segments[connection.Key].Sides[i].ConnectedSegment = segments[(uint)connectedSegmentId];
                }
            }

            foreach (Segment segment in segments.Values)
            {
                block.Segments.Add(segment);
            }

            BlockCommon.SetupVertexConnections(block);

            return block;
        }

        public void WriteToStream(Stream stream)
        {
            throw new NotImplementedException();
        }
    }

    internal class BlockStreamReader
    {
        private StreamReader reader;

        public BlockStreamReader(Stream stream)
        {
            reader = new StreamReader(stream);
            LastLineNumber = 0; // Starts at 1 once something is read
        }

        public bool EndOfStream => reader.EndOfStream;

        public int LastLineNumber { get; private set; }
        public string LastLine { get; private set; }

        public string ReadLine()
        {
            LastLine = reader.ReadLine();
            LastLineNumber++;
            return LastLine;
        }
    }

    internal static class BlockCommon
    {
        private static readonly Regex uvlRegex;
        private static readonly Regex vertexRegex;
        private static readonly Regex extendedVertexRegex;

        static BlockCommon()
        {
            uvlRegex = new Regex(@"^    uvls (-?\d+) (-?\d+) (\d+)$", RegexOptions.Compiled);
            vertexRegex = new Regex(@"^  vms_vector (\d+) (-?\d+) (-?\d+) (-?\d+)$", RegexOptions.Compiled);
            extendedVertexRegex = new Regex(@"^  Vertex (\d+) (-?\d+) (-?\d+) (-?\d+)$", RegexOptions.Compiled);
        }

        internal static ushort ReadPrimaryTextureIndex(BlockStreamReader reader, bool isExtendedFormat)
        {
            return ReadValue<ushort>(reader, isExtendedFormat ? "BaseTex" : "tmap_num");
        }

        internal static (ushort, OverlayRotation) ReadSecondaryTexture(BlockStreamReader reader, bool isExtendedFormat)
        {
            var rawValue = ReadValue<short>(reader, isExtendedFormat ? "OvlTex" : "tmap_num2");
            ushort index = (ushort)(rawValue & 0x3FFF);
            OverlayRotation rotation = (OverlayRotation)((rawValue & 0xC000) >> 14);
            return (index, rotation);
        }

        internal static Uvl ReadUvl(BlockStreamReader reader)
        {
            var match = uvlRegex.Match(reader.ReadLine());
            if (!match.Success)
            {
                throw new InvalidDataException($"Expected uvls at line {reader.LastLineNumber}: '{reader.LastLine}'");
            }
            var u = short.Parse(match.Groups[1].Value);
            var v = short.Parse(match.Groups[2].Value);
            var l = ushort.Parse(match.Groups[3].Value);
            return Uvl.FromRawValues(u, v, l);
        }

        internal static int[] ReadSegmentChildren(BlockStreamReader reader)
        {
            var regex = new Regex(@"^  children (-?\d+) (-?\d+) (-?\d+) (-?\d+) (-?\d+) (-?\d+)$");
            var match = regex.Match(reader.ReadLine());
            if (!match.Success)
            {
                throw new InvalidDataException($"Expected children at line {reader.LastLineNumber}: '{reader.LastLine}'");
            }
            object[] groups = new object[match.Groups.Count];
            match.Groups.CopyTo(groups, 0);
            return Array.ConvertAll(groups.Skip(1).ToArray(), group => int.Parse((group as Group).Value));
        }

        internal static FixVector ReadVertex(BlockStreamReader reader, out uint vertexId, bool isExtendedFormat)
        {
            var regex = isExtendedFormat ? extendedVertexRegex : vertexRegex;
            var match = regex.Match(reader.ReadLine());
            if (!match.Success)
            {
                string name = isExtendedFormat ? "Vertex" : "vms_vector";
                throw new InvalidDataException($"Expected {name} at line {reader.LastLineNumber}: '{reader.LastLine}'");
            }

            vertexId = uint.Parse(match.Groups[1].Value);
            return FixVector.FromRawValues(
                x: int.Parse(match.Groups[2].Value),
                y: int.Parse(match.Groups[3].Value),
                z: int.Parse(match.Groups[4].Value));
        }

        internal static uint[] ReadExtendedSideVertexIds(BlockStreamReader reader)
        {
            var regex = new Regex(@"^    vertex ids (\d+) (\d+) (\d+) (\d+)$");
            var match = regex.Match(reader.ReadLine());
            if (!match.Success)
            {
                throw new InvalidDataException($"Expected vertex ids at line {reader.LastLineNumber}: '{reader.LastLine}'");
            }
            object[] groups = new object[match.Groups.Count];
            match.Groups.CopyTo(groups, 0);
            return Array.ConvertAll(groups.Skip(1).ToArray(), group => uint.Parse((group as Group).Value));
        }

        internal static Tuple<uint, int, int> ReadExtendedVariableLight(BlockStreamReader reader)
        {
            var regex = new Regex(@"^    variable light (\d+) (-?\d+) (-?\d+)$");
            var match = regex.Match(reader.ReadLine());
            if (!match.Success)
            {
                throw new InvalidDataException($"Expected variable light at line {reader.LastLineNumber}: '{reader.LastLine}'");
            }
            return new Tuple<uint, int, int>(uint.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value));
        }

        internal static T ReadValue<T>(BlockStreamReader reader, string valueName)
        {
            var regex = new Regex($"^\\s*{valueName} (-?\\d+)$");
            var match = regex.Match(reader.ReadLine());
            if (!match.Success)
            {
                throw new InvalidDataException($"Expected {valueName} at line {reader.LastLineNumber}: '{reader.LastLine}'");
            }
            return (T)System.ComponentModel.TypeDescriptor.GetConverter(typeof(T))
                .ConvertFromString(match.Groups[1].Value);
        }

        internal static void SetupVertexConnections(IBlock block)
        {
            foreach (Segment segment in block.Segments)
            {
                for (uint vertexNum = 0; vertexNum < segment.Vertices.Length; vertexNum++)
                {
                    segment.Vertices[vertexNum].ConnectedSegments.Add((segment, vertexNum));
                }

                foreach (Side side in segment.Sides)
                {
                    for (int vertexNum = 0; vertexNum < side.GetNumVertices(); vertexNum++)
                    {
                        side.GetVertex(vertexNum).ConnectedSides.Add((side, (uint)vertexNum));
                    }
                }
            }
        }

        internal static uint GetVertexCount(IBlock block)
        {
            // Recalculating this on every call is slow, but it should rarely be needed
            // (mostly on block paste to make sure there are enough vertices left in the
            // level). If it becomes a problem we could keep a "secret" vertex list and
            // report the size of that list.

            List<LevelVertex> vertices = new List<LevelVertex>();
            foreach (Segment segment in block.Segments)
            {
                foreach (LevelVertex vertex in segment.Vertices)
                {
                    if (!vertices.Contains(vertex))
                    {
                        vertices.Add(vertex);
                    }
                }
            }

            return (uint)vertices.Count;
        }
    }
}