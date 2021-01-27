using System;
using System.Windows.Forms;

namespace ChessForms
{
    public class TilePictureControl : PictureBox
    {
        public TilePictureControl()
        {
            SetStyle(ControlStyles.StandardDoubleClick, false);
            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.None;
            SizeMode = PictureBoxSizeMode.Zoom;
            Padding = new Padding(0);
            Margin = new Padding(0);
        }
    }
}
