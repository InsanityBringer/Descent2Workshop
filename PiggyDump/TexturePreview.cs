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
using System.Windows.Forms;

namespace Descent2Workshop
{
    public partial class TexturePreview : Form
    {
        //public int index = 0;
        public TexturePreview()
        {
            InitializeComponent();
        }

        public void fillOutTextureBox(HAMFile bmData)
        {
            /*int max = bmData.NumBitmaps;
            for (int x = 0; x < max; x++)
            {
                Bitmap bm = bmData.LookUpImageByIndex(bmData.Textures[x]).GetPicture();
                imageList1.Images.Add(bm);
                //bm.Dispose();
                string name = new String(bmData.TMapInfo[x].filename);
                ListViewItem lvi = new ListViewItem(name);
                lvi.ImageIndex = x;
                listView1.Items.Add(lvi);
            }
            listView1.LargeImageList = imageList1;*/
        }

        public int getIndex()
        {
            ListViewItem lvi = listView1.SelectedItems[0];
            return lvi.Index;
        }
    }
}
