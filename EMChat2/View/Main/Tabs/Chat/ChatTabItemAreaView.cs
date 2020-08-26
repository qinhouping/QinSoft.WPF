using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace EMChat2.View.Main.Tabs.Chat
{
    public class ChatTabItemAreaView : TabItem
    {

        #region 属性
        public static DependencyProperty CreateTimeProperty = DependencyProperty.Register("CreateTime", typeof(DateTime), typeof(ChatTabItemAreaView));
        public DateTime? CreateTime
        {
            get
            {
                return (DateTime)this.GetValue(CreateTimeProperty);
            }
            set
            {
                this.SetValue(CreateTimeProperty, value);
            }
        }

        public static DependencyProperty LastMessageTimeProperty = DependencyProperty.Register("LastMessage", typeof(DateTime?), typeof(ChatTabItemAreaView));
        public DateTime? LastMessage
        {
            get
            {
                return (DateTime?)this.GetValue(LastMessageTimeProperty);
            }
            set
            {
                this.SetValue(LastMessageTimeProperty, value);
            }
        }

        public static DependencyProperty IsTopProperty = DependencyProperty.Register("IsTop", typeof(bool), typeof(ChatTabItemAreaView));
        public bool IsTop
        {
            get
            {
                return (bool)this.GetValue(IsTopProperty);
            }
            set
            {
                this.SetValue(IsTopProperty, value);
            }
        }
        #endregion
    }
}
