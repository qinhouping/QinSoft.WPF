using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Event
{
    public class SelectFileEventArgs : EventArgs
    {
        public FileInfo File { get; set; }
    }
}
