using LibDescent.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PiggyDump
{
    public partial class ElementList : Form
    {
        public int ElementNumber { get { return ElementListBox.SelectedIndex; } }
        public ElementList(HAMFile datafile, HAMType type)
        {
            InitializeComponent();
            switch (type)
            {
                case HAMType.VClip:
                    foreach (String name in datafile.VClipNames)
                    {
                        ElementListBox.Items.Add(name);
                    }
                    break;
                case HAMType.EClip:
                    foreach (String name in datafile.EClipNames)
                    {
                        ElementListBox.Items.Add(name);
                    }
                    break;
                case HAMType.Robot:
                    foreach (String name in datafile.RobotNames)
                    {
                        ElementListBox.Items.Add(name);
                    }
                    break;
                case HAMType.Weapon:
                    foreach (String name in datafile.WeaponNames)
                    {
                        ElementListBox.Items.Add(name);
                    }
                    break;
                case HAMType.Model:
                    foreach (String name in datafile.ModelNames)
                    {
                        ElementListBox.Items.Add(name);
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
