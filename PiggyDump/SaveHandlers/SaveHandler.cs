/*
    Copyright (c) 2020 SaladBadger

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

using System.IO;

namespace Descent2Workshop.SaveHandlers
{
    public abstract class SaveHandler
    {
        /// <summary>
        /// Gets the stream to write the data to.
        /// </summary>
        /// <returns>A stream to write data.</returns>
        public abstract Stream GetStream();
        /// <summary>
        /// Closes the current stream and performs any actions needed to ensure data is up to date.
        /// </summary>
        public abstract bool FinalizeStream();

        /// <summary>
        /// Destroys the current SaveHandler. Used to ensure data is properly detached in when required.
        /// </summary>
        public abstract void Destroy();

        /// <summary>
        /// Gets a name to show in the editor UI.
        /// </summary>
        /// <returns>The current filename.</returns>
        public abstract string GetUIName();

        public abstract string GetErrorMsg();
    }
}
