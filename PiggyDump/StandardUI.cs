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
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using Descent2Workshop.SaveHandlers;
using LibDescent.Data;
using LibDescent.Edit;

namespace Descent2Workshop
{
    public partial class StandardUI : Form
    {
        private EditorHOGFile defaultHogFile;
        private PIGFile defaultPigFile;
        private SNDFile defaultSoundFile;
        public static UserOptions options = new UserOptions();

        public bool readyForUse = false;

        public SNDFile DefaultSoundFile { get => defaultSoundFile; }
        public PIGFile DefaultPigFile { get => defaultPigFile; }
        public EditorHOGFile DefaultHogFile { get => defaultHogFile; }
        public Palette DefaultPalette { get; private set; }

        public StandardUI()
        {
            InitializeComponent();

            options.openOptionsFile(Application.StartupPath + "/default.cfg");
            tbLog.Text += "\r\n-----------------------------\r\n";

            string defaultHOG = options.GetOption("HOGFile", "");
            string defaultPIG = options.GetOption("PIGFile", "");
            string defaultSound = options.GetOption("SNDFile", "");
            if (defaultHOG != "")
            {
                defaultHogFile = LoadDefaultHOG(defaultHOG);
                if (defaultHogFile != null)
                {
                    //Piggy is dependent on HOG palettes, so we need a HOG first. 
                    if (defaultPIG != "")
                    {
                        defaultPigFile = LoadDefaultPig(defaultPIG, defaultHogFile);
                    }
                }
            }

            if (defaultSound != "")
            {
                defaultSoundFile = LoadDefaultSND(defaultSound);
            }

            readyForUse = CheckReadyForUse();

            if (!readyForUse)
            {
                MessageBox.Show("It seems you haven't configured your default .HOG, .PIG, and .SXX files yet." +
                    "\nThis must be done before you can use D2 Workshop" +
                    "\nYou can do this in the Options menu");
            }
            //GraphicsContext.ShareContexts = false;

#if DEBUG == false
            mainMenu1.MenuItems.Remove(DebugMenu);
#endif
        }

        public void AppendConsole(string msg)
        {
            tbLog.Text += msg;
            tbLog.SelectionStart = tbLog.Text.Length - 1;
            tbLog.ScrollToCaret();
        }

