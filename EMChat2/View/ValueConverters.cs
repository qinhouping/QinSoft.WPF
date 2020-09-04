using EMChat2.Common;
using EMChat2.Model.Entity;
using EMChat2.View.Main.Tabs.Chat;
using EMChat2.ViewModel.Main.Tabs.Chat;
using QinSoft.WPF;
using QinSoft.WPF.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EMChat2.View
{
    public static class ValueConverters
    {
        public static IValueConverter MessageToContentMarkConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    MessageInfo message = value as MessageInfo;
                    if (message == null) return null;
                    else return MessageTools.GetMessageContentMark(message.Type, message.Content);
                });
            }
        }

        public static IMultiValueConverter MessageToHorizontalAlignment
        {
            get
            {
                return new DelegateMultiValueConverter((values, targetType, parameter, cultInfo) =>
                {
                    MessageInfo message = values[0] as MessageInfo;
                    StaffInfo staff = values[1] as StaffInfo;
                    if (message == null || staff == null) return null;
                    if (message.FromUser.Equals(staff.ImUserId) == true) return HorizontalAlignment.Right;
                    else return HorizontalAlignment.Left;
                });
            }
        }

        public static IMultiValueConverter MessageToUserConverter
        {
            get
            {
                return new DelegateMultiValueConverter((values, targetType, parameter, cultInfo) =>
                {
                    MessageInfo message = values[0] as MessageInfo;
                    ChatInfo chat = values[1] as ChatInfo;
                    if (message == null || chat == null) return null;
                    return chat.ChatUsers.FirstOrDefault(u => u.ImUserId.Equals(message.FromUser))?.Name;
                });
            }
        }

        #region 私有方法
        private static void ParseMessageContentToDocument(FlowDocument document, Block block, string messageType, string messageContent)
        {
            if (block == null) { block = new Paragraph(); document.Blocks.Add(block); }
            switch (messageType)
            {
                case MessageTypeConst.Text:
                    {
                        TextMessageContent textMessageContent = MessageTools.ParseMessageContent(messageType, messageContent) as TextMessageContent;
                        string[] messageContentItems = Regex.Split(textMessageContent.Content, Environment.NewLine);
                        for (int i = 0; i < messageContentItems.Length; i++)
                        {
                            string messageContentItem = messageContentItems[i];
                            if (block is BlockUIContainer)
                            {
                                block = new Paragraph(); document.Blocks.Add(block);
                                (block as Paragraph).Inlines.Add(messageContentItem.Trim());
                            }
                            else if (block is Paragraph)
                            {
                                (block as Paragraph).Inlines.Add(messageContentItem.Trim());
                            }
                            if (i < messageContentItems.Length - 1)
                                block = new Paragraph(); document.Blocks.Add(block);
                        }
                    }; break;
                case MessageTypeConst.Emotion:
                case MessageTypeConst.Image:
                case MessageTypeConst.Voice:
                case MessageTypeConst.Video:
                case MessageTypeConst.Link:
                case MessageTypeConst.File:
                    {
                        if (block is BlockUIContainer)
                        {
                            (block as BlockUIContainer).Child = new ChatMessageContentControlView()
                            {
                                DataContext = new ChatMessageContentControlViewModel(messageType, MessageTools.ParseMessageContent(messageType, messageContent))
                            };
                            block = new Paragraph(); document.Blocks.Add(block);
                        }
                        else if (block is Paragraph)
                        {
                            (block as Paragraph).Inlines.Add(new ChatMessageContentControlView()
                            {
                                DataContext = new ChatMessageContentControlViewModel(messageType, MessageTools.ParseMessageContent(messageType, messageContent))
                            });
                        }
                    }; break;
                case MessageTypeConst.Mixed:
                    {
                        foreach (MessageContentInfo mixedItem in (MessageTools.ParseMessageContent(messageType, messageContent) as MixedMessageContent).Items)
                            ParseMessageContentToDocument(document, block, mixedItem.Type, mixedItem.Content);
                    }; break;
                case MessageTypeConst.Tips:
                case MessageTypeConst.Event:
                    break;
            }
        }

        private static void ParseInlinesToMessageContent(Inline[] inlines, List<MessageContentInfo> messageContents, ref MessageContentInfo messageContent)
        {
            foreach (Inline inline in inlines)
            {
                if (inline is Run)
                {
                    Run run = inline as Run;
                    if (messageContent == null || messageContent.Type != MessageTypeConst.Text)
                    {
                        messageContent = new MessageContentInfo { Type = MessageTypeConst.Text, Content = new TextMessageContent() { Content = run.Text }.ObjectToJson() };
                        messageContents.Add(messageContent);
                    }
                    else
                    {
                        TextMessageContent textMessageContent = MessageTools.ParseMessageContent(messageContent.Type, messageContent.Content) as TextMessageContent;
                        textMessageContent.Content += string.Format("{0}{1}", Environment.NewLine, run.Text);
                        messageContent.Content = textMessageContent.ObjectToJson();
                    }
                }
                else if (inline is InlineUIContainer)
                {
                    InlineUIContainer inlineUIContainer = inline as InlineUIContainer;
                    if (inlineUIContainer.Child is ChatMessageContentControlView)
                    {
                        ChatMessageContentControlView chatMessageContentControlView = inlineUIContainer.Child as ChatMessageContentControlView;
                        ChatMessageContentControlViewModel chatMessageContentControlViewModel = chatMessageContentControlView.DataContext as ChatMessageContentControlViewModel;
                        messageContent = new MessageContentInfo { Type = chatMessageContentControlViewModel.MsgType, Content = chatMessageContentControlViewModel.MsgContent.ObjectToJson() };
                        messageContents.Add(messageContent);
                    }
                }
                else if (inline is Span)
                {
                    Span span = inline as Span;
                    ParseInlinesToMessageContent(span.Inlines.ToArray(), messageContents, ref messageContent);
                }
            }
        }

        private static void ParseDocumentToMessageContent(FlowDocument document, List<MessageContentInfo> messageContents)
        {
            MessageContentInfo messageContentInfo = null;
            foreach (Block block in document.Blocks)
            {
                if (block is BlockUIContainer)
                {
                    BlockUIContainer blockUIContainer = block as BlockUIContainer;
                    if (blockUIContainer.Child is ChatMessageContentControlView)
                    {
                        ChatMessageContentControlView chatMessageContentControlView = blockUIContainer.Child as ChatMessageContentControlView;
                        ChatMessageContentControlViewModel chatMessageContentControlViewModel = chatMessageContentControlView.DataContext as ChatMessageContentControlViewModel;
                        messageContentInfo = new MessageContentInfo { Type = chatMessageContentControlViewModel.MsgType, Content = chatMessageContentControlViewModel.MsgContent.ObjectToJson() };
                        messageContents.Add(messageContentInfo);
                    }
                }
                else if (block is Paragraph)
                {
                    Paragraph paragraph = block as Paragraph;
                    ParseInlinesToMessageContent(paragraph.Inlines.ToArray(), messageContents, ref messageContentInfo);
                }
            }
        }
        #endregion

        public static IValueConverter MessageToDocumentConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    FlowDocumentExt flowDocument = new FlowDocumentExt();
                    MessageContentInfo message = value as MessageContentInfo;
                    if (message != null)
                        ParseMessageContentToDocument(flowDocument, null, message.Type, message.Content);
                    return flowDocument;
                }, (value, targetType, parameter, cultInfo) =>
                {
                    FlowDocumentExt document = value as FlowDocumentExt;
                    if (document == null) return null;
                    List<MessageContentInfo> messageContents = new List<MessageContentInfo>();
                    ParseDocumentToMessageContent(document, messageContents);
                    if (messageContents.Count > 1)
                    {
                        return new MessageContentInfo() { Type = MessageTypeConst.Mixed, Content = messageContents.ObjectToJson() };
                    }
                    else if (messageContents.Count == 1)
                    {
                        return messageContents[0];
                    }
                    else
                    {
                        return null;
                    }
                });
            }
        }

        public static IValueConverter TipsMessageToStringConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    MessageContentInfo message = value as MessageContentInfo;
                    if (message == null) return null;
                    if (message.Type == MessageTypeConst.Tips)
                    {
                        TipsMessageContent tipsMessageContent = MessageTools.ParseMessageContent(message.Type, message.Content) as TipsMessageContent;
                        return tipsMessageContent.Content;
                    }
                    else
                    {
                        return "非提示信息";
                    }
                });
            }
        }
    }
}
