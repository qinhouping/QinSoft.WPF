using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Navigation;

namespace QinSoft.WPF.Control
{
    public class AutoRichTextBox : RichTextBox
    {
        private bool informFromSource = false;
        private bool informFromTarget = false;
        #region 构造函数
        public AutoRichTextBox()
        {
            this.TextChanged += AutoRichTextBox_TextChanged;
            DependencyPropertyDescriptor.FromProperty(MaxWidthProperty, typeof(AutoRichTextBox)).AddValueChanged(this, this.AutoRichTextBox_MaxWidthPropertyChanged);
        }

        #endregion

        #region 属性
        public static readonly DependencyProperty BindingDocumentProperty = DependencyProperty.Register("BindingDocument", typeof(FlowDocument), typeof(AutoRichTextBox), new PropertyMetadata(null, OnBindingDocumentPropertyChanged));

        public static readonly DependencyProperty IsAutoProperty = DependencyProperty.Register("IsAuto", typeof(bool), typeof(AutoRichTextBox), new PropertyMetadata(true, OnIsAutoPropertyChanged));

        private static void OnBindingDocumentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoRichTextBox autoRichTextBox = d as AutoRichTextBox;
            if (autoRichTextBox.informFromTarget)
            {
                autoRichTextBox.informFromTarget = false;
                return;
            }
            if (autoRichTextBox.informFromSource)
            {
                autoRichTextBox.informFromSource = false;
                return;
            }
            autoRichTextBox.informFromSource = true;
            autoRichTextBox.Document = (e.NewValue as FlowDocument) ?? new FlowDocument();
        }

        private static void OnIsAutoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public FlowDocument BindingDocument
        {
            get
            {
                return GetValue(BindingDocumentProperty) as FlowDocument;
            }
            set
            {
                this.SetValue(BindingDocumentProperty, value);
            }
        }

        public bool IsAuto
        {
            get
            {
                return (bool)GetValue(IsAutoProperty);
            }
            set
            {
                this.SetValue(IsAutoProperty, value);
            }
        }
        #endregion

        #region 方法
        private void AutoRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsAuto)
                this.Width = Math.Min(GetDocumentWidths().Max() + 20, this.MaxWidth);
            if (informFromSource)
            {
                informFromSource = false;
                return;
            }
            informFromTarget = true;
            informFromSource = true;
            this.BindingDocument = this.Document;
        }

        private void AutoRichTextBox_MaxWidthPropertyChanged(object sender, EventArgs e)
        {
            if (IsAuto) this.Width = Math.Min(GetDocumentWidths().Max(), this.MaxWidth);
        }

        protected virtual double[] GetDocumentWidths()
        {
            if (this.Document == null) return null;
            List<double> widths = new List<double>() { 0 };
            foreach (Block block in this.Document.Blocks)
            {
                if (block is BlockUIContainer)
                {
                    widths.Add(double.MaxValue);
                }
                else if (block is Paragraph)
                {
                    Paragraph paragraph = block as Paragraph;
                    widths.AddRange(GetInlineWidth(paragraph.Inlines.ToArray()));
                }
            }
            return widths.ToArray();
        }

        protected virtual double[] GetInlineWidth(Inline[] inlines)
        {
            List<double> widths = new List<double>();
            foreach (Inline inline in inlines)
            {
                if (inline is Span)
                {
                    widths.AddRange(GetInlineWidth((inline as Span).Inlines.ToArray()));
                }
                else if (inline is Run)
                {
                    Run run = inline as Run;
                    double fontSize = inline.FontSize;
                    string text = run.Text;
                    foreach (string itemText in text.Split(Environment.NewLine.ToCharArray()))
                    {
                        double width = 0;
                        for (int i = 0; i < itemText.Length; i++)
                        {
                            if ((int)itemText[i] > 127)
                                width += fontSize;
                            else
                                width += (fontSize / 2);
                        }
                        widths.Add(width);
                        if (width >= this.MaxWidth) break;
                    }
                }
                else if (inline is InlineUIContainer)
                {
                    InlineUIContainer inlineUIContainer = inline as InlineUIContainer;
                    if (inlineUIContainer.Child is FrameworkElement)
                    {
                        FrameworkElement frameworkElement = inlineUIContainer.Child as FrameworkElement;
                        double width = frameworkElement.Width;
                        if (double.IsNaN(width) || width == 0) width = frameworkElement.MaxWidth;
                        if (double.IsInfinity(width)) width = double.MaxValue;
                        widths.Add(width);
                        if (width >= this.MaxWidth) break;
                    }
                }
            }
            return widths.ToArray();
        }
        #endregion
    }

    public class FlowDocumentExt : FlowDocument
    {
        protected override bool IsEnabledCore => true;
    }
}
