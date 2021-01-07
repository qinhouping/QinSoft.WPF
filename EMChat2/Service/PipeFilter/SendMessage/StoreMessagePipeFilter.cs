using EMChat2.Common;
using EMChat2.Common.PipeFilter;
using EMChat2.Event;
using EMChat2.Model.Api;
using EMChat2.Model.BaseInfo;
using EMChat2.Model.Enum;
using Hardcodet.Wpf.TaskbarNotification;
using QinSoft.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Service.PipeFilter.SendMessage
{
    public class StoreMessagePipeFilter : PipeFilterBase
    {
        private EventAggregator eventAggregator;
        public StoreMessagePipeFilter(EventAggregator eventAggregator)
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

            switch (message.Type)
            {
                case MessageTypeConst.Text:
                case MessageTypeConst.Emotion:
                case MessageTypeConst.Image:
                case MessageTypeConst.Voice:
                case MessageTypeConst.Video:
                case MessageTypeConst.Link:
                case MessageTypeConst.File:
                case MessageTypeConst.Mixed:
                case MessageTypeConst.Shared:
                    {
                        if (AddMessage(message))
                        {
                            arg.OutArg = message;
                        }
                        else
                        {
                            arg.Cancel = true;
                        }
                    }
                    break;
                case MessageTypeConst.Tips:
                    {
                        arg.Cancel = true;
                    }
                    break;
                case MessageTypeConst.Event:
                    {
                        arg.OutArg = message;
                    }
                    break;
                default:
                    {
                        arg.Cancel = true;
                    }
                    break;
            }
        }

        protected virtual bool AddMessage(MessageModel message)
        {
            if (!ApiTools.AddMessage(message, out string error, out string messageId))
            {
                this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "消息保存失败",
                        Content = error,
                        Icon = BalloonIcon.Error
                    }
                });

                message.State = MessageStateEnum.SendFailure;
                this.eventAggregator.PublishAsync<MessageStateChangedEventArgs>(new MessageStateChangedEventArgs()
                {
                    ChatId = message.ChatId,
                    MessageId = message.Id,
                    MessageState = MessageStateEnum.SendFailure
                });
                return false;
            }
            else
            {
                this.eventAggregator.PublishAsync<MessageIdChangedEventArgs>(new MessageIdChangedEventArgs()
                {
                    ChatId = message.ChatId,
                    MessageId = message.Id,
                    NewMessageId = messageId
                });
                message.Id = messageId;
                return true;
            }
        }

        protected virtual void UpdateMessage(string messageId, MessageStateEnum state)
        {

        }
    }
}
