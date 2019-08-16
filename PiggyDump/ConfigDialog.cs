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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PiggyDump
{
    public partial class ConfigDialog : Form
    {
        public ConfigDialog()
        {
            InitializeComponent();
        }

        private void ConfigDialog_Load(object sender, EventArgs e)
        {
            txtHogFilename.Text = StandardUI.options.GetOption("HOGFile", "");
            txtPigFilename.Text = StandardUI.options.GetOption("PIGFile", "");
            txtSndFilename.Text = StandardUI.options.GetOption("SNDFile", "");
            chkNoPMView.Checked = bool.Parse(StandardUI.options.GetOption("CompatObjBitmaps", bool.FalseString));
            chkTraces.Checked = bool.Parse(StandardUI.options.GetOption("TraceModels", bool.FalseString));
            txtTraceDir.Text = StandardUI.options.GetOption("TraceDir", "");
            cbPofVer.SelectedIndex = int.Parse(StandardUI.options.GetOption("PMVersion", "8")) - 7;
        }

        public string HogFilename { get { return txtHogFilename.Text; } }
        public string PigFilename { get { return txtPigFilename.Text; } }
        public string SndFilename { get { return txtSndFilename.Text; } }
        public string TraceDir { get { return txtTraceDir.Text; } }
        public bool Traces { get { return chkTraces.Checked; } }
        public bool NoPMView { get { return chkNoPMView.Checked; } }
        public int PofVer { get { return cbPofVer.SelectedIndex + 7; } }
    }
}
