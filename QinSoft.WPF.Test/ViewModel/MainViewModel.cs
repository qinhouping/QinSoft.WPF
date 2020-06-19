using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QinSoft.WPF.Core;
using QinSoft.Ioc.Attribute;
using QinSoft.Event;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace QinSoft.WPF.Test.ViewModel
{
    [Component]
    public class MainViewModel : PropertyChangedBase
    {
        private string title;
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
                this.NotifyPropertyChange(() => this.Title);
            }
        }
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private TestViewModel testViewModel;
        private ObservableCollection<Message> messages;
        public ObservableCollection<Message> Messages
        {
            get
            {
                return this.messages;
            }
            set
            {
                this.messages = value;
                this.NotifyPropertyChange(() => this.Messages);
            }
        }

        public MainViewModel(IWindowManager windowManager, TestViewModel testViewModel, EventAggregator eventAggregator)
        {
            this.Title = "QinSoft.WPF.Core.Test";
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.testViewModel = testViewModel;

            this.messages = new ObservableCollection<Message> {
               new Message() { Left = false, Sender = "test", SendTime = DateTime.Now,  Type=MessageType.Mix, InnerMessages=new ObservableCollection<Message>(){
               new Message() { Left = true, Sender = "test", SendTime = DateTime.Now, Type=MessageType.Text, Text = "inner message"  },
               new Message() { Left = false, Sender = "test", SendTime = DateTime.Now,  Type=MessageType.Image, ThumbUrl="https://ss3.bdstatic.com/70cFv8Sh_Q1YnxGkpoWK1HF6hhy/it/u=2534506313,1688529724&fm=26&gp=0.jpg", Url="https://ss3.bdstatic.com/70cFv8Sh_Q1YnxGkpoWK1HF6hhy/it/u=2534506313,1688529724&fm=26&gp=0.jpg"  },
                          new Message() { Left = true, Sender = "test", SendTime = DateTime.Now, Type=MessageType.Text, Text = "inner message"  }
               }  }
            };
        }

        public ICommand TestClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    windowManager.ShowWindow(this.testViewModel);
                });
            }
        }

        public ICommand TestEventCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.eventAggregator.PublishAsync<string>(DateTime.Now.ToString());
                });
            }
        }

        public ICommand ShowImageCommand
        {
            get
            {
                return new RelayCommand<Message>((arg) =>
                {
                    windowManager.ShowDialog(new ImageViewModel(arg.Url));
                });
            }
        }



        public class Message : PropertyChangedBase
        {

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
                    this.NotifyPropertyChange(() => this.Sender);
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
                    this.NotifyPropertyChange(() => this.SendTime);
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
                    this.NotifyPropertyChange(() => this.Left);
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
                    this.NotifyPropertyChange(() => this.Type);
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
                    this.NotifyPropertyChange(() => this.Text);
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
                    this.NotifyPropertyChange(() => this.ThumbUrl);
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
                    this.NotifyPropertyChange(() => this.Url);
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
                    this.NotifyPropertyChange(() => this.InnerMessages);
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
    }
}
