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
        /// <summary>
        /// 开始截屏
        /// </summary>
        Begin,

        /// <summary>
        /// 结束截屏
        /// </summary>
        End
    }
}
