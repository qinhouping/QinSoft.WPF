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
using EMChat2.Model.Enum;

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
            if (!(arg.InArg is MessageModel))
            {
                arg.Cancel = true;
                return;
            }

            MessageModel message = arg.InArg as MessageModel;
            if (message.Type == MessageTypeConst.Event)
            {
                if (message.IsTimeValid())
                {
                    EventMessageContent eventMessage = message.Content as EventMessageContent;
                    switch (eventMessage.Event)
                    {
                        case EventMessageTypeConst.RecvMessage: HandleRecvMessageEvent(message, eventMessage as RecvMessageEventMessageContent); break;
                        case EventMessageTypeConst.ReadMessage: HandleReadMessageEvent(message, eventMessage as ReadMessageEventMessageContent); break;
                        case EventMessageTypeConst.RefuseMessage: HandleRefuseMessageEvent(message, eventMessage as RefuseMessageEventMessageContent); break;
                        case EventMessageTypeConst.RevokeMessage: HandleRevokeMessageEvent(message, eventMessage as RevokeMessageEventMessageContent); break;
                        case EventMessageTypeConst.InputMessage: HandleInputMessageEvent(message, eventMessage as InputMessageEventMessageContent); break;
                    }
                }
                arg.Cancel = true;
            }
            else
            {
                arg.OutArg = message;
            }
        }

        protected virtual void HandleRecvMessageEvent(MessageModel message, RecvMessageEventMessageContent messageContent)
        {
            this.eventAggregator.PublishAsync<MessageStateChangedEventArgs>(new MessageStateChangedEventArgs() { ChatId = message.ChatId, MessageId = messageContent.MessageId, MessageState = MessageStateEnum.Received });
        }

        protected virtual void HandleReadMessageEvent(MessageModel message, ReadMessageEventMessageContent messageContent)
        {
            foreach (string messageId in messageContent.MessageIds)
            {
                this.eventAggregator.PublishAsync<MessageStateChangedEventArgs>(new MessageStateChangedEventArgs() { ChatId = message.ChatId, MessageId = messageId, MessageState = MessageStateEnum.Readed });
            }
        }

        protected virtual void HandleRefuseMessageEvent(MessageModel message, RefuseMessageEventMessageContent messageContent)
        {
            this.eventAggregator.PublishAsync<MessageStateChangedEventArgs>(new MessageStateChangedEventArgs() { ChatId = message.ChatId, MessageId = messageContent.MessageId, MessageState = MessageStateEnum.Refused });
        }

        protected virtual void HandleRevokeMessageEvent(MessageModel message, RevokeMessageEventMessageContent messageContent)
        {
            this.eventAggregator.PublishAsync<MessageStateChangedEventArgs>(new MessageStateChangedEventArgs() { ChatId = message.ChatId, MessageId = messageContent.MessageId, MessageState = MessageStateEnum.Revoked });
        }

        protected virtual void HandleInputMessageEvent(MessageModel message, InputMessageEventMessageContent messageContent)
        {
            this.eventAggregator.PublishAsync<InputMessageChangedEventArgs>(new InputMessageChangedEventArgs() { ChatId = messageContent.ChatId, IsInputing = messageContent.IsInputing });
        }

    }
}
