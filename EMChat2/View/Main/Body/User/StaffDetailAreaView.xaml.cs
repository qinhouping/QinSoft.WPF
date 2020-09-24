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

namespace EMChat2.View.Main.Body.User
{
    /// <summary>
    /// StaffDetailAreaView.xaml 的交互逻辑
    /// </summary>
    public partial class StaffDetailAreaView : UserControl
    {
        public StaffDetailAreaView()
        {
            InitializeComponent();
        }

        public static DependencyProperty AllowChatProperty = DependencyProperty.Register("AllowChat", typeof(bool), typeof(StaffDetailAreaView), new PropertyMetadata(true, (s, e) =>
        {
            StaffDetailAreaView staffDetailAreaView = s as StaffDetailAreaView;
            if (staffDetailAreaView == null) return;
            staffDetailAreaView.allowChatBtn.Visibility = staffDetailAreaView.AllowChat ? Visibility.Visible : Visibility.Collapsed;
        }));

        public bool AllowChat
        {
            get
            {
                return (bool)this.GetValue(AllowChatProperty);
            }
            set
            {
                this.SetValue(AllowChatProperty, value);
            }
        }
    }
}
