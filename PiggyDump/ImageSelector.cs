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

namespace Descent2Workshop
{
    public partial class ImageSelector : Form
    {
        private struct ListEntry
        {
            public string Name;
            public int id;

            public override string ToString()
            {
                return Name;
            }
        }
        private PIGFile pigFile;
        private bool animOnly;

        private int selection = 0;
        public int Selection
        {
            get
            {
                return selection;
            }
        }
        public ImageSelector(PIGFile pigFile, bool animOnly)
        {
            InitializeComponent();
            this.pigFile = pigFile;
            this.animOnly = animOnly;
        }

        private void ImageSelector_Load(object sender, EventArgs e)
        {
            PIGImage img;
            for (int i = 0; i < pigFile.images.Count; i++)
            {
                img = pigFile.images[i];
                ListEntry entry;
                entry.Name = img.name;
                entry.id = i;
                if (animOnly)
                {
                    if (img.isAnimated && img.frame == 0)
                        listBox1.Items.Add(entry);
                }
                else
                {
                    listBox1.Items.Add(entry);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Image image = pictureBox1.Image;
                pictureBox1.Image = null;
                image.Dispose();
            }
            ListEntry entry = (ListEntry)listBox1.Items[listBox1.SelectedIndex];
            selection = entry.id;
            pictureBox1.Image = PiggyBitmapConverter.GetBitmap(pigFile, entry.id);
        }
    }
}
