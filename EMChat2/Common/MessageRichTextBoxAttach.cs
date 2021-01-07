using EMChat2.Model.BaseInfo;
using EMChat2.Model.Enum;
using EMChat2.View.Main.Body.Chat;
using EMChat2.ViewModel.Main.Tabs.Chat;
using QinSoft.WPF.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace EMChat2.Common
{
    public static class MessageRichTextBoxAttach
    {
        public static readonly DependencyProperty InputMessageContentProperty =
             DependencyProperty.RegisterAttached("InputMessageContent", typeof(MessageContentModel), typeof(MessageRichTextBoxAttach), new PropertyMetadata(OnMessageContentInfoPropertyChangedCallback));
        public static void SetInputMessageContent(DependencyObject dp, MessageContentModel value)
        {
            dp.SetValue(InputMessageContentProperty, value);
        }
        public static MessageContentModel GetInputMessageContent(DependencyObject dp)
        {
            return dp.GetValue(InputMessageContentProperty) as MessageContentModel;
        }

        private static void AppendMessageContent(AutoAdjustRichTextBox autoAdjustRichTextBox, MessageContentModel messageContent)
        {
            switch (messageContent.Type)
            {
                case MessageTypeConst.Text:
                    {
                        TextMessageContent textMessageContent = messageContent.Content as TextMessageContent;
                        if (string.IsNullOrEmpty(textMessageContent.Content)) return;
                        Run run = new Run(textMessageContent.Content, autoAdjustRichTextBox.CaretPosition);
                        autoAdjustRichTextBox.CaretPosition = run.ElementEnd;
                        run.BringIntoView();
                    }; break;
                case MessageTypeConst.Emotion:
                case MessageTypeConst.Image:
                case MessageTypeConst.Voice:
                case MessageTypeConst.Video:
                case MessageTypeConst.Link:
                case MessageTypeConst.File:
                    {
                        ChatMessageContentControlView ChatMessageContentControlView = new ChatMessageContentControlView()
                        {
                            DataContext = new ChatMessageContentControlViewModel(messageContent)
                        };
                        InlineUIContainer inlineUIContainer = new InlineUIContainer(ChatMessageContentControlView, autoAdjustRichTextBox.CaretPosition);
                        autoAdjustRichTextBox.CaretPosition = inlineUIContainer.ElementEnd;
                        inlineUIContainer.BringIntoView();
                    }
                    break;
                case MessageTypeConst.Mixed:
                    {
                        MixedMessageContent mixedMessageContent = messageContent.Content as MixedMessageContent;
                        foreach (MessageContentModel item in mixedMessageContent.Items)
                        {
                            AppendMessageContent(autoAdjustRichTextBox, item);
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
            MessageContentModel messageContent = e.NewValue as MessageContentModel;
            if (messageContent == null) return;
            autoAdjustRichTextBox.Focus();
            AppendMessageContent(autoAdjustRichTextBox, messageContent);
        }
    }
}
