using EMChat2.Model.BaseInfo;
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
    /// QuickReplyAreaView.xaml 的交互逻辑
    /// </summary>
    public partial class QuickReplyAreaView : UserControl
    {
        public static readonly DependencyProperty BusinessProperty = DependencyProperty.Register("Business", typeof(BusinessEnum?), typeof(QuickReplyAreaView));

        public BusinessEnum? Business
        {
            get
            {
                return this.GetValue(BusinessProperty) as BusinessEnum?;
            }
            set
            {
                this.SetValue(BusinessProperty, value);
            }
        }

        public QuickReplyAreaView()
        {
            InitializeComponent();
        }
    }
}
