using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiggyDump.Editor
{
    public class EditorState : IInputEventHandler
    {
        private Level level;
        private HAMFile dataFile;
        private SharedRendererState rendererState;
        private List<LevelVertex> selectedVertices = new List<LevelVertex>();
        private Dictionary<LevelVertex, int> selectedVertMapping = new Dictionary<LevelVertex, int>();
        private float gridSize = 1;

        public List<LevelVertex> SelectedVertices { get { return selectedVertices; } }
        public float GridSize { get { return gridSize; } set { gridSize = value; } }
        public Dictionary<LevelVertex, int> SelectedVertMapping { get => selectedVertMapping; set => selectedVertMapping = value; }

        public EditorState(Level level, HAMFile dataFile, SharedRendererState rendererState)
        {
            this.level = level;
            this.dataFile = dataFile;
            this.rendererState = rendererState;
            rendererState.state = this;
        }

        public bool HandleEvent(InputEvent ev)
        {
            return false;
        }

        public void ToggleSelectedVert(LevelVertex vert)
        {
            if (!vert.selected)
            {
                vert.selected = true;
                selectedVertMapping.Add(vert, selectedVertices.Count);
                selectedVertices.Add(vert);
            }
            else
            {
                vert.selected = false;
                LevelVertex lastVert = selectedVertices[selectedVertices.Count - 1];
                int deleteIndex = selectedVertMapping[vert];
                selectedVertMapping[lastVert] = deleteIndex;
                selectedVertMapping.Remove(vert);
                selectedVertices[deleteIndex] = lastVert;
                selectedVertices.RemoveAt(selectedVertices.Count - 1);
            }
            rendererState.ToggleSelectedVert(vert);
        }
    }
}
