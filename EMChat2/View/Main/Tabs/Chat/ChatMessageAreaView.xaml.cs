using EMChat2.ViewModel.Main.Tabs.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// ChatMessageAreaView.xaml 的交互逻辑
    /// </summary>
    public partial class ChatMessageAreaView : UserControl
    {
        public ChatMessageAreaView()
        {
            InitializeComponent();

            this.DataContextChanged += ChatMessageAreaView_DataContextChanged;
        }

        private void ChatMessageAreaView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ChatTabItemAreaViewModel chatTabItemAreaViewModel = e.NewValue as ChatTabItemAreaViewModel;
            if (chatTabItemAreaViewModel == null) return;
            chatTabItemAreaViewModel.Messages.CollectionChanged += Messages_CollectionChanged;
        }

        private void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!this.messageList.IsFocused)
            {
                if (messageList.Items.Count == 0) return;
                messageList.ScrollIntoView(messageList.Items[messageList.Items.Count - 1]);
            }
        }
    }
}
