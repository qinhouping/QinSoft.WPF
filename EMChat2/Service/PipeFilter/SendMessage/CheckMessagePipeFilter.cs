using EMChat2.Common.PipeFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Service.PipeFilter.SendMessage
{
    public class CheckMessagePipeFilter : PipeFilterBase
    {
        public override void Action(PipeFilterEventArgs arg)
        {
            arg.OutArg = arg.InArg;
        }
    }
}
