using LibDescent.Data;
using LibDescent.Edit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Descent2Workshop
{
    public partial class ElementList : Form
    {
        public int ElementNumber { get { return ElementListBox.SelectedIndex; } }
        public ElementList(EditorHAMFile datafile, HAMType type)
        {
            InitializeComponent();
            switch (type)
            {
                case HAMType.VClip:
                    foreach (VClip vclip in datafile.VClips)
                    {
                        ElementListBox.Items.Add(vclip.Name);
                    }
                    break;
                case HAMType.EClip:
                    foreach (EClip clip in datafile.EClips)
                    {
                        ElementListBox.Items.Add(clip.Name);
                    }
                    break;
                case HAMType.Robot:
                    foreach (Robot robot in datafile.Robots)
                    {
                        ElementListBox.Items.Add(robot.Name);
                    }
                    break;
                case HAMType.Weapon:
                    foreach (Weapon weapon in datafile.Weapons)
                    {
                        ElementListBox.Items.Add(weapon.Name);
                    }
                    break;
                case HAMType.Model:
                    foreach (Polymodel model in datafile.Models)
                    {
                        ElementListBox.Items.Add(model.Name);
                    }
                    break;
                case HAMType.Sound:
                    foreach (String name in datafile.SoundNames)
                    {
                        ElementListBox.Items.Add(name);
                    }
                    break;
            }
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ElementListBox_DoubleClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
