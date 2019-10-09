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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Descent2Workshop.Editor
{
    /// <summary>
    /// The possible input types of an input event
    /// </summary>
    public enum EventType
    {
        Key,
        MouseButton,
        MouseMove
    }

    /// <summary>
    /// An input event, to be handled by an IInputEventHandler.
    /// </summary>
    public class InputEvent
    {
        public EventType type;
        public Keys key;
        public MouseButtons mouseButton;
        public bool down;
        public int x, y;
        public int w, h;

        /// <summary>
        /// Creates a new Keyboard event.
        /// </summary>
        /// <param name="key">The keycode pressed.</param>
        /// <param name="down">True if the button was being pushed, or False if it is being released.</param>
        public InputEvent(Keys key, bool down)
        {
            this.down = down;
            this.key = key;
            type = EventType.Key;
        }

        /// <summary>
        /// Creates a new Mouse button event. 
        /// </summary>
        /// <param name="mouseButton">The mouse buttons pressed.</param>
        /// <param name="down">True if the button was being pushed, or False if it is being released.</param>
        public InputEvent(MouseButtons mouseButton, bool down)
        {
            this.down = down;
            this.mouseButton = mouseButton;
            type = EventType.MouseButton;
        }

        public InputEvent(int x, int y)
        {
            type = EventType.MouseMove;
            this.x = x; this.y = y;
        }
    }

    public interface IInputEventHandler
    {
        /// <summary>
        /// Handles a given input event.
        /// </summary>
        /// <param name="ev">The event for the handler to handle.</param>
        /// <returns>True if the event should not be passed further, or false if it should be passed further in the handler chain.</returns>
        bool HandleEvent(InputEvent ev);
    }
}
