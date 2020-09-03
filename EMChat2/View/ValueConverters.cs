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
                    return chat.ChatUsers.FirstOrDefault(u => u.ImUserId.Equals(message.FromUser)).Name;
                });
            }
        }

        private static Inline[] ParseMesageContentToInlines(string messageType, string messageContent)
        {
            switch (messageType)
            {
                case MessageTypeConst.Text:
                    return new Inline[] { new Run((MessageTools.ParseMessageContent(messageType, messageContent) as TextMessageContent).Content) };
                case MessageTypeConst.Emotion:
                case MessageTypeConst.Image:
                case MessageTypeConst.Voice:
                case MessageTypeConst.Video:
                case MessageTypeConst.Link:
                case MessageTypeConst.File:
                    return new Inline[] { new InlineUIContainer(
                        new ChatMessageContentControlView()
                        {
                            DataContext = new ChatMessageContentControlViewModel(messageType, MessageTools.ParseMessageContent(messageType, messageContent))
                        })
                    };
                case MessageTypeConst.Mixed:
                    {
                        List<Inline> inlines = new List<Inline>();
                        foreach (MessageContentItem mixedItem in (MessageTools.ParseMessageContent(messageType, messageContent) as MixedMessageContent).Items)
                            inlines.AddRange(ParseMesageContentToInlines(mixedItem.Type, mixedItem.Content));
                        return inlines.ToArray();
                    }
                default: return new Inline[] { };
            }
        }

        public static IValueConverter MessageToDocumentConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    MessageInfo message = value as MessageInfo;
                    FlowDocumentExt flowDocument = new FlowDocumentExt();
                    Paragraph paragraph = new Paragraph();
                    paragraph.Inlines.AddRange(ParseMesageContentToInlines(message.Type, message.Content));
                    flowDocument.Blocks.Add(paragraph);
                    return flowDocument;
                });
            }
        }
    }
}
