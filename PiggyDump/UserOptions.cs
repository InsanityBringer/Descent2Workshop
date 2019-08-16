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
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace PiggyDump
{
    public class UserOptions
    {
        public struct UserOption
        {
            public string name;
            public string value;
        }

        //List<UserOption> al = new List<UserOption>();
        Dictionary<string, string> options = new Dictionary<string, string>();
        public void openOptionsFile(string filename)
        {
            if (filename != "")
            {
                StreamReader sr = null;
                try
                {
                    sr = new StreamReader(File.Open(filename, FileMode.Open));
                    string currentOption = sr.ReadLine();
                    while (currentOption != null)
                    {
                        string[] optionData = currentOption.Split('=');
                        if (optionData.Length < 2)
                        {
                            throw new Exception("Syntax error while parsing config file");
                        }
                        else
                        {
                            options.Add(optionData[0], optionData[1]);
                        }
                        currentOption = sr.ReadLine();
                    }

                }
                catch (Exception)
                {
                    //log.Items.Add("error while loading config file -- may not exist");
                }

                if (sr != null)
                {
                    sr.Close();
                }
            }
        }

        public string GetOption(string optionName, string defaultValue)
        {
            if (options.ContainsKey(optionName))
                return options[optionName];
            //If the option doesn't exist, silently create it for archival later.
            options.Add(optionName, defaultValue);
            return defaultValue;
        }

        public void SetOption(string optionName, string defaultValue)
        {
            if (options.ContainsKey(optionName))
                options[optionName] = defaultValue;
            else
                options.Add(optionName, defaultValue);
        }

        public void WriteOptions(string filename)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(filename,false);
                /*for (int x = 0; x < al.Count; x++)
                {
                    UserOption uo = (UserOption)al[x];
                    sw.WriteLine(String.Format("{0}={1}",uo.name,uo.value));
                }*/
                foreach (KeyValuePair<string, string> option in options)
                {
                    sw.WriteLine("{0}={1}", option.Key, option.Value);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("error saving file to " + filename);
            }

            if (sw != null)
            {
                sw.Close();
            }
        }
    }
}
