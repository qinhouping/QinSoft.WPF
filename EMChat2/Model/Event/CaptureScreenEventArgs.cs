using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Event
{
    public class CaptureScreenEventArgs : EventArgs
    {
        public CaptureScreenAction Action;
    }

    public enum CaptureScreenAction
    {
        Begin,
        End
    }
}
