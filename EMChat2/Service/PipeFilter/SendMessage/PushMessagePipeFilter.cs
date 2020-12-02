using EMChat2.Common;
using EMChat2.Common.PipeFilter;
using EMChat2.Model.BaseInfo;
using EMChat2.Event;
using QinSoft.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMChat2.Model.Api;

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
            if (!(arg.InArg is MessageModel))
            {
                arg.Cancel = true;
                return;
            }

            MessageModel message = arg.InArg as MessageModel;
            
            if (IMTools.Send(message))
            {
                message.State = MessageStateEnum.SendSuccess;
                arg.OutArg = message;
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
