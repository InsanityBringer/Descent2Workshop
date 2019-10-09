/*
    Copyright (c) 2019 SaladBadger

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LibDescent.Data;

namespace Descent2Workshop.Editor
{
    public class ConvertToOverload
    {
        public const string DefaultGlobals = "  \"global_data\": {\r\n    \"grid_size\": 6,\r\n    \"pre_smooth\": 4,\r\n    \"post_smooth\": 1,\r\n    \"simplify_strength\": 0.0,\r\n    \"deform_presets0\": \"PLAIN_NOISE\",\r\n    \"deform_presets1\": \"NONE\",\r\n    \"deform_presets2\": \"NONE\",\r\n    \"deform_presets3\": \"NONE\"\r\n  },";
        public const string DefaultCustom = "  \"custom_level_info\": {\r\n    \"exit_music_start_time\": 0.0,\r\n    \"exit_no_explosions\": false,\r\n    \"alien_lava\": false,\r\n    \"custom_count\": 0,\r\n    \"objective\": \"NORMAL\"\r\n  },";
        public const string DefaultDecals = "          \"decals\": [\r\n            {\r\n              \"mesh_name\": \"\",\r\n              \"align\": \"CENTER\",\r\n              \"mirror\": \"OFF\",\r\n              \"rotation\": 0,\r\n              \"repeat_u\": 1,\r\n              \"repeat_v\": 1,\r\n              \"offset_u\": 0,\r\n              \"offset_v\": 0,\r\n              \"hidden\": false,\r\n              \"clips\": [\r\n                \"NONE\",\r\n                \"NONE\",\r\n                \"NONE\",\r\n                \"NONE\"\r\n              ],\r\n              \"caps\": [\r\n                \"NONE\",\r\n                \"NONE\",\r\n                \"NONE\",\r\n                \"NONE\"\r\n              ]\r\n            },\r\n            {\r\n              \"mesh_name\": \"\",\r\n              \"align\": \"CENTER\",\r\n              \"mirror\": \"OFF\",\r\n              \"rotation\": 0,\r\n              \"repeat_u\": 1,\r\n              \"repeat_v\": 1,\r\n              \"offset_u\": 0,\r\n              \"offset_v\": 0,\r\n              \"hidden\": false,\r\n              \"clips\": [\r\n                \"NONE\",\r\n                \"NONE\",\r\n                \"NONE\",\r\n                \"NONE\"\r\n              ],\r\n              \"caps\": [\r\n                \"NONE\",\r\n                \"NONE\",\r\n                \"NONE\",\r\n                \"NONE\"\r\n              ]\r\n            }\r\n          ],";
        public const string DefaultRot = "      \"rotation\": [\r\n        1.0,\r\n        0.0,\r\n        0.0,\r\n        0.0,\r\n        0.0,\r\n        0.0,\r\n        1.0,\r\n        0.0,\r\n        0.0,\r\n        -1.0,\r\n        0.0,\r\n        0.0,\r\n        0.0,\r\n        0.0,\r\n        0.0,\r\n        1.0\r\n      ],";
        public const string DefaultObj = "      \"properties\": {\r\n        \"special_props\": \"NONE\",\r\n        \"matcen_spawn_type_1\": \"RECOILA\",\r\n        \"matcen_spawn_probability_1\": \"1\",\r\n        \"matcen_spawn_type_2\": \"RECOILA\",\r\n        \"matcen_spawn_probability_2\": \"0\",\r\n        \"m_max_alive\": \"3\",\r\n        \"m_spawn_wait\": \"MEDIUM\",\r\n        \"ed_invulnerable\": \"False\"";
        public static void WriteOverloadLevel(string filename, Level level)
        {
            int numSegs = level.Segments.Count;
            int numVerts = level.Verts.Count;
            int numEnts = 1; //don't export ents atm

            StreamWriter sw = new StreamWriter(File.Open(filename, FileMode.Create));
            sw.WriteLine("{");
            sw.WriteLine("  \"properties\": {");
            sw.WriteLine("    \"next_segment\": {0},", numSegs);
            sw.WriteLine("    \"next_vertex\": {0},", numVerts);
            sw.WriteLine("    \"next_entity\": {0},", numEnts);
            sw.WriteLine("    \"selected_segment\": {0},", numSegs-1);
            sw.WriteLine("    \"selected_side\": {0},", 0);
            sw.WriteLine("    \"selected_vertex\": {0},", numVerts-1);
            sw.WriteLine("    \"selected_entity\": {0},", numEnts-1);
            sw.WriteLine("    \"num_segments\": {0},", numSegs);
            sw.WriteLine("    \"num_vertices\": {0},", numVerts);
            sw.WriteLine("    \"num_entities\": {0},", numEnts);
            sw.WriteLine("    \"num_marked_segments\": {0},", 0);
            sw.WriteLine("    \"num_marked_sides\": {0},", 0);
            sw.WriteLine("    \"num_marked_vertices\": {0},", 0);
            sw.WriteLine("    \"num_marked_entities\": {0},", 0);
            sw.WriteLine("    \"texture_set\": \"{0}\"", "Titan - Bronze");
            sw.WriteLine("  }, ");
            sw.WriteLine(DefaultGlobals);
            sw.WriteLine(DefaultCustom);
            sw.WriteLine("  \"verts\": {");
            FixVector vert;
            for (int i = 0; i < numVerts; i++)
            {
                vert = level.Verts[i].location;
                sw.WriteLine("    \"{0}\": {{", i);
                sw.WriteLine("      \"marked\": false, ");
                sw.WriteLine("      \"x\": {0}, ", (float)(vert.x / 65536.0 / 5.0));
                sw.WriteLine("      \"y\": {0}, ", (float)(vert.y / 65536.0 / 5.0));
                sw.WriteLine("      \"z\": {0} ", (float)(vert.z / 65536.0 / 5.0));
                sw.Write("    }");
                if (i < (numVerts-1))
                    sw.WriteLine(",");
                else
                    sw.WriteLine();
            }
            sw.WriteLine("  },");
            sw.WriteLine("  \"segments\": {");
            Segment seg;
            for (int i = 0; i < numSegs; i++)
            {
                seg = level.Segments[i];
                sw.WriteLine("    \"{0}\": {{", i);
                sw.WriteLine("      \"marked\": false, ");
                sw.WriteLine("      \"pathfinding\": \"All\", ");
                sw.WriteLine("      \"exitsegement\": \"None\", ");
                sw.WriteLine("      \"dark\": false, ");
                sw.WriteLine("      \"verts\": [");
                for (int v = 0; v < Segment.MaxSegmentVerts; v++)
                {
                    sw.Write("        {0}", seg.verts[v]);
                    if (v < Segment.MaxSegmentVerts - 1)
                        sw.WriteLine(",");
                    else
                        sw.WriteLine();
                }
                sw.WriteLine("      ],");
                sw.WriteLine("      \"sides\": [");
                Side side;
                for (int s = 0; s < Segment.MaxSegmentSides; s++)
                {
                    side = seg.sides[s];
                    sw.WriteLine("        {");
                    sw.WriteLine("          \"marked\": false, ");
                    sw.WriteLine("          \"chunk_plane_order\": -1, ");
                    sw.WriteLine("          \"tex_name\": \"{0}\",", GetTexture(side.tmapNum, side.tmapNum2 & 0x3FFF));
                    sw.WriteLine("          \"deformation_preset\": 0, ");
                    sw.WriteLine("          \"deformation_height\": 0.0, ");
                    sw.WriteLine("          \"verts\": [");
                    for (int v = 0; v < 4; v++)
                    {
                        sw.Write("            {0}", seg.verts[Segment.SideVerts[s, v]]);
                        if (v < 3)
                            sw.WriteLine(",");
                        else
                            sw.WriteLine();
                    }
                    sw.WriteLine("          ],");
                    sw.WriteLine("          \"uvs\": [");
                    for (int v = 0; v < 4; v++)
                    {
                        sw.WriteLine("            {");
                        sw.WriteLine("              \"u\": {0}, ", (float)(side.uvls[v].x / 65536.0));
                        sw.WriteLine("              \"v\": {0} ", (float)(side.uvls[v].y / 65536.0));
                        if (v < 3)
                            sw.WriteLine("            },");
                        else
                            sw.WriteLine("            }");
                    }
                    sw.WriteLine("          ],");
                    sw.WriteLine(DefaultDecals);
                    sw.WriteLine("          \"door\": -1 ");
                    sw.Write("        }");
                    if (s < Segment.MaxSegmentSides - 1)
                        sw.WriteLine(",");
                    else
                        sw.WriteLine();

                }
                sw.WriteLine("      ],");
                sw.WriteLine("      \"neighbors\": [");
                for (int s = 0; s < Segment.MaxSegmentSides; s++)
                {
                    sw.Write("          {0}", (float)seg.childrenIDs[s]);
                    if (s < Segment.MaxSegmentSides - 1)
                        sw.WriteLine(",");
                    else
                        sw.WriteLine();
                }
                sw.WriteLine("      ]");
                sw.Write("    }");
                if (i < numSegs - 1)
                    sw.WriteLine(",");
                else
                    sw.WriteLine();
            }
            sw.WriteLine("  },");
            sw.WriteLine("  \"entities\": {");
            sw.WriteLine("    \"0\": {");
            sw.WriteLine("      \"guid\": \"{0}\",", Guid.NewGuid());
            sw.WriteLine("      \"ref_guid\": \"00000000-0000-0000-0000-000000000000\",");
            sw.WriteLine("      \"position\": [");
            sw.WriteLine("        {0},", level.Objects[0].position.x / 65536.0f / 5.0f);
            sw.WriteLine("        {0},", level.Objects[0].position.y / 65536.0f / 5.0f);
            sw.WriteLine("        {0}", level.Objects[0].position.z / 65536.0f / 5.0f);
            sw.WriteLine("      ],");
            sw.WriteLine(DefaultRot);
            sw.WriteLine("      \"segnum\": \"{0}\",", level.Objects[0].segnum);
            sw.WriteLine("      \"type\": \"SPECIAL\",");
            sw.WriteLine("      \"sub_type\": \"PLAYER_START\",");
            sw.WriteLine("      \"mp_team\": \"0\",");
            sw.WriteLine(DefaultObj);
            sw.WriteLine("      }");
            sw.WriteLine("    }");
            sw.WriteLine("  }");
            sw.WriteLine("}");

            sw.Flush();
            sw.Close();
            sw.Dispose();
        }

        private static string GetTexture(int id1, int id2)
        {
            if (id1 == 378 || id1 == 404 || id1 == 409)
                return "Lava_v2";
            else if (id1 == 405 || id1 == 406 || id1 == 407 || id1 == 408)
                return "Lava_flowing";
            if (id1 >= 275) //ceils
                return "ind_metal_ceiling_01i";
            else if (id1 > 200 || id2 > 200)
            {
                return "ind_metal_wall_01e";
            }
            else
            {
                return "rockwall_01a";
            }
        }

    }
}
