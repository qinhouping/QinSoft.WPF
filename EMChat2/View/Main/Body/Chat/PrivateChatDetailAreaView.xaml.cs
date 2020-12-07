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

namespace EMChat2.View.Main.Body.Chat
{
    /// <summary>
    /// PrivateChatDetailAreaView.xaml 的交互逻辑
    /// </summary>
    public partial class PrivateChatDetailAreaView : UserControl
    {
        public PrivateChatDetailAreaView()
        {
            InitializeComponent();
        }

        private void ChatInputAreaView_InputStateChanged(object sender, InputStateChangedRoutedEventArgs e)
        {
            (this.DataContext as PrivateChatViewModel).SendInputState(e.IsInputing);
        }
    }
}
