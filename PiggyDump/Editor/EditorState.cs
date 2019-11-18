using LibDescent.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Descent2Workshop.Editor
{
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

        public void AttachRenderer(Render.MineRender renderer)
        {
            rendererState = renderer;
        }

        public bool HandleEvent(InputEvent ev)
        {
            return false;
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
                rendererState.AddSelectedVert(vert);
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
                rendererState.RemoveSelectedVertAt(deleteIndex);
            }
            //rendererState.SetSelectedVert(vert, index);
            
            host.InvalidateAll();
        }
    }
}