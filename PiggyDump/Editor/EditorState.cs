using LibDescent.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Descent2Workshop.Editor
{
    [Flags]
    public enum UpdateFlags
    {
        /// <summary>
        /// No updates pending.
        /// </summary>
        None = 0,
        /// <summary>
        /// World has changed.
        /// </summary>
        World = 1,
        /// <summary>
        /// Selected elements have changed.
        /// </summary>
        Selected = 2,
        /// <summary>
        /// Update shadow has changed.
        /// </summary>
        Shadow = 4,
    }

    public class EditorState : IInputEventHandler
    {
        private Level level;
        private HAMFile dataFile;
        //private SharedRendererState rendererState;
        private Render.MineRender rendererState;
        private EditorUI host;
        private List<LevelVertex> selectedVertices = new List<LevelVertex>();
        private Dictionary<LevelVertex, int> selectedVertMapping = new Dictionary<LevelVertex, int>();
        private float gridSize = 1;
        private Tool currentTool;

        public ShadowProperties shadow = new ShadowProperties();

        //This could be done better
        //Update everything first time around
        /// <summary>
        /// Flags of all objects to update at the beginning of a frame. 
        /// </summary>
        public UpdateFlags updateFlags = (UpdateFlags)0x7fffffff;

        private Render.Camera workingCamera;
        private int viewportX, viewportY;

        public List<LevelVertex> SelectedVertices { get { return selectedVertices; } }
        public float GridSize { get { return gridSize; } set { gridSize = value; } }
        public Dictionary<LevelVertex, int> SelectedVertMapping { get => selectedVertMapping; set => selectedVertMapping = value; }
        public Level EditorLevel { get { return level; } }
        public HAMFile EditorData { get { return dataFile; } }

        public EditorState(Level level, HAMFile dataFile, EditorUI host)
        {
            this.level = level;
            this.dataFile = dataFile;
            this.host = host;
            //rendererState.state = this;
            //this.rendererState = rendererState;
        }

        /// <summary>
        /// This function is a bit of a hack. Tools need to know the properties of the viewport they're in, 
        /// so this is called before handling events to ensure that the state knows the current viewport.
        /// </summary>
        /// <param name="camera">The camera of the viewport.</param>
        public void SetViewportProperties(Render.Camera camera, int w, int h)
        {
            workingCamera = camera;
            viewportX = w; viewportY = h;
        }

        public void AttachRenderer(Render.MineRender renderer)
        {
            rendererState = renderer;
        }

        public void StatusMessage(string msg)
        {
            host.StatusMessage(msg);
        }

        public bool HandleEvent(InputEvent ev)
        {
            if (currentTool != null)
            {
                bool ret = currentTool.HandleEvent(ev);
                if (ret) return true;
            }
            if (ev.type == EventType.Key)
            {
                if (ev.key == System.Windows.Forms.Keys.G)
                {
                    if (selectedVertices.Count == 0)
                    {
                        StatusMessage("No elements selected.");
                    }
                    else
                    {
                        Vector3 upVector, sideVector;
                        workingCamera.GetUpSide(out upVector, out sideVector);
                        Vector4 hack = workingCamera.TransformPoint(new Vector3(SelectedVertices[0].location.x, SelectedVertices[0].location.y, SelectedVertices[0].location.z));
                        Console.WriteLine("z {0} w {1}", hack.Z, hack.W);
                        Tools.GrabTool grabTool = new Tools.GrabTool(this, sideVector, upVector, selectedVertices);
                        grabTool.SetGrabScale(hack.W * 2 / Math.Min(viewportY, viewportX));
                        currentTool = grabTool;
                    }
                    return true;
                }
            }
            return false;
        }

        public void ApplyTool()
        {
            if (currentTool != null)
            {
                Operation op = currentTool.GenerateOperation();
                op.Apply(this);
                currentTool.EndTool();
                currentTool = null;
            }
        }

        public void AbortTool()
        {
            if (currentTool != null)
            {
                currentTool.EndTool();
                currentTool = null;
            }
        }

        public void ToggleSelectedVert(LevelVertex vert)
        {
            int index;
            if (!vert.selected)
            {
                vert.selected = true;
                index = selectedVertices.Count;
                selectedVertMapping.Add(vert, index);
                selectedVertices.Add(vert);
            }
            else
            {
                index = selectedVertices.Count - 1;
                vert.selected = false;
                LevelVertex lastVert = selectedVertices[index];
                int deleteIndex = selectedVertMapping[vert];
                selectedVertMapping[lastVert] = deleteIndex;
                selectedVertMapping.Remove(vert);
                selectedVertices[deleteIndex] = lastVert;
                selectedVertices.RemoveAt(index);
                index = deleteIndex;
            }

            updateFlags |= UpdateFlags.Selected;
            host.InvalidateAll();
        }
    }
}