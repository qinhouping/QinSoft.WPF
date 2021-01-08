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
using EMChat2.Model.Enum;
using Hardcodet.Wpf.TaskbarNotification;

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
                        if (SendMessage(message))
                        {
                            arg.OutArg = message;
                        }
                        else
                        {
                            arg.Cancel = true;
                        }
                        if (UpdateMessage(message.Id, message.State))
                        {
                            this.eventAggregator.PublishAsync<MessageStateChangedEventArgs>(new MessageStateChangedEventArgs()
                            {
                                ChatId = message.ChatId,
                                MessageId = message.Id,
                                MessageState = message.State
                            });
                        }
                    }
                    break;
                case MessageTypeConst.Tips:
                case MessageTypeConst.Event:
                    {
                        if (SendMessage(message))
                        {
                            arg.OutArg = message;
                        }
                        else
                        {
                            arg.Cancel = true;
                        }
                    }
                    break;
                default:
                    {
                        arg.Cancel = true;
                    }
                    break;
            }
        }

        protected virtual bool SendMessage(MessageModel message)
        {
            if (!IMTools.Send(message, out string error))
            {
                message.State = MessageStateEnum.SendFailure;
                this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "消息发送失败",
                        Content = error,
                        Icon = BalloonIcon.Error
                    }
                });
                return false;
            }
            else
            {
                message.State = MessageStateEnum.SendSuccess;
                return true;
            }
        }

        protected virtual bool UpdateMessage(string messageId, MessageStateEnum state)
        {
            if (!ApiTools.ModifyMessage(messageId, state, out string error))
            {
                this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "消息更新失败",
                        Content = error,
                        Icon = BalloonIcon.Error
                    }
                });

                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
