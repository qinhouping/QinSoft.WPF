﻿using EMChat2.Model.BaseInfo;
using EMChat2.Model.Enum;
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
        public static readonly DependencyProperty BusinessIdProperty = DependencyProperty.Register("BusinessId", typeof(string), typeof(QuickReplyAreaView));

        public string BusinessId
        {
            get
            {
                return (string)this.GetValue(BusinessIdProperty);
            }
            set
            {
                this.SetValue(BusinessIdProperty, value);
            }
        }

        public QuickReplyAreaView()
        {
            InitializeComponent();
        }
    }
}
