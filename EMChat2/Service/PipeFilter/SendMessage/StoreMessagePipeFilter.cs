using EMChat2.Common.PipeFilter;
using EMChat2.Model.Api;
using EMChat2.Model.BaseInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Service.PipeFilter.SendMessage
{
    public class StoreMessagePipeFilter : PipeFilterBase
    {
        public override void Action(PipeFilterEventArgs arg)
        {
            if (!(arg.InArg is MessageModel))
            {
                arg.Cancel = true;
                return;
            }

            MessageModel message = arg.InArg as MessageModel;

            if (message.Type == MessageTypeConst.Event)
            {
                arg.Cancel = true;
            }
            else
            {
                arg.OutArg = message;
            }
        }
    }
}
