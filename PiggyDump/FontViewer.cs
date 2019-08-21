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
    public partial class FontViewer : Form
    {
        Font mainFont;
        public FontViewer(Font font)
        {
            InitializeComponent();
            mainFont = font;
            numericUpDown1.Minimum = 0; numericUpDown1.Maximum = font.lastChar - font.firstChar + 1;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Image temp = pictureBox1.Image;
                temp.Dispose();
            }
            pictureBox1.Image = mainFont.GetCharacterBitmap((int)numericUpDown1.Value);
            pictureBox1.Refresh();
        }
    }
}
