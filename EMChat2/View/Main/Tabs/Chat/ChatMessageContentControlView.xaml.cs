using EMChat2.Model.Entity;
using EMChat2.ViewModel.Main.Tabs.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EMChat2.View.Main.Tabs.Chat
{
    /// <summary>
    /// ChatMessageContentControlView.xaml 的交互逻辑
    /// </summary>
    public partial class ChatMessageContentControlView : UserControl
    {
        public ChatMessageContentControlView()
        {
            InitializeComponent();

            this.DataContextChanged += ChatMessageContentControlView_DataContextChanged;
        }

        private void ChatMessageContentControlView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ChatMessageContentControlViewModel)
            {
                ChatMessageContentControlViewModel chatMessageContentControlViewModel = e.NewValue as ChatMessageContentControlViewModel;
                switch (chatMessageContentControlViewModel.MsgType)
                {
                    case MessageTypeConst.Text: { } break;
                    case MessageTypeConst.Emotion: { this.MaxWidth = 64; } break;
                    case MessageTypeConst.Image: { this.MaxWidth = 256; } break;
                    case MessageTypeConst.Voice: { } break;
                    case MessageTypeConst.Video: { } break;
                    case MessageTypeConst.Link: { this.MaxWidth = 256; MaxHeight = 48; } break;
                    case MessageTypeConst.File: { this.MaxWidth = 200; MaxHeight = 48; } break;
                    case MessageTypeConst.Tips: { } break;
                    case MessageTypeConst.Event: { } break;
                }
            }
        }
    }
}
