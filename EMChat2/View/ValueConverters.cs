using EMChat2.Common;
using EMChat2.Model.BaseInfo;
using EMChat2.View.Main.Body.Chat;
using EMChat2.ViewModel.Main.Tabs.Chat;
using QinSoft.WPF;
using QinSoft.WPF.Control;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace EMChat2.View
{
    public static class ValueConverters
    {
        public static IMultiValueConverter ShellbarButtonIsEnabledConverter
        {
            get
            {
                return new DelegateMultiValueConverter((values, targetType, parameter, cultInfo) =>
                {
                    ResizeMode resize = (ResizeMode)values[0];
                    WindowState currentState = (WindowState)values[1];
                    WindowState targetState = (WindowState)parameter;
                    switch (resize)
                    {
                        case ResizeMode.CanMinimize:
                            {
                                return targetState == WindowState.Minimized;
                            }
                        case ResizeMode.CanResize:
                            {
                                return targetState != currentState;
                            }
                        case ResizeMode.CanResizeWithGrip:
                            {
                                return targetState != currentState;
                            }
                        case ResizeMode.NoResize:
                            {
                                return targetState == WindowState.Minimized;
                            }
                        default:
                            return false;
                    }
                });
            }
        }

        public static IValueConverter MessageToContentMarkConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    MessageContentModel messageContent = value as MessageContentModel;
                    if (messageContent == null) return null;
                    return MessageTools.GetMessageContentMark(messageContent);
                });
            }
        }

        public static IValueConverter ChatLastMessageToContentMarkConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    ChatViewModel chat = value as ChatViewModel;
                    MessageModel message = chat.LastMessage;
                    if (chat == null || message == null) return null;
                    if (message.State == MessageStateEnum.Revoked)
                        return string.Format("\"{0}\"撤回了一条消息", chat.Chat.ChatAllUsers.FirstOrDefault(u => u.ImUserId.Equals(message.FromUser))?.Name);
                    return string.Format("{0}:{1}", chat.Chat.ChatAllUsers.FirstOrDefault(u => u.ImUserId.Equals(message.FromUser))?.Name, MessageTools.GetMessageContentMark(message));
                });
            }
        }

        public static IMultiValueConverter MessageToHorizontalAlignment
        {
            get
            {
                return new DelegateMultiValueConverter((values, targetType, parameter, cultInfo) =>
                {
                    MessageModel message = values[0] as MessageModel;
                    StaffModel staff = values[1] as StaffModel;
                    if (message.IsSendFrom(staff)) return HorizontalAlignment.Right;
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
                    MessageModel message = values[0] as MessageModel;
                    ChatModel chat = values[1] as ChatModel;
                    if (message == null || chat == null) return null;
                    return chat.ChatAllUsers.FirstOrDefault(u => u.ImUserId.Equals(message.FromUser))?.Name;
                });
            }
        }

        #region 私有方法
        private static void ParseMessageContentToDocument(FlowDocumentExt document, ref Block block, MessageContentModel messageContent)
        {
            if (block == null) { block = new Paragraph(); document.Blocks.Add(block); }
            switch (messageContent.Type)
            {
                case MessageTypeConst.Text:
                    {
                        TextMessageContent textMessageContent = messageContent.Content as TextMessageContent;
                        string[] messageContentItems = Regex.Split(textMessageContent.Content, Environment.NewLine);
                        for (int i = 0; i < messageContentItems.Length; i++)
                        {
                            string messageContentItem = messageContentItems[i];
                            if (!string.IsNullOrEmpty(messageContentItem))
                            {
                                if (block is BlockUIContainer)
                                {
                                    block = new Paragraph(); document.Blocks.Add(block);
                                    (block as Paragraph).Inlines.Add(messageContentItem);
                                }
                                else if (block is Paragraph)
                                {
                                    (block as Paragraph).Inlines.Add(messageContentItem);
                                }
                            }
                            if (i < messageContentItems.Length - 1)
                            {
                                block = new Paragraph(); document.Blocks.Add(block);
                            }
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
                                DataContext = new ChatMessageContentControlViewModel(messageContent)
                            };
                            block = new Paragraph(); document.Blocks.Add(block);
                        }
                        else if (block is Paragraph)
                        {
                            (block as Paragraph).Inlines.Add(new ChatMessageContentControlView()
                            {
                                DataContext = new ChatMessageContentControlViewModel(messageContent)
                            });
                        }
                    }; break;
                case MessageTypeConst.Mixed:
                    {
                        foreach (MessageContentModel mixedItem in (messageContent.Content as MixedMessageContent).Items)
                            ParseMessageContentToDocument(document, ref block, mixedItem);
                    }; break;
                case MessageTypeConst.Tips:
                case MessageTypeConst.Event:
                    break;
            }
        }

        private static void ParseInlinesToMessageContent(Inline[] inlines, List<MessageContentModel> messageContents, ref MessageContentModel messageContent)
        {
            foreach (Inline inline in inlines)
            {
                if (inline is Run)
                {
                    Run run = inline as Run;
                    if (messageContent == null || messageContent.Type != MessageTypeConst.Text)
                    {
                        messageContent = MessageTools.CreateTextMessageContent(run.Text);
                        messageContents.Add(messageContent);
                    }
                    else
                    {
                        TextMessageContent textMessageContent = messageContent.Content as TextMessageContent;
                        textMessageContent.Content += run.Text;
                    }
                }
                else if (inline is InlineUIContainer)
                {
                    InlineUIContainer inlineUIContainer = inline as InlineUIContainer;
                    if (inlineUIContainer.Child is ChatMessageContentControlView)
                    {
                        ChatMessageContentControlView ChatMessageContentControlView = inlineUIContainer.Child as ChatMessageContentControlView;
                        ChatMessageContentControlViewModel ChatMessageContentControlViewModel = ChatMessageContentControlView.DataContext as ChatMessageContentControlViewModel;
                        if (ChatMessageContentControlViewModel == null) continue;
                        messageContent = ChatMessageContentControlViewModel.MessageContent;
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

        private static void ParseDocumentToMessageContent(FlowDocumentExt document, List<MessageContentModel> messageContents)
        {
            MessageContentModel messageContent = null;
            for (int i = 0; i < document.Blocks.Count; i++)
            {
                Block block = document.Blocks.ElementAt(i);
                if (block is BlockUIContainer)
                {
                    BlockUIContainer blockUIContainer = block as BlockUIContainer;
                    if (blockUIContainer.Child is ChatMessageContentControlView)
                    {
                        ChatMessageContentControlView ChatMessageContentControlView = blockUIContainer.Child as ChatMessageContentControlView;
                        ChatMessageContentControlViewModel ChatMessageContentControlViewModel = ChatMessageContentControlView.DataContext as ChatMessageContentControlViewModel;
                        messageContent = ChatMessageContentControlViewModel.MessageContent;
                        messageContents.Add(messageContent);
                    }
                }
                else if (block is Paragraph)
                {
                    Paragraph paragraph = block as Paragraph;
                    ParseInlinesToMessageContent(paragraph.Inlines.ToArray(), messageContents, ref messageContent);

                    if (i < document.Blocks.Count - 1)
                    {
                        if (messageContent == null || messageContent.Type != MessageTypeConst.Text)
                        {
                            messageContent = MessageTools.CreateTextMessageContent(Environment.NewLine);
                            messageContents.Add(messageContent);
                        }
                        else
                        {
                            TextMessageContent textMessageContent = messageContent.Content as TextMessageContent;
                            textMessageContent.Content += Environment.NewLine;
                        }
                    }
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
                    MessageContentModel messageContent = value as MessageContentModel;
                    if (messageContent != null)
                    {
                        Block block = null;
                        ParseMessageContentToDocument(flowDocument, ref block, messageContent);
                    }
                    return flowDocument;
                }, (value, targetType, parameter, cultInfo) =>
                {
                    FlowDocumentExt document = value as FlowDocumentExt;
                    if (document == null) return null;
                    List<MessageContentModel> messageContents = new List<MessageContentModel>();
                    ParseDocumentToMessageContent(document, messageContents);
                    if (messageContents.Count > 1)
                    {
                        return MessageTools.CreateMixedMessageContent(messageContents.ToArray());
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
                    MessageModel message = value as MessageModel;
                    if (message != null && message.Type == MessageTypeConst.Tips)
                    {
                        TipsMessageContent tipsMessageContent = message.Content as TipsMessageContent;
                        return tipsMessageContent.Content;
                    }
                    return null;
                });
            }
        }

        public static IMultiValueConverter RevokeMessageToStringConverter
        {
            get
            {
                return new DelegateMultiValueConverter((values, targetType, parameter, cultInfo) =>
                {
                    MessageModel message = values[0] as MessageModel;
                    ChatModel chat = values[1] as ChatModel;
                    if (message == null || chat == null) return null;
                    return string.Format("\"{0}\"撤回了一条消息", chat.ChatAllUsers.FirstOrDefault(u => u.ImUserId.Equals(message.FromUser))?.Name);
                });
            }
        }

        public static IMultiValueConverter ChatFilterConverter
        {
            get
            {
                return new DelegateMultiValueConverter((values, targetType, parameter, cultInfo) =>
                {
                    ICollectionView collectionView = values[0] as ICollectionView;
                    bool onlyShowUnread = (bool)values[1];
                    if (collectionView == null) return null;
                    if (onlyShowUnread)
                    {
                        collectionView.Filter = (item) =>
                        {
                            ChatViewModel chat = item as ChatViewModel;
                            return chat.NotReadMessagesCount > 0;
                        };
                    }
                    return collectionView;
                });
            }
        }

        public static IMultiValueConverter QuickReplyGroupFilterConverter
        {
            get
            {
                return new DelegateMultiValueConverter((values, targetType, parameter, cultInfo) =>
                {
                    ObservableCollection<QuickReplyGroupModel> quickReplyGroups = values[0] as ObservableCollection<QuickReplyGroupModel>;
                    BusinessEnum business = (BusinessEnum)values[1];
                    ICollectionView collectionView = CollectionViewSource.GetDefaultView(quickReplyGroups);
                    if (collectionView == null) return null;
                    collectionView.Filter = (item) =>
                    {
                        QuickReplyGroupModel quickReplyGroup = item as QuickReplyGroupModel;
                        return quickReplyGroup.Business == business;
                    };
                    return collectionView;
                });
            }
        }

        public static IMultiValueConverter QuickReplyFilterConverter
        {
            get
            {
                return new DelegateMultiValueConverter((values, targetType, parameter, cultInfo) =>
                {
                    ObservableCollection<QuickReplyModel> quickReplyInfos = values[0] as ObservableCollection<QuickReplyModel>;
                    string condition = values[1] as string;
                    ICollectionView collectionView = CollectionViewSource.GetDefaultView(quickReplyInfos);
                    if (collectionView == null) return null;
                    collectionView.Filter = (item) =>
                    {
                        QuickReplyModel quickReply = item as QuickReplyModel;
                        string name = quickReply.Name;
                        string content = MessageTools.GetMessageContentMark(quickReply.Content);

                        string contentPinYin = ChineseCharactorTools.ToPinyin(content, true);
                        string contentFullPinYin = ChineseCharactorTools.ToPinyin(content);

                        return name.ToLower().Contains(condition.ToLower()) ||
                        ChineseCharactorTools.ToPinyin(name, true).ToLower().Contains(condition.ToLower()) ||
                        ChineseCharactorTools.ToPinyin(name).ToLower().Contains(condition.ToLower()) ||
                        content.ToLower().Contains(condition.ToLower()) ||
                        ChineseCharactorTools.ToPinyin(content, true).ToLower().Contains(condition.ToLower()) ||
                        ChineseCharactorTools.ToPinyin(content).ToLower().Contains(condition.ToLower());
                    };
                    return collectionView;
                });
            }
        }

        public static IValueConverter StaffToImageConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    StaffModel staff = value as StaffModel;
                    string content = string.Format("{0} {1}{2}", AppTools.AppName, staff?.WorkCode, staff?.Name);
                    Image image = DrawTools.CreateWaterMark(content);

                    BitmapImage bitmapImage = new BitmapImage { CacheOption = BitmapCacheOption.None, CreateOptions = BitmapCreateOptions.DelayCreation | BitmapCreateOptions.IgnoreImageCache };
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = image.ImageToStream();
                    bitmapImage.EndInit();
                    return bitmapImage;
                });
            }
        }

        public static IValueConverter BussinessOutsideFilterConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    ObservableCollection<BusinessEnum> businesses = value as ObservableCollection<BusinessEnum>;
                    if (businesses == null) return null;
                    ICollectionView collectionView = CollectionViewSource.GetDefaultView(businesses);
                    if (collectionView == null) return null;
                    collectionView.Filter = (item) =>
                    {
                        BusinessEnum business = (BusinessEnum)item;
                        FieldInfo finfo = business.GetType().GetField(business.ToString());
                        OutSideBussinessAttribute outSide = finfo.GetCustomAttribute(typeof(OutSideBussinessAttribute)) as OutSideBussinessAttribute;
                        return outSide != null;
                    };
                    return collectionView;
                });
            }
        }
    }
}
