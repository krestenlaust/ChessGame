using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessForms
{
    public class TilePictureControl : PictureBox
    {
        public TilePictureControl()
        {
            this.SetStyle(ControlStyles.StandardDoubleClick, false);
        }
    }
}
