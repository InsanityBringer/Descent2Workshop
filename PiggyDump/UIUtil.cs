using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Descent2Workshop
{
    public static class UIUtil
    {
        public static void SafeFillComboBox(ComboBox comboBox, int value)
        {
            if (value < -1 || value >= comboBox.Items.Count)
            {
                comboBox.SelectedIndex = -1;
                //comboBox.Text = string.Format("<{0}>", value); //Sadly, text isn't rendered on a -1 combo box. 
            }
            else
                comboBox.SelectedIndex = value;
        }
    }
}
