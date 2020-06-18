using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace QinSoft.WPF.Test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            mcob.ItemsSource = new string[] { "hello", "world" };
            mcob.SelectedItem = "hello";


            chat.ItemsSource = new ObservableCollection<Message> {
               new Message() { Left = false, Sender = "test", SendTime = DateTime.Now,  Type=MessageType.Mix, InnerMessages=new ObservableCollection<Message>(){
               new Message() { Left = true, Sender = "test", SendTime = DateTime.Now, Type=MessageType.Text, Text = "inner message"  },
               new Message() { Left = false, Sender = "test", SendTime = DateTime.Now,  Type=MessageType.Image, ThumbUrl="https://ss3.bdstatic.com/70cFv8Sh_Q1YnxGkpoWK1HF6hhy/it/u=2534506313,1688529724&fm=26&gp=0.jpg", Url="https://ss3.bdstatic.com/70cFv8Sh_Q1YnxGkpoWK1HF6hhy/it/u=2534506313,1688529724&fm=26&gp=0.jpg"  },
                          new Message() { Left = true, Sender = "test", SendTime = DateTime.Now, Type=MessageType.Text, Text = "inner message"  }
               }  }
            };

        }

        public class Message : INotifyPropertyChanged
        {

            #region INotifyPropertyChanged 成员

            public event PropertyChangedEventHandler PropertyChanged;

            #endregion

            private string sender;
            public string Sender
            {
                get
                {
                    return this.sender;
                }
                set
                {
                    this.sender = value;
                    if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Sender"));
                }
            }

            private DateTime sendTime;
            public DateTime SendTime
            {
                get
                {
                    return this.sendTime;
                }
                set
                {
                    this.sendTime = value;
                    if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs("SendTime"));
                }
            }

            private bool left;
            public bool Left
            {
                get
                {
                    return this.left;
                }
                set
                {
                    this.left = value;
                    if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Left"));
                }
            }

            private MessageType type;
            public MessageType Type
            {
                get
                {
                    return this.type;
                }
                set
                {
                    this.type = value;
                    if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Type"));
                }
            }

            private string text;
            public string Text
            {
                get
                {
                    return this.text;
                }
                set
                {
                    this.text = value;
                    if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Text"));
                }
            }

            private string thumbUrl;
            public string ThumbUrl
            {
                get
                {
                    return this.thumbUrl;
                }
                set
                {
                    this.thumbUrl = value;
                    if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ThumbUrl"));
                }
            }

            private string url;
            public string Url
            {
                get
                {
                    return this.url;
                }
                set
                {
                    this.url = value;
                    if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Url"));
                }
            }

            private ObservableCollection<Message> innerMessages;

            public ObservableCollection<Message> InnerMessages
            {
                get
                {
                    return this.innerMessages;
                }
                set
                {
                    this.innerMessages = value;
                    if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs("InnerMessages"));
                }
            }
        }

        public enum MessageType
        {
            Unknown,
            Text,
            Image,
            Mix
        }

        private void img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount > 1)
            {
                Window win = new ImageDialog((sender as Image).Source);
                win.Owner = this;
                win.ShowDialog();
            }
        }
    }
}
