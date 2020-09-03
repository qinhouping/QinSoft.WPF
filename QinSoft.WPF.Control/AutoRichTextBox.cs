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

namespace QinSoft.WPF.Control
{
    public class AutoRichTextBox : RichTextBox
    {
        public AutoRichTextBox()
        {
            this.TextChanged += AutoRichTextBox_TextChanged;
            DependencyPropertyDescriptor.FromProperty(MaxWidthProperty, typeof(AutoRichTextBox)).AddValueChanged(this, this.AutoRichTextBox_MaxWidthPropertyChanged);
        }

        private void AutoRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Width = Math.Min(GetDocumentWidths().Max() + 20, this.MaxWidth);
        }

        private void AutoRichTextBox_MaxWidthPropertyChanged(object sender, EventArgs e)
        {
            this.Width = Math.Min(GetDocumentWidths().Max(), this.MaxWidth);
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
                    foreach (string itemText in text.Split('\r'))
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
                        widths.Add(frameworkElement.Width);
                        if (frameworkElement.Width >= this.MaxWidth) break;
                    }
                }
            }
            return widths.ToArray();
        }
    }
}
