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
        }

        private void messageList_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}
