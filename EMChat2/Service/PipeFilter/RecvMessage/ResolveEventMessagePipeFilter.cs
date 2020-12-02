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

namespace EMChat2.Service.PipeFilter.RecvMessage
{
    public class ResolveEventMessagePipeFilter : PipeFilterBase
    {
        private EventAggregator eventAggregator;
        public ResolveEventMessagePipeFilter(EventAggregator eventAggregator)
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
            if (message.Type == MessageTypeConst.Event)
            {
                EventMessageContentBase eventMessage = MessageTools.ParseMessageContent(message) as EventMessageContentBase;
                switch (eventMessage.Event)
                {
                    case EventTypeConst.RecvMessage: HandleRecvMessageEvent(message, eventMessage as RecvMessageEventMessageContent); break;
                    case EventTypeConst.ReadMessage: HandleReadMessageEvent(message, eventMessage as ReadMessageEventMessageContent); break;
                    case EventTypeConst.RefuseMessage: HandleRefuseMessageEvent(message, eventMessage as RefuseMessageEventMessageContent); break;
                    case EventTypeConst.RevokeMessage: HandleRevokeMessageEvent(message, eventMessage as RevokeMessageEventMessageContent); break;
                }
                arg.Cancel = true;
            }
            else
            {
                arg.OutArg = arg.InArg;
            }
        }

        protected virtual void HandleRecvMessageEvent(MessageInfo message, RecvMessageEventMessageContent messageContent)
        {
            this.eventAggregator.PublishAsync<MessageStateChangedEventArgs>(new MessageStateChangedEventArgs() { Message = messageContent.Message });
        }

        protected virtual void HandleReadMessageEvent(MessageInfo message, ReadMessageEventMessageContent messageContent)
        {
            this.eventAggregator.PublishAsync<MessageStateChangedEventArgs>(new MessageStateChangedEventArgs() { Message = messageContent.Message });
        }

        protected virtual void HandleRefuseMessageEvent(MessageInfo message, RefuseMessageEventMessageContent messageContent)
        {
            this.eventAggregator.PublishAsync<MessageStateChangedEventArgs>(new MessageStateChangedEventArgs() { Message = messageContent.Message });
        }

        protected virtual void HandleRevokeMessageEvent(MessageInfo message, RevokeMessageEventMessageContent messageContent)
        {
            this.eventAggregator.PublishAsync<MessageStateChangedEventArgs>(new MessageStateChangedEventArgs() { Message = messageContent.Message });
        }
    }
}
