using EMChat2.Common;
using EMChat2.Model.Entity;
using QinSoft.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EMChat2.View
{
    public static class ValueConverters
    {

        public static IValueConverter MessageToDisplayerConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    RichTextBox richTextBox = new RichTextBox();
                    FlowDocument document = new FlowDocument();
                    document.LineHeight = 1;
                    Paragraph paragraph = new Paragraph();
                    Run run = new Run(@"很");
                    run.Foreground = new SolidColorBrush(Colors.Red);
                    paragraph.Inlines.Add(run);
                    paragraph.Inlines.Add("\r\nq");
                    paragraph.Inlines.Add(new Image() { Source = new BitmapImage(new Uri("https://dss0.bdstatic.com/70cFuHSh_Q1YnxGkpoWK1HF6hhy/it/u=1906469856,4113625838&fm=26&gp=0.jpg")), Height = 64 });
                    document.Blocks.Add(paragraph);
                    richTextBox.Document = document;
                    return richTextBox;
                }, (value, targetType, parameter, cultInfo) =>
                {
                    return new MessageInfo();
                });
            }
        }

        public static IValueConverter MessageToContentMarkConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    MessageInfo message = value as MessageInfo;
                    if (message == null) return null;
                    else return MessageTools.GetMessageContentMark(message.Content, message.Type);
                });
            }
        }
    }
}
