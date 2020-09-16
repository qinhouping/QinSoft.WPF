using EMChat2.Model.Entity;
using EMChat2.View.Main.Tabs.Chat;
using EMChat2.ViewModel.Main.Tabs.Chat;
using QinSoft.WPF.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace EMChat2.Common
{
    public class MessageRichTextBoxAttach
    {
        public static readonly DependencyProperty InputMessageContentProperty =
             DependencyProperty.RegisterAttached("InputMessageContent", typeof(MessageContentInfo), typeof(MessageRichTextBoxAttach), new PropertyMetadata(OnMessageContentInfoPropertyChangedCallback));
        public static void SetInputMessageContent(DependencyObject dp, MessageContentInfo value)
        {
            dp.SetValue(InputMessageContentProperty, value);
        }
        public static MessageContentInfo GetInputMessageContent(DependencyObject dp)
        {
            return dp.GetValue(InputMessageContentProperty) as MessageContentInfo;
        }

        private static void AppendMessageContent(MessageContentInfo messageContent, AutoAdjustRichTextBox autoAdjustRichTextBox)
        {
            autoAdjustRichTextBox.CaretPosition = autoAdjustRichTextBox.Document.ContentEnd; //追加在文档最后
            switch (messageContent.Type)
            {
                case MessageTypeConst.Text:
                    {
                        TextMessageContent textMessageContent = MessageTools.ParseMessageContent(messageContent) as TextMessageContent;
                        Run run = new Run(textMessageContent.Content, autoAdjustRichTextBox.CaretPosition);
                    }; break;
                case MessageTypeConst.Emotion:
                case MessageTypeConst.Image:
                case MessageTypeConst.Voice:
                case MessageTypeConst.Video:
                case MessageTypeConst.Link:
                case MessageTypeConst.File:
                    {
                        object tmpMessageContent = MessageTools.ParseMessageContent(messageContent);
                        ChatMessageContentControlView chatMessageContentControlView = new ChatMessageContentControlView()
                        {
                            DataContext = new ChatMessageContentControlViewModel(messageContent.Type, tmpMessageContent)
                        };
                        new InlineUIContainer(chatMessageContentControlView, autoAdjustRichTextBox.CaretPosition);
                    }
                    break;
                case MessageTypeConst.Mixed:
                    {
                        MixedMessageContent mixedMessageContent = MessageTools.ParseMessageContent(messageContent) as MixedMessageContent;
                        foreach (MessageContentInfo tmpMessageContent in mixedMessageContent.Items)
                        {
                            AppendMessageContent(tmpMessageContent, autoAdjustRichTextBox);
                        }
                    }; break;
                case MessageTypeConst.Tips:
                case MessageTypeConst.Event:
                    break;
            }
        }

        private static void OnMessageContentInfoPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoAdjustRichTextBox autoAdjustRichTextBox = d as AutoAdjustRichTextBox;
            if (autoAdjustRichTextBox == null) return;
            MessageContentInfo messageContent = e.NewValue as MessageContentInfo;
            if (messageContent == null) return;
            AppendMessageContent(messageContent, autoAdjustRichTextBox);
        }
    }
}
