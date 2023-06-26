// from https://stackoverflow.com/questions/29157/how-do-i-make-a-picturebox-use-nearest-neighbor-resampling

using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Descent2Workshop
{
    /// <summary>
    /// Inherits from PictureBox; adds Interpolation Mode Setting
    /// </summary>
    public class PictureBoxInterpolation : PictureBox
    {
        public InterpolationMode InterpolationMode { get; set; }

        protected override void OnPaint(PaintEventArgs paintEventArgs)
        {
            paintEventArgs.Graphics.InterpolationMode = InterpolationMode;
            base.OnPaint(paintEventArgs);
        }
    }
}
