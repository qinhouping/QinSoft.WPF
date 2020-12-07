﻿using QinSoft.WPF.Control;
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

namespace EMChat2.View.Main.Body.Chat
{
    /// <summary>
    /// ChatInputAreaView.xaml 的交互逻辑
    /// </summary>
    public partial class ChatInputAreaView : UserControl
    {
        public static readonly RoutedEvent InputStateChangedEvent = EventManager.RegisterRoutedEvent("InputStateChanged", RoutingStrategy.Bubble,
         typeof(InputStateChangedRoutedEventHandler), typeof(ChatInputAreaView));

        public event InputStateChangedRoutedEventHandler InputStateChanged
        {
            add
            {
                base.AddHandler(InputStateChangedEvent, value);
            }
            remove
            {
                base.RemoveHandler(InputStateChangedEvent, value);
            }
        }

        public ChatInputAreaView()
        {
            InitializeComponent();
        }

        private void AutoAdjustRichTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new InputStateChangedRoutedEventArgs() { RoutedEvent = InputStateChangedEvent, Source = this, IsInputing = true });
        }

        private void AutoAdjustRichTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new InputStateChangedRoutedEventArgs() { RoutedEvent = InputStateChangedEvent, Source = this, IsInputing = false });
        }
    }

    public delegate void InputStateChangedRoutedEventHandler(object sender, InputStateChangedRoutedEventArgs e);

    public class InputStateChangedRoutedEventArgs : RoutedEventArgs
    {
        public bool IsInputing { get; set; }
    }
}
