using EMChat2.Common.PipeFilter;
using EMChat2.Model.BaseInfo;
using EMChat2.Model.Event;
using QinSoft.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Service.PipeFilter.RecvMessage
{
    public class RenderMessagePipeFilter : PipeFilterBase
    {
        private EventAggregator eventAggregator;
        public RenderMessagePipeFilter(EventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }
        public override void Action(PipeFilterEventArgs arg)
        {
            if (!(arg.InArg is MessageInfo))
            {
                arg.Cancel = true;
                return;
            }

            MessageInfo message = arg.InArg as MessageInfo;
            this.eventAggregator.PublishAsync<ReceiveMessageEventArgs>(new ReceiveMessageEventArgs() { Message = message });

            arg.OutArg = arg.InArg;
        }
    }
}
