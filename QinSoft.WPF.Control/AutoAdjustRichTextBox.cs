using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Navigation;

namespace QinSoft.WPF.Control
{
    public class AutoAdjustRichTextBox : RichTextBox
    {
        private double defaultWidth = 1024;//默认宽度
        private bool informFromSource = false;
        private bool informFromTarget = false;
        #region 构造函数
        public AutoAdjustRichTextBox()
        {
            this.TextChanged += AutoRichTextBox_TextChanged;
        }
        #endregion

        #region 属性
        public static readonly DependencyProperty BindingDocumentProperty = DependencyProperty.Register("BindingDocument", typeof(FlowDocumentExt), typeof(AutoAdjustRichTextBox), new PropertyMetadata(null, OnBindingDocumentPropertyChanged));

        public static readonly DependencyProperty IsAutoProperty = DependencyProperty.Register("IsAuto", typeof(bool), typeof(AutoAdjustRichTextBox), new PropertyMetadata(true, OnIsAutoPropertyChanged));

        private static void OnBindingDocumentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoAdjustRichTextBox autoRichTextBox = d as AutoAdjustRichTextBox;
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
            autoRichTextBox.Document = (e.NewValue as FlowDocumentExt) ?? new FlowDocumentExt();
            autoRichTextBox.CaretPosition = autoRichTextBox.Document.ContentEnd;
        }

        private static void OnIsAutoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public FlowDocumentExt BindingDocument
        {
            get
            {
                return GetValue(BindingDocumentProperty) as FlowDocumentExt;
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
                this.Width = Math.Min(GetDocumentWidths().Max() + 10, this.MaxWidth);

            if (informFromSource)
            {
                informFromSource = false;
                return;
            }
            informFromTarget = true;
            informFromSource = true;
            this.BindingDocument = this.Document as FlowDocumentExt;
        }

        protected virtual double[] GetDocumentWidths()
        {
            if (this.Document == null) return null;
            List<double> widths = new List<double>() { 0 };
            foreach (Block block in this.Document.Blocks)
            {
                if (block is BlockUIContainer)
                {
                    double width = this.defaultWidth;

                    width = widths[widths.Count - 1] + width;
                    widths[widths.Count - 1] = width;
                    if (width >= this.MaxWidth) break;
                }
                else if (block is Paragraph)
                {
                    Paragraph paragraph = block as Paragraph;
                    GetInlineWidth(paragraph.Inlines.ToArray(), ref widths);
                }
                widths.Add(0);
            }
            return widths.ToArray();
        }

        protected virtual double[] GetInlineWidth(Inline[] inlines, ref List<double> widths)
        {
            foreach (Inline inline in inlines)
            {
                if (inline is Span)
                {
                    widths.AddRange(GetInlineWidth((inline as Span).Inlines.ToArray(), ref widths));
                }
                else if (inline is Run)
                {
                    Run run = inline as Run;
                    string text = run.Text;
                    string[] texts = text.Split(Environment.NewLine.ToCharArray());
                    for (int textIndex = 0; textIndex < texts.Length; textIndex++)
                    {
                        string itemText = texts[textIndex];
                        SizeF size = GetStringSize(itemText, new Font(run.FontFamily.ToString(), (float)run.FontSize, GraphicsUnit.Pixel));
                        double width = size.Width;

                        width = widths[widths.Count - 1] + width;
                        widths[widths.Count - 1] = width;
                        if (width >= this.MaxWidth) break;
                        if (textIndex < texts.Length - 1) widths.Add(0);
                    }
                }
                else if (inline is InlineUIContainer)
                {
                    InlineUIContainer inlineUIContainer = inline as InlineUIContainer;
                    if (inlineUIContainer.Child is FrameworkElement)
                    {
                        FrameworkElement frameworkElement = inlineUIContainer.Child as FrameworkElement;
                        double width = frameworkElement.Width;

                        if (double.IsNaN(width) || double.IsInfinity(width) || width == 0) width = frameworkElement.MaxWidth;

                        if (double.IsNaN(width) || double.IsInfinity(width) || width == 0) width = defaultWidth;

                        if (widths[widths.Count - 1] + width >= this.MaxWidth) widths.Add(0);

                        width = widths[widths.Count - 1] + width;
                        widths[widths.Count - 1] = width;
                        if (width >= this.MaxWidth) break;
                    }
                }
            }
            return widths.ToArray();
        }

        protected virtual SizeF GetStringSize(string content, Font font)
        {
            using (Graphics graphics = Graphics.FromHwnd(new WindowInteropHelper(Application.Current.MainWindow).Handle))
            {
                return graphics.MeasureString(content, font);
            }
        }
        #endregion
    }

    public class FlowDocumentExt : FlowDocument
    {
        protected override bool IsEnabledCore => true;
    }
}
