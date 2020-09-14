﻿using EMChat2.Common;
using EMChat2.Model.Entity;
using EMChat2.Model.Event;
using EMChat2.Service;
using QinSoft.Event;
using QinSoft.Log.Utils;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace EMChat2.ViewModel.Main.Tabs.Chat
{
    public abstract class ChatTabItemAreaViewModel : PropertyChangedBase, IDisposable
    {
        #region 构造函数
        public ChatTabItemAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, EmotionPickerAreaViewModel emotionPickerAreaViewModel, ChatInfo chat, ChatService chatService, SystemService systemService)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.emotionPickerAreaViewModel = emotionPickerAreaViewModel;
            this.chat = chat;
            this.chatService = chatService;
            this.systemService = systemService;
            this.Messages = new ThreadSafeObservableCollection<MessageInfo>();
        }
        #endregion

        #region 属性
        protected IWindowManager windowManager;
        protected EventAggregator eventAggregator;
        private ChatService chatService;
        private SystemService systemService;
        private ChatInfo chat;
        public ChatInfo Chat
        {
            get
            {
                return this.chat;
            }
            set
            {
                this.chat = value;
                this.NotifyPropertyChange(() => this.Chat);
            }
        }
        private ApplicationContextViewModel applicationContextViewModel;
        public ApplicationContextViewModel ApplicationContextViewModel
        {
            get
            {
                return this.applicationContextViewModel;
            }
            set
            {
                this.applicationContextViewModel = value;
                this.NotifyPropertyChange(() => this.ApplicationContextViewModel);
            }
        }
        private EmotionPickerAreaViewModel emotionPickerAreaViewModel;
        public EmotionPickerAreaViewModel EmotionPickerAreaViewModel
        {
            get
            {
                return this.emotionPickerAreaViewModel;
            }
            set
            {
                this.emotionPickerAreaViewModel = value;
                this.NotifyPropertyChange(() => this.EmotionPickerAreaViewModel);
            }
        }
        private ThreadSafeObservableCollection<MessageInfo> messages;
        public ThreadSafeObservableCollection<MessageInfo> Messages
        {
            get
            {
                return this.messages;
            }
            set
            {
                this.messages = value;
                this.NotifyPropertyChange(() => this.Messages);
                this.NotifyPropertyChange(() => this.NotReadMessageCount);
                this.NotifyPropertyChange(() => this.LastMessage);
                this.eventAggregator.PublishAsync(new NotReadMessageCountChangedEventArgs());
                this.messages.CollectionChanged += (s, e) =>
                {
                    this.NotifyPropertyChange(() => this.NotReadMessageCount);
                    this.NotifyPropertyChange(() => this.LastMessage);
                    this.eventAggregator.PublishAsync(new NotReadMessageCountChangedEventArgs());
                };
            }
        }
        private MessageContentInfo inputMessageContent;
        public MessageContentInfo InputMessageContent
        {
            get
            {
                return this.inputMessageContent;
            }
            set
            {
                this.inputMessageContent = value;
                this.NotifyPropertyChange(() => this.InputMessageContent);
            }
        }
        private MessageContentInfo temporaryInputMessagContent;
        public MessageContentInfo TemporaryInputMessagContent
        {
            get
            {
                MessageContentInfo messageContent = this.temporaryInputMessagContent;
                //用完就会被清除
                this.temporaryInputMessagContent = null;
                return messageContent;
            }
            set
            {
                this.temporaryInputMessagContent = value;
                this.NotifyPropertyChange(() => this.TemporaryInputMessagContent);
            }
        }
        public int NotReadMessageCount
        {
            get
            {
                lock (this.messages)
                {
                    return this.messages.Count(u => u.State.Equals(MessageState.Received));
                }
            }
        }
        public MessageInfo LastMessage
        {
            get
            {
                lock (this.messages)
                {
                    return this.messages.LastOrDefault();
                }
            }
        }
        #endregion

        #region 命令
        public ICommand ToggleChatIsTopCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.Chat.IsTop = !this.Chat.IsTop;
                });
            }
        }

        public ICommand ToggleChatIsInformCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.Chat.IsInform = !this.Chat.IsInform;
                });
            }
        }

        public ICommand ScreenShotCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                });
            }
        }

        public ICommand SelectImageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    FileDialog fileDialog = new OpenFileDialog();
                    fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                    fileDialog.Filter = "图片|*.jpg;*.jpeg;*.png;*.gif";

                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        this.eventAggregator.PublishAsync(new SelectImageEventArgs() { File = new FileInfo(fileDialog.FileName) });
                    }
                });
            }
        }

        public ICommand SelectFileCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    FileDialog fileDialog = new OpenFileDialog();
                    fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                    fileDialog.Filter = "文件|*.*";

                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        this.eventAggregator.PublishAsync(new SelectFileEventArgs() { File = new FileInfo(fileDialog.FileName) });
                    }
                });
            }
        }

        public ICommand SendMessageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

                    MessageInfo message = MessageTools.CreateMessage(applicationContextViewModel.CurrentStaff, this.Chat, this.InputMessageContent);
                    if (message == null) return;
                    new Action(() =>
                    {
                        this.InputMessageContent = null;
                        lock (this.Messages)
                        {
                            this.Messages.Add(message);
                        }
                    }).ExecuteInUIThread();
                });
            }
        }

        public ICommand ResendMessageCommand
        {
            get
            {
                return new RelayCommand<MessageInfo>((oldMessage) =>
                {
                    MessageInfo message = MessageTools.CreateMessage(applicationContextViewModel.CurrentStaff, this.Chat, oldMessage);
                    if (message == null) return;
                    new Action(() =>
                    {
                        lock (this.Messages)
                        {
                            this.Messages.Remove(oldMessage);
                            this.Messages.Add(message);
                        }
                    }).ExecuteInUIThread();
                });
            }
        }
        #endregion

        #region 方法
        public virtual void Dispose()
        {
            this.eventAggregator.Unsubscribe(this);
        }
        #endregion
    }
}
