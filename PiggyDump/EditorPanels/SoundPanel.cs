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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Descent2Workshop.Transactions;
using LibDescent.Data;
using LibDescent.Edit;

namespace Descent2Workshop.EditorPanels
{
    public partial class SoundPanel : UserControl
    {
        private TransactionManager transactionManager;
        private int tabPage;

        private EditorHAMFile datafile;

        private int soundID;
        private bool isLocked = false;

        public SoundPanel(TransactionManager transactionManager, int tabPage, EditorHAMFile datafile, SNDFile soundFile)
        {
            InitializeComponent();
            this.transactionManager = transactionManager;
            this.tabPage = tabPage;
            this.datafile = datafile;

            SoundIDComboBox.Items.Clear();
            SoundIDComboBox.Items.Add("None");

            //TODO: This is slow, but is only done once for now. Should be fixed, though.
            foreach (SoundData sound in soundFile.sounds)
            {
                SoundIDComboBox.Items.Add(sound.name);
            }
        }

        public void Init(List<string> soundNames)
        {
            LowMemorySoundComboBox.Items.Clear();
            LowMemorySoundComboBox.Items.Add("None");
            LowMemorySoundComboBox.Items.AddRange(soundNames.ToArray());
        }

        public void ChangeOwnName(string newname)
        {
            LowMemorySoundComboBox.Items[soundID + 1] = newname;
        }

        public void Update(int id)
        {
            soundID = id;

            isLocked = true;
            if (datafile.Sounds[soundID] == 255)
                SoundIDComboBox.SelectedIndex = 0;
            else
                SoundIDComboBox.SelectedIndex = datafile.Sounds[soundID] + 1;
            if (datafile.AltSounds[soundID] == 255)
                LowMemorySoundComboBox.SelectedIndex = 0;
            else
                LowMemorySoundComboBox.SelectedIndex = datafile.AltSounds[soundID] + 1;
            isLocked = false;
        }

        private void SoundIDComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress) return;

            ComboBox control = (ComboBox)sender;
            int value = control.SelectedIndex - 1;
            if (value < 0) value = 255;

            ListReplaceTransaction transaction = new ListReplaceTransaction("Sound id", datafile, (string)control.Tag, soundID, (byte)value, soundID, tabPage);
            transactionManager.ApplyTransaction(transaction);
        }
    }
}
