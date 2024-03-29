﻿using System;
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
    /// ChatListAreaView.xaml 的交互逻辑
    /// </summary>
    public partial class ChatListAreaView : UserControl
    {
        public static readonly DependencyProperty OnlyShowUnreadProperty = DependencyProperty.Register("OnlyShowUnread", typeof(bool), typeof(QuickReplyAreaView), new PropertyMetadata(false));

        public bool OnlyShowUnread
        {
            get
            {
                return (bool)this.GetValue(OnlyShowUnreadProperty);
            }
            set
            {
                this.SetValue(OnlyShowUnreadProperty, value);
            }
        }
        public ChatListAreaView()
        {
            InitializeComponent();
        }
    }
}
