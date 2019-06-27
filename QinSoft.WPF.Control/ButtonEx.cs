using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace QinSoft.WPF.Control
{
    public class ButtonEx : Button
    {
        public static readonly DependencyProperty TypedProperty =
            DependencyProperty.Register("Type", typeof(string), typeof(ButtonEx), new PropertyMetadata("A", new PropertyChangedCallback((d, e) =>
            {

            })), new ValidateValueCallback((v) =>
            {
                return true;
            }));

        public string Type
        {
            get { return this.GetValue(TypedProperty) as string; }
            set { this.SetValue(TypedProperty, value); }
        }

        static ButtonEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonEx), new FrameworkPropertyMetadata(typeof(ButtonEx)));
        }
    }
}
