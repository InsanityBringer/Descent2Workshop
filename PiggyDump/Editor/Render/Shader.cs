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
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Descent2Workshop.Editor.Render
{
    public class Shader
    {
        private int shaderid;
        //For debugging purposes atm
        private string name;

        public bool isValid = false;

        public int shaderID
        {
            get
            {
                return shaderid;
            }
        }

        public Shader(string name)
        {
            this.name = name;
        }

        public void UseShader()
        {
            GL.UseProgram(shaderid);
        }

        public void Init()
        {
            shaderid = GL.CreateProgram();
        }

        public void AddShader(string filename, ShaderType type)
        {
            Console.WriteLine("Adding shader {0}", filename);
            StreamReader sr = new StreamReader(File.Open(filename, FileMode.Open));
            string shaderSource = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();

            int id = GL.CreateShader(type);
            GL.ShaderSource(id, shaderSource);
            GL.CompileShader(id);

            int status;
            GL.GetShader(id, ShaderParameter.CompileStatus, out status);
            if (status != 1)
            {
                Console.WriteLine("Error compiling shader {0}:", filename);
                string infolog = GL.GetShaderInfoLog(id);
                Console.WriteLine(infolog);
            }
            else
            {
                GL.AttachShader(shaderid, id);
            }
        }

        public void LinkShader()
        {
            GL.LinkProgram(shaderid);
            int status;
            GL.GetProgram(shaderid, GetProgramParameterName.LinkStatus, out status);
            Console.WriteLine("Linking program");
            if (status != 1)
            {
                Console.WriteLine("Error linking program {0}: ");
                string log = GL.GetProgramInfoLog(shaderid);
                Console.WriteLine(log);
            }
            else
            {
                isValid = true;
            }
        }
    }
}
