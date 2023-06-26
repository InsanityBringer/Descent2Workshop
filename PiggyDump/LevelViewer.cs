using LibDescent.Data;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Descent2Workshop
{
    public class LevelViewer : OpenTK.GLControl
    {
        private float ViewPitch, ViewYaw, ViewScale;
        private OpenTK.Vector3 ViewTrans;

        private Point DragStart;
        private bool ControlLoaded;

        private Palette Palette;
        private Dictionary<ushort, PIGImage> TexImg = new Dictionary<ushort, PIGImage>();
        private Dictionary<ushort, int> TexGL = new Dictionary<ushort, int>();
        private int[] GLTextures;

        private ILevel _level;
        public ILevel Level { get => _level; set { _level = value; InitLevel(); } }

        public StandardUI Host { get; set; }

        public LevelViewer()
        {
            MouseWheel += LevelControl_MouseWheel;
            MouseMove += LevelControl_MouseMove;
            MouseDown += LevelControl_MouseDown;
            Reset();
        }

        public void Reset()
        {
            ViewPitch = -0.3f;
            ViewYaw = 0;
            ViewScale = 0.002f;
            ViewTrans = default;
        }

        private void LevelControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                DragStart = e.Location;
        }

        private void LevelControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (ModifierKeys.HasFlag(Keys.Control))
                {
                    ViewTrans.X += (float)(e.X - DragStart.X) / Width;
                    ViewTrans.Y -= (float)(e.Y - DragStart.Y) / Height;
                }
                else
                {
                    ViewYaw -= (float)(e.X - DragStart.X) / Width;
                    ViewPitch -= (float)(e.Y - DragStart.Y) / Height;
                }
                DragStart = e.Location;
                Invalidate();
            }
        }

        private void LevelControl_MouseWheel(object sender, MouseEventArgs e)
        {
            double v = (double)e.Delta * SystemInformation.MouseWheelScrollLines / 120;
            ViewScale *= (float)Math.Pow(1.1, v / 2);
            Invalidate();
        }

        private void LoadTextures()
        {
            if (GLTextures != null)
            {
                GL.DeleteTextures(GLTextures.Length, GLTextures);
                GLTextures = null;
            }

            int[] paletteInt = new int[256];

            for (int i = 0; i < 256; i++)
            {
                int alpha = i == 254 ? 128 : i == 255 ? 0 : 255;
                paletteInt[i] = (alpha << 24) + (Palette[i].B << 16) + (Palette[i].G << 8) + (Palette[i].R);
            }

            var imgs = TexImg.ToArray();
            GLTextures = new int[imgs.Length];
            GL.GenTextures(GLTextures.Length, GLTextures);

            for (int imgIdx = 0; imgIdx < imgs.Length; imgIdx++)
            {
                var img = imgs[imgIdx].Value;
                byte[] data = img.GetData();
                int[] imgRgb = new int[data.Length];
                for (int i = 0; i < data.Length; i++)
                    imgRgb[i] = paletteInt[data[i]];
                GL.BindTexture(TextureTarget.Texture2D, GLTextures[imgIdx]);
                unsafe
                {
                    fixed (int* imgPtr = imgRgb)
                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                            img.Width, img.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte,
                            new IntPtr(imgPtr));
                }
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                TexGL[imgs[imgIdx].Key] = GLTextures[imgIdx];
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Level != null)
                LoadTextures();

            ControlLoaded = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (DesignMode)
            {
                e.Graphics.Clear(System.Drawing.Color.Black);
                return;
            }

            GL.Viewport(0, 0, Width, Height);

            Matrix4 modelMatrix = Matrix4.Identity;
            modelMatrix = Matrix4.CreateTranslation(ViewTrans) * modelMatrix;
            modelMatrix = Matrix4.CreateScale(ViewScale, ViewScale, ViewScale) * modelMatrix;
            modelMatrix = Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(ViewPitch, ViewYaw, 0)) * modelMatrix;

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelMatrix);

            GL.ClearColor(0, 0, 0, 255);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Gequal, 0.02f);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);

            GL.Enable(EnableCap.Texture2D);

            if (Level != null)
                foreach (var seg in Level.Segments)
                    for (int sideNum = 0; sideNum < Segment.MaxSides; sideNum++)
                        if (seg.Sides[sideNum].IsVisible)
                            DrawSide(seg, sideNum);

            SwapBuffers();
        }

        private void DrawSide(Segment seg, int sideNum)
        {
            Side side = seg.Sides[sideNum];

            if (TexGL.TryGetValue(side.BaseTextureIndex, out int glTex))
                GL.BindTexture(TextureTarget.Texture2D, glTex);
            else
                GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Begin(PrimitiveType.Polygon);
            for (int i = 0; i < Side.MaxVertices; i++)
            {
                var vert = side.GetVertex(i);
                ref Uvl uvl = ref side.Uvls[i];
                float l = (float)uvl.L;
                GL.Color3(l, l, l);
                GL.TexCoord2((float)uvl.U, (float)uvl.V);
                GL.Vertex3(vert.X, vert.Y, vert.Z);
            }
            GL.End();

            if (side.OverlayTextureIndex != 0 &&
                TexGL.TryGetValue(side.OverlayTextureIndex, out glTex))
            {
                GL.BindTexture(TextureTarget.Texture2D, glTex);
                GL.Begin(PrimitiveType.Polygon);
                for (int i = 0; i < Side.MaxVertices; i++)
                {
                    var vert = side.GetVertex(i);
                    ref Uvl uvl = ref side.Uvls[i];
                    float l = (float)uvl.L, ou = (float)uvl.U, ov = (float)uvl.V, u, v;
                    switch (side.OverlayRotation)
                    {
                        default: u = ou; v = ov; break;
                        case OverlayRotation.Rotate90: u = 1 - ov; v = ou; break;
                        case OverlayRotation.Rotate180: u = 1 - ou; v = 1 - ov; break;
                        case OverlayRotation.Rotate270: u = ov; v = 1 - ou; break;
                    }
                    GL.Color3(l, l, l);
                    GL.TexCoord2(u, v);
                    GL.Vertex3(vert.X, vert.Y, vert.Z);
                }
                GL.End();
            }
        }

        private void InitLevel()
        {
            Reset();
            TexImg.Clear();
            if (Level == null) return;
            var textures = new HashSet<ushort>();
            foreach (var seg in Level.Segments)
                foreach (var side in seg.Sides)
                {
                    textures.Add(side.BaseTextureIndex);
                    if (side.OverlayTextureIndex != 0)
                        textures.Add(side.OverlayTextureIndex);
                }

            string paletteName = null;
            if (Level is D2Level d2Level)
                paletteName = d2Level.PaletteName.ToLowerInvariant();

            PIGFile pigFile;
            string pigFilename = paletteName != null ?
                Path.Combine(Path.GetDirectoryName(Host.DefaultHogFile.Filename),
                    Path.ChangeExtension(paletteName, "pig")) : null;
            if (pigFilename != null && File.Exists(pigFilename) &&
                Host.DefaultHogFile.ContainsFile(paletteName))
            {
                pigFile = new PIGFile();
                using (var f = File.OpenRead(pigFilename))
                    pigFile.Read(f);
                Palette = new Palette(Host.DefaultHogFile.GetLumpData(paletteName));
            }
            else
            {
                pigFile = Host.DefaultPigFile;
                Palette = Host.DefaultPalette;
            }
            string hamFilename = Path.ChangeExtension(Host.DefaultHogFile.Filename, "ham");
            var ham = new HAMFile();
            using (var f = File.OpenRead(hamFilename))
                ham.Read(f);
            foreach (var texId in textures)
                if (texId < ham.Textures.Count)
                    TexImg[texId] = pigFile.GetImage(ham.Textures[texId]);
            if (ControlLoaded)
                LoadTextures();
        }
    }
}