        private void menuItem11_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = ".HOG files|*.HOG";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                EditorHOGFile archive = new EditorHOGFile();
                archive.Read(openFileDialog1.FileName);
                HOGEditor archiveEditor = new HOGEditor(archive, this);
                archiveEditor.Show();
            }
        }

        private void menuItem12_Click(object sender, EventArgs e)
        {
            EditorHOGFile archive = new EditorHOGFile();
            HOGEditor archiveEditor = new HOGEditor(archive, this);
            archiveEditor.Show();
        }

        private void LoadPIGMenu_Click(object sender, EventArgs e)
        {
            if (defaultHogFile == null)
            {
                MessageBox.Show("A default HOG file must be loaded before loading the default PIG file");
                return;
            }
            openFileDialog1.Filter = ".PIG files|*.PIG";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string paletteName = Path.GetFileNameWithoutExtension(openFileDialog1.FileName) + ".256";
                Palette newPalette; int lumpIndex;

                lumpIndex = defaultHogFile.GetLumpNum(paletteName);
                if (lumpIndex != -1) newPalette = new Palette(defaultHogFile.GetLumpData(lumpIndex));
                else newPalette = new Palette(); //If the palette couldn't be located, make a default grayscale palette

                PIGFile archive = new PIGFile();
                string statusMsg;
                if (FileUtilities.LoadDataFile(openFileDialog1.FileName, archive, out statusMsg))
                {
                    PIGEditor archiveEditor = new PIGEditor(archive, newPalette, openFileDialog1.FileName);
                    archiveEditor.host = this;
                    archiveEditor.Show();
                }
                else
                {
                    MessageBox.Show(statusMsg, "Error loading PIG file.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DefaultHOGMenu_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = ".HOG files|*.HOG";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                defaultHogFile = LoadDefaultHOG(openFileDialog1.FileName);
            }
        }

        private void StandardUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            options.WriteOptions(Application.StartupPath + "/default.cfg");
        }

        private void DefaultPIGMenu_Click(object sender, EventArgs e)
        {
            if (defaultHogFile == null)
            {
                AppendConsole("A default HOG file must be loaded before loading the default PIG file.");
                return;
            }
            openFileDialog1.Filter = ".PIG files|*.PIG";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                defaultPigFile = LoadDefaultPig(openFileDialog1.FileName, defaultHogFile);
            }
        }

        /// <summary>
        /// Helper function for loading a HAM file from a file.
        /// </summary>
        /// <param name="filename">The filename to load from.</param>
        /// <param name="dataFile">The HAM file to load data into.</param>
        /// <returns>True if no error, false if an error occurred.</returns>
        private bool LoadHAMFile(string filename, EditorHAMFile dataFile)
        {
            string statusMsg;
            if (!FileUtilities.LoadDataFile(filename, dataFile, out statusMsg))
            {
                MessageBox.Show(statusMsg, "Error loading HAM file.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Helper function for loading a HAM file from a file.
        /// </summary>
        /// <param name="filename">The filename to load from.</param>
        /// <param name="dataFile">The HAM file to load data into.</param>
        /// <returns>True if no error, false if an error occurred.</returns>
        private bool LoadHXMFile(string filename, EditorHXMFile dataFile)
        {
            string statusMsg;
            if (!FileUtilities.LoadDataFile(filename, dataFile, out statusMsg))
            {
                MessageBox.Show(statusMsg, "Error loading HXM file.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Helper function for loading a V-HAM file from a file.
        /// </summary>
        /// <param name="filename">The filename to load from.</param>
        /// <param name="dataFile">The HAM file to load data into.</param>
        /// <returns>True if no error, false if an error occurred.</returns>
        private bool LoadVHAMFile(string filename, EditorVHAMFile dataFile)
        {
            string statusMsg;
            if (!FileUtilities.LoadDataFile(filename, dataFile, out statusMsg))
            {
                MessageBox.Show(statusMsg, "Error loading V-HAM file.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void menuItem4_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = ".HAM files|*.HAM";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                EditorHAMFile archive = new EditorHAMFile(defaultPigFile);
                if (LoadHAMFile(openFileDialog1.FileName, archive))
                {
                    HAMEditor archiveEditor = new HAMEditor(archive, this, defaultPigFile, DefaultPalette, new FileSaveHandler(openFileDialog1.FileName));
                    archiveEditor.Show();
                }
            }
        }

        private void menuItem23_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = ".POF files|*.POF";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string traceto = "";
                if (bool.Parse(StandardUI.options.GetOption("TraceModels", bool.FalseString)))
                {
                    string bareFilename = Path.GetFileName(openFileDialog1.FileName);
                    traceto = StandardUI.options.GetOption("TraceDir", ".") + Path.DirectorySeparatorChar + Path.ChangeExtension(bareFilename, "txt");
                }

                Stream strim = File.OpenRead(openFileDialog1.FileName);
                Polymodel model = POFReader.ReadPOFFile(strim);
                strim.Close();
                strim.Dispose();
                PolymodelPreviewer archiveEditor = new PolymodelPreviewer(model, this);
                archiveEditor.Show();
            }
        }

        private void NewHXMFileMenu_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = ".HAM files|*.HAM";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                EditorHAMFile archive = new EditorHAMFile(defaultPigFile);
                if (LoadHAMFile(openFileDialog1.FileName, archive))
                {
                    EditorHXMFile hxm = new EditorHXMFile(archive);

                    HXMEditor editor = new HXMEditor(hxm, this, null);
                    editor.Show();
                }
            }
        }

        private void menuItem24_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = ".S** files|*.S22;*.S11";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bool closeOnExit = true;
                SNDFile archive;

                //Retaining this will be useful later, in case the default sound fiel changes.
                /*if (defaultSoundFile != null && Path.GetFullPath(openFileDialog1.FileName) == Path.GetFullPath(options.GetOption("SNDFile", "")))
                {
                    AppendConsole("Loading internal SND file for editing.");
                    archive = defaultSoundFile;
                    closeOnExit = false;
                }*/
                archive = new SNDFile();

                FileStream newSoundStream = File.OpenRead(openFileDialog1.FileName);
                archive.Read(newSoundStream);
                //TODO: Not strictly needed for now, but will be when it's possible to play out of the default SND file.
                SoundCache cache = SoundCache.CreateCacheFromFile(archive);
                newSoundStream.Close();

                SXXEditor archiveEditor = new SXXEditor(this, archive, cache, openFileDialog1.FileName, new FileSaveHandler(openFileDialog1.FileName));
                if (Path.GetExtension(openFileDialog1.FileName).Equals(".S11", StringComparison.OrdinalIgnoreCase) ||
                    Path.GetExtension(openFileDialog1.FileName).Equals(".HAM", StringComparison.OrdinalIgnoreCase))
                {
                    archiveEditor.isLowFi = true;
                }
                archiveEditor.closeOnExit = closeOnExit;
                archiveEditor.Show();
            }
        }

        private void menuItem25_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = ".S** files|*.S22;*.S11";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                /*defaultSoundFile = new SNDFile();
                soundStream = File.OpenRead(openFileDialog1.FileName);
                defaultSoundFile.Read(soundStream);
                options.SetOption("SNDFile", openFileDialog1.FileName);
                tbLog.Text += "Loaded .SXX!\r\n";*/

                //Gonna assume there's no reason why I can't do this.
                //I should know my own code better...
                LoadDefaultSND(openFileDialog1.FileName);
            }
        }

        private void OptionsMenu_Click(object sender, EventArgs e)
        {
            ConfigDialog config = new ConfigDialog();
            if (config.ShowDialog() == DialogResult.OK)
            {
                options.SetOption("HOGFile", config.HogFilename);
                options.SetOption("PIGFile", config.PigFilename);
                options.SetOption("SNDFile", config.SndFilename);
                options.SetOption("CompatObjBitmaps", config.NoPMView.ToString());
                options.SetOption("TraceModels", config.Traces.ToString());
                options.SetOption("TraceDir", config.TraceDir);
                options.SetOption("PMVersion", config.PofVer.ToString());

                defaultHogFile = LoadDefaultHOG(config.HogFilename);
                if (defaultHogFile != null)
                    defaultPigFile = LoadDefaultPig(config.PigFilename, defaultHogFile);
                else
                    AppendConsole("Could not load default PIG, since the default HOG is not loaded");
                defaultSoundFile = LoadDefaultSND(config.SndFilename);
            }
        }

        private void MnuAbout_Click(object sender, EventArgs e)
        {
#if DEBUG
            Stream stream = File.OpenRead("h:/descent/data/mac-full/DESCENT.PIG");
            Descent1PIGFile piggyFile = new Descent1PIGFile(true);
            piggyFile.Read(stream);
            stream.Dispose();
            PIGFile newPiggyFile = new PIGFile();
            for (int i = 1; i < piggyFile.Bitmaps.Count; i++)
            {
                newPiggyFile.Bitmaps.Add(piggyFile.Bitmaps[i]);
            }
            Palette palette = new Palette(defaultHogFile.GetLumpData("default.256"));
            PIGEditor editor = new PIGEditor(newPiggyFile, palette, "ara ara");
            editor.Show();

            /*HAMFile ham = new HAMFile();
            Stream stream = File.OpenRead("C:/Games/Descent/D2X-Rebirth/GOOD.HAM");
            ham.Read(stream);
            stream.Dispose();
            StreamWriter sw = new StreamWriter("goodtbls.txt");
            sw.WriteLine("Obj Bitmaps");
            for (int i = 0; i < ham.ObjBitmaps.Count; i++)
            {
                sw.WriteLine(ham.ObjBitmaps[i]);
            }
            sw.WriteLine("Obj Bitmaps Ptrs");
            for (int i = 0; i < ham.ObjBitmapPointers.Count; i++)
            {
                sw.WriteLine(ham.ObjBitmapPointers[i]);
            }
            sw.Close();
            ham = new HAMFile();
            stream = File.OpenRead("C:/Games/Descent/D2X-Rebirth/DESCENT2.HAM");
            ham.Read(stream);
            stream.Dispose();
            sw = new StreamWriter("newtbls.txt");
            sw.WriteLine("Obj Bitmaps");
            for (int i = 0; i < ham.ObjBitmaps.Count; i++)
            {
                sw.WriteLine(ham.ObjBitmaps[i]);
            }
            sw.WriteLine("Obj Bitmaps Ptrs");
            for (int i = 0; i < ham.ObjBitmapPointers.Count; i++)
            {
                sw.WriteLine(ham.ObjBitmapPointers[i]);
            }
            sw.Close();*/

#else
            AppendConsole("Descent II Workshop by ISB... heh\n");
#endif
        }

        private void menuItem6_Click_1(object sender, EventArgs e)
        {
            VHAMLoadDialog dialog = new VHAMLoadDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                EditorHAMFile dataFile = new EditorHAMFile(defaultPigFile);
                if (!LoadHAMFile(dialog.HAMFilename, dataFile))
                {
                    AppendConsole("VHAM load aborted.\r\n");
                    return;
                }
                EditorVHAMFile hxmFile = new EditorVHAMFile(dataFile);
                if (LoadVHAMFile(dialog.HXMFilename, hxmFile))
                {
                    VHAMEditor editor = new VHAMEditor(hxmFile, this);
                    editor.Show();
                }
            }
        }

        private EditorHOGFile LoadDefaultHOG(string filename)
        {
            EditorHOGFile defaultHOG = new EditorHOGFile();
            try
            {
                defaultHOG.Read(filename);
                AppendConsole("Loaded default HOG file!\r\n");
                options.SetOption("HOGFile", filename);
            }
            catch (Exception exc)
            {
                AppendConsole("Failed to load default HOG file!\r\n");
                FileExceptionHandler(exc, "HOG file");
                defaultHOG = null;
            }
            return defaultHOG;
        }

        private PIGFile LoadDefaultPig(string filename, EditorHOGFile hogFile)
        {
            string paletteName = Path.GetFileNameWithoutExtension(filename) + ".256";
            Palette newPalette; int lumpIndex;

            lumpIndex = defaultHogFile.GetLumpNum(paletteName);
            if (lumpIndex != -1) newPalette = new Palette(defaultHogFile.GetLumpData(lumpIndex));
            else newPalette = new Palette(); //If the palette couldn't be located, make a default grayscale palette
            DefaultPalette = newPalette;

            PIGFile defaultPIG = new PIGFile();
            try
            {
                /*defaultPIG.Read(filename);
                AppendConsole("Loaded default PIG file!\r\n");*/
                string statusMsg;
                if (FileUtilities.LoadDataFile(filename, defaultPIG, out statusMsg))
                {
                    options.SetOption("PIGFile", filename);
                    AppendConsole("Loaded default PIG file!\r\n");
                }
                else
                {
                    AppendConsole("Failed to load default PIG file:\r\n");
                    AppendConsole(statusMsg);
                    defaultPIG = null;
                }
                
            }
            catch (Exception)
            {
                AppendConsole("Failed to load default PIG file!\r\n");
                defaultPIG = null;
            }
            return defaultPIG;
        }

        private SNDFile LoadDefaultSND(string filename)
        {
            SNDFile defaultSND = new SNDFile();
            FileStream soundStream = null;
            try
            {
                soundStream = File.OpenRead(filename);
                defaultSND.Read(soundStream);
                AppendConsole("Loaded default SND file!\r\n");
                //if (defaultSoundFile != null)
                //    defaultSoundFile.CloseDataFile();
                options.SetOption("SNDFile", filename);

                defaultSoundFile = defaultSND;
            }
            catch (Exception exc)
            {
                AppendConsole("Failed to load default SND file!\r\n");
                FileExceptionHandler(exc, "SND file");
                defaultSND = null;
            }
            finally
            {
                if (soundStream != null)
                {
                    //TODO: ATM it's impossible to play sounds from the default sound file, so close the stream now.
                    //If this is changed, this needs to be changed and a SoundCache needs to be created.
                    soundStream.Dispose();
                }
            }
            return defaultSND;
        }

        private void FileExceptionHandler(Exception error, string context)
        {
            if (error is FileNotFoundException)
            {
                AppendConsole(String.Format("The specified {0} was not found.\r\n", context));
            }
            else if (error is UnauthorizedAccessException)
            {
                AppendConsole(String.Format("You do not have permission to access the specified {0}.\r\n", context));
            }
            else
            {
                AppendConsole(String.Format("Unhandled error loading {0}: {1}.\r\n", context, error.Message));
            }
        }

        private bool CheckReadyForUse()
        {
            return (defaultHogFile != null) && (defaultPigFile != null) && (defaultSoundFile != null);
        }

        private void OpenHXMMenu_Click(object sender, EventArgs e)
        {
            HXMLoadDialog dialog = new HXMLoadDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                EditorHAMFile dataFile = new EditorHAMFile(defaultPigFile);
                EditorVHAMFile augmentFile = null;
                if (!LoadHAMFile(dialog.HAMFilename, dataFile))
                {
                    AppendConsole("HXM load aborted.\r\n");
                    return;
                }
                if (dialog.VHAMFilename != "")
                {
                    augmentFile = new EditorVHAMFile(dataFile);
                    if (!LoadVHAMFile(dialog.VHAMFilename, augmentFile))
                    {
                        AppendConsole("HXM load aborted.\r\n");
                        return;
                    }
                }
                EditorHXMFile hxmFile = new EditorHXMFile(dataFile);
                if (augmentFile != null)
                    hxmFile.AugmentFile = augmentFile;

                if (LoadHXMFile(dialog.HXMFilename, hxmFile))
                {
                    HXMEditor editor = new HXMEditor(hxmFile, this, new FileSaveHandler(dialog.HXMFilename));
                    editor.Show();
                }
            }
        }

        private void ViewFNTMenu_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Font files|*.FNT";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                LibDescent.Data.Font font = new LibDescent.Data.Font();
                font.LoadFont(openFileDialog1.FileName);
                FontViewer viewer = new FontViewer(font);
                viewer.Show();
            }
        }

        private void OpenPOGMenu_Click(object sender, EventArgs e)
        {
            if (defaultHogFile == null)
            {
                MessageBox.Show("A default HOG file must be loaded before loading a POG file");
                return;
            }
            openFileDialog1.Filter = ".POG files|*.POG";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                POGFile archive = new POGFile();
                string statusMsg;
                if (FileUtilities.LoadDataFile(openFileDialog1.FileName, archive, out statusMsg))
                {
                    POGEditor archiveEditor = new POGEditor(archive, defaultHogFile, this, openFileDialog1.FileName);
                    archiveEditor.Show();
                }
                else
                {
                    MessageBox.Show(statusMsg, "Error loading POG file.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadLevelMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void DumpDescent1PigMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = ".PIG files|*.PIG";
            Descent1PIGFile piggyFile = new Descent1PIGFile(false);
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream stream = File.Open(openFileDialog1.FileName, FileMode.Open);
                piggyFile.Read(stream);
                stream.Close();
                stream.Dispose();
            }
            saveFileDialog1.FileName = "ignored";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Palette newPalette;
                int lumpIndex = defaultHogFile.GetLumpNum("DEFAULT.256");
                if (lumpIndex != -1) newPalette = new Palette(defaultHogFile.GetLumpData(lumpIndex));
                else newPalette = new Palette(); //If the palette couldn't be located, make a default grayscale palette
                DebugUtil.DumpDescent1PIGToBBM(piggyFile, newPalette, saveFileDialog1.FileName);
            }
        }

        private void DumpDescent2PigMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = ".PIG files|*.PIG";
            PIGFile piggyFile = new PIGFile();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream stream = File.Open(openFileDialog1.FileName, FileMode.Open);
                piggyFile.Read(stream);
                stream.Close();
                stream.Dispose();
            }
            saveFileDialog1.FileName = "ignored";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Palette newPalette;
                string paletteName = Path.GetFileNameWithoutExtension(openFileDialog1.FileName) + ".256";
                int lumpIndex = defaultHogFile.GetLumpNum(paletteName);
                if (lumpIndex != -1) newPalette = new Palette(defaultHogFile.GetLumpData(lumpIndex));
                else newPalette = new Palette(); //If the palette couldn't be located, make a default grayscale palette
                DebugUtil.DumpPIGToBBM(piggyFile, newPalette, saveFileDialog1.FileName);
            }
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            string[] levelnames = {"d2leva-1.rl2", "d2leva-2.rl2", "d2leva-3.rl2", "d2leva-4.rl2", "d2leva-S.rl2",
                            "d2levb-1.rl2", "d2levb-2.rl2", "d2levb-3.rl2", "d2levb-4.rl2", "d2levb-S.rl2",
                            "d2levc-1.rl2", "d2levc-2.rl2", "d2levc-3.rl2", "d2levc-4.rl2", "d2levc-S.rl2",
                            "d2levd-1.rl2", "d2levd-2.rl2", "d2levd-3.rl2", "d2levd-4.rl2", "d2levd-S.rl2",
                            "d2leve-1.rl2", "d2leve-2.rl2", "d2leve-3.rl2", "d2leve-4.rl2", "d2leve-S.rl2",
                            "d2levf-1.rl2", "d2levf-2.rl2", "d2levf-3.rl2", "d2levf-4.rl2", "d2levf-S.rl2", };

            openFileDialog1.Filter = ".HAM files|*.HAM";
            HAMFile hamfile = new HAMFile();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream stream = File.Open(openFileDialog1.FileName, FileMode.Open);
                hamfile.Read(stream);
                stream.Close();
                stream.Dispose();
            }

            int[] usenums = new int[hamfile.Textures.Count];

            foreach (string name in levelnames)
            {
                Stream lumpstream = defaultHogFile.GetLumpAsStream(name);
                D2Level level = D2Level.CreateFromStream(lumpstream);

                for (int i = 0; i < level.Segments.Count; i++)
                {
                    Segment seg = level.Segments[i];
                    for (int j = 0; j < 6; j++)
                    {
                        Side side = seg.GetSide((SegSide)j);
                        if (side.BaseTextureIndex < usenums.Length)
                            usenums[side.BaseTextureIndex]++;
                        else
                            AppendConsole(string.Format("Level {0}, segment {1} side {2}'s base texture ({3}) is invalid\r\n", name, i, j, side.BaseTextureIndex));
                        if (side.OverlayTextureIndex < usenums.Length)
                            usenums[side.OverlayTextureIndex]++;
                        else
                            AppendConsole(string.Format("Level {0}, segment {1} side {2}'s overlay texture ({3}) is invalid\r\n", name, i, j, side.OverlayTextureIndex));
                    }
                }

                for (int i = 0; i < level.Objects.Count; i++)
                {
                    LevelObject obj = level.Objects[i];
                    if (obj.RenderTypeID == RenderTypeID.Polyobj)
                    {
                        PolymodelRenderType renderType = (PolymodelRenderType)obj.RenderType;
                        if (renderType.TextureOverride != -1)
                        {
                            usenums[renderType.TextureOverride]++;
                        }
                    }
                }

                lumpstream.Dispose();
            }

            //Check all used textures
            /*for (int i = 0; i < usenums.Length; i++)
            {
                if (usenums[i] == 0)
                {
                    //AppendConsole(string.Format("Texture {0} is unused.\r\n", defaultPigFile.Bitmaps[hamfile.Textures[i]].Name));
                    PIGImage image = defaultPigFile.Bitmaps[hamfile.Textures[i]];
                    if (image.Frame == 0 || image.Frame == -1)
                    {
                        Bitmap bitmap = PiggyBitmapUtilities.GetBitmap(image, DefaultPalette);
                        string path = Path.Combine("C:/games/Descent/D2X-Rebirth/tcrf", string.Format("descent2-{0}.png", image.Name));
                        bitmap.Save(path);
                        bitmap.Dispose();
                    }
                }
            }*/

            StreamWriter sw = new StreamWriter("texturecount.txt");

            for (int i = 0; i < usenums.Length; i++)
            {
                PIGImage image = defaultPigFile.Bitmaps[hamfile.Textures[i]];
                if (image.Frame == -1 || image.Frame == 0)
                    sw.WriteLine("Texture {0}({1}) is used {2} times.", i, image.Name, usenums[i]);
            }

            sw.Dispose();
        }
    }
}
