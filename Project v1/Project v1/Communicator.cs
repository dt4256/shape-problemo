using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_v1
{
    public delegate void RadDelegate(object sender, RadiusEvent e);
    public class RadiusEvent : EventArgs
    {
        private int r;
        public RadiusEvent(int r) { this.r = r; }

        public int R{
            get { return r; }
            set { r = Math.Abs(value); }
        }
    }
}
