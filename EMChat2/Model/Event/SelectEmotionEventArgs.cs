using EMChat2.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Event
{
    public class SelectEmotionEventArgs : EventArgs
    {
        public EmotionInfo Emotion { get; set; }
    }
}
