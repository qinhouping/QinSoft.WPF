using EMChat2.Common;
using EMChat2.Common.PipeFilter;
using EMChat2.Model.BaseInfo;
using EMChat2.Model.Event;
using QinSoft.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Service.PipeFilter.SendMessage
{
    public class PushMessagePipeFilter : PipeFilterBase
    {
        private EventAggregator eventAggregator;
        public PushMessagePipeFilter(EventAggregator eventAggregator)
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
            if (IMTools.Send(message))
            {
                message.State = MessageStateEnum.SendSuccess;
                arg.OutArg = arg.InArg;
            }
            else
            {
                message.State = MessageStateEnum.SendFailure;
                arg.Cancel = true;
            }
            this.eventAggregator.PublishAsync<MessageStateChangedEventArgs>(new MessageStateChangedEventArgs() { Message = message });
        }
    }
}
