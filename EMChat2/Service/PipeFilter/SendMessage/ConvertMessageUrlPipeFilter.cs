using EMChat2.Common;
using EMChat2.Common.PipeFilter;
using EMChat2.Event;
using EMChat2.Model.BaseInfo;
using EMChat2.Model.IM;
using Hardcodet.Wpf.TaskbarNotification;
using QinSoft.Event;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Service.PipeFilter.SendMessage
{
    public class ConvertMessageUrlPipeFilter : PipeFilterBase
    {
        private EventAggregator eventAggregator;
        public ConvertMessageUrlPipeFilter(EventAggregator eventAggregator)
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
            if (Convert(message))
            {
                arg.OutArg = message;
            }
            else
            {
                message.State = MessageStateEnum.SendFailure;
                this.eventAggregator.PublishAsync<MessageStateChangedEventArgs>(new MessageStateChangedEventArgs() { Message = message });
                arg.Cancel = true;
            }
        }

        protected virtual bool Convert(MessageContentModel message)
        {
            bool res = true;
            switch (message.Type)
            {
                case MessageTypeConst.Text: break;
                case MessageTypeConst.Emotion: break;
                case MessageTypeConst.Image:
                    {
                        ImageMessageContent messageContent = message.Content as ImageMessageContent;
                        if (!messageContent.Url.IsNetUrl())
                        {
                            IMImageInfo image = IMTools.UploadImage(new FileInfo(messageContent.Url), out string error);
                            if (image != null)
                            {
                                messageContent.Url = image.Url;
                            }
                            else
                            {
                                this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                                {
                                    BalloonTip = new BalloonTipInfo
                                    {
                                        Title = "上传图片失败",
                                        Content = error,
                                        Icon = BalloonIcon.Error
                                    }
                                });
                                res = false;
                            }
                        }
                    }
                    break;
                case MessageTypeConst.Voice: break;
                case MessageTypeConst.Video: break;
                case MessageTypeConst.Link: break;
                case MessageTypeConst.File:
                    {
                        FileMessageContent messageContent = message.Content as FileMessageContent;
                        if (!messageContent.Url.IsNetUrl())
                        {
                            if (messageContent.Url.IsImageFile())
                            {
                                IMImageInfo image = IMTools.UploadImage(new FileInfo(messageContent.Url), out string error);
                                if (image != null)
                                {
                                    messageContent.Url = image.Url;
                                }
                                else
                                {
                                    this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                                    {
                                        BalloonTip = new BalloonTipInfo
                                        {
                                            Title = "上传图片失败",
                                            Content = error,
                                            Icon = BalloonIcon.Error
                                        }
                                    });
                                    res = false;
                                }
                            }
                            else
                            {
                                IMFileInfo file = IMTools.UploadFile(new FileInfo(messageContent.Url), out string error);
                                if (file != null)
                                {
                                    messageContent.Url = file.Url;
                                }
                                else
                                {
                                    this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                                    {
                                        BalloonTip = new BalloonTipInfo
                                        {
                                            Title = "上传文件失败",
                                            Content = error,
                                            Icon = BalloonIcon.Error
                                        }
                                    });
                                    res = false;
                                }
                            }
                        }
                    }
                    break;
                case MessageTypeConst.Mixed:
                    {
                        MixedMessageContent messageContent = message.Content as MixedMessageContent;
                        foreach (MessageContentModel item in messageContent.Items)
                        {
                            if (res) res &= Convert(item);
                        }
                    }
                    break;
                case MessageTypeConst.Tips: break;
                case MessageTypeConst.Event: break;
            }
            return res;
        }
    }
}
