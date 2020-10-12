using EMChat2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Event
{
    public class ShowBalloonTipEventArgs : EventArgs
    {
        public BalloonTipInfo BalloonTip { get; set; }
    }
}
