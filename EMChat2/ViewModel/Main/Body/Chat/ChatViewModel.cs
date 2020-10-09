using EMChat2.Common;
using EMChat2.Model.BaseInfo;
using EMChat2.Model.Event;
using EMChat2.Service;
using QinSoft.Event;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace EMChat2.ViewModel.Main.Tabs.Chat
{
    public abstract class ChatViewModel : PropertyChangedBase, IDisposable
    {
        #region 构造函数
        public ChatViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, EmotionPickerAreaViewModel emotionPickerAreaViewModel, ChatInfo chat, ChatService chatService, SystemService systemService)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.emotionPickerAreaViewModel = emotionPickerAreaViewModel;
            this.chat = chat;
            this.chatService = chatService;
            this.systemService = systemService;
            this.Messages = new ObservableCollection<MessageInfo>();
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
        private ObservableCollection<MessageInfo> messages;
        public ObservableCollection<MessageInfo> Messages
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
                this.NotifyPropertyChange(() => this.LastMessageTimeSort);
                this.eventAggregator.PublishAsync(new NotReadMessageCountChangedEventArgs());
                this.eventAggregator.PublishAsync(new RefreshChatsEventArgs());
                this.messages.CollectionChanged += (s, e) =>
                {
                    this.NotifyPropertyChange(() => this.NotReadMessageCount);
                    this.NotifyPropertyChange(() => this.LastMessage);
                    this.NotifyPropertyChange(() => this.LastMessageTimeSort);
                    this.eventAggregator.PublishAsync(new NotReadMessageCountChangedEventArgs());
                    this.eventAggregator.PublishAsync(new RefreshChatsEventArgs());
                };

                ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.messages);
                collectionView.SortDescriptions.Add(new SortDescription("MsgTime", ListSortDirection.Ascending));
                this.MessagesCollectionView = collectionView;
            }
        }
        private ICollectionView messagesCollectionView;
        public ICollectionView MessagesCollectionView
        {
            get
            {
                return this.messagesCollectionView;
            }
            set
            {
                this.messagesCollectionView = value;
                this.NotifyPropertyChange(() => this.MessagesCollectionView);
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
                    return this.messages.Count(u => u.State.Equals(MessageStateEnum.Received));
                }
            }
        }
        public MessageInfo LastMessage
        {
            get
            {
                lock (this.messages)
                {
                    this.messagesCollectionView.MoveCurrentToLast();
                    return this.messagesCollectionView.CurrentItem as MessageInfo;
                }
            }
        }
        private bool isDragMessageContent;
        public bool IsDragMessageContent
        {
            get
            {
                return this.isDragMessageContent;
            }
            set
            {
                this.isDragMessageContent = value;
                this.NotifyPropertyChange(() => this.IsDragMessageContent);
            }
        }
        #region 排序属性
        public bool IsTopSort
        {
            get
            {
                return this.chat.IsTop;
            }
        }
        public DateTime? LastMessageTimeSort
        {
            get
            {
                return this.LastMessage?.MsgTime;
            }
        }

        public DateTime CreateTimeSort
        {
            get
            {
                return this.chat.CreateTime;
            }
        }
        #endregion
        #endregion

        #region 命令
        public ICommand ToggleChatIsTopCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.Chat.IsTop = !this.Chat.IsTop;
                    this.NotifyPropertyChange(() => this.IsTopSort);
                    this.eventAggregator.PublishAsync(new RefreshChatsEventArgs());
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

        public ICommand ClearInputMessageContentCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.InputMessageContent = null;
                });
            }
        }

        public ICommand CaptureScreenCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (applicationContextViewModel.Setting.IsHideWhenCaptureScreen)
                    {
                        this.eventAggregator.Publish(new CaptureScreenEventArgs() { Action = CaptureScreenAction.Begin });
                    }
                    this.InputImageMessageContent(CaptureScreenTools.CallCaptureScreenProcess());
                    if (applicationContextViewModel.Setting.IsHideWhenCaptureScreen)
                    {
                        this.eventAggregator.Publish(new CaptureScreenEventArgs() { Action = CaptureScreenAction.End });
                    }
                });
            }
        }

        public ICommand SelectImageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
                    fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                    fileDialog.Filter = "图片|*.jpg;*.jpeg;*.png;*.gif";
                    fileDialog.Multiselect = true;

                    if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        foreach (string fileName in fileDialog.FileNames)
                        {
                            this.InputImageMessageContent(new FileInfo(fileName), true);
                        }
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
                    System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
                    fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    fileDialog.Filter = "文件|*.*";
                    fileDialog.Multiselect = true;

                    if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        foreach (string fileName in fileDialog.FileNames)
                        {
                            this.InputFileMessageContent(new FileInfo(fileName), true);
                        }
                    }
                });
            }
        }

        public ICommand RollbackMessageCommand
        {
            get
            {
                return new RelayCommand<MessageInfo>((oldMessage) =>
                {
                    new Action(() =>
                    {
                        lock (this.Messages)
                        {
                            this.Messages.Remove(oldMessage);
                        }
                    }).ExecuteInUIThread();
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
                }, () => this.InputMessageContent != null);
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

        #region 内容复制粘贴

        public ICommand CopyMessageContentCommand
        {
            get
            {
                return new RelayCommand<MessageContentInfo>((messageContent) =>
                {
                    IDataObject dataObject = new DataObject();
                    dataObject.SetData(typeof(MessageContentInfo).FullName, messageContent.ObjectToJson());
                    Clipboard.SetDataObject(dataObject, false);
                });
            }
        }

        public ICommand PasteMessageContentCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (Clipboard.GetDataObject().GetDataPresent(typeof(MessageContentInfo).FullName))
                    {
                        MessageContentInfo messageContent = (Clipboard.GetDataObject().GetData(typeof(MessageContentInfo).FullName) as string).JsonToObject<MessageContentInfo>();
                        this.InputObjectMessageContent(messageContent);
                    }
                    else if (Clipboard.GetDataObject().GetDataPresent(DataFormats.FileDrop))
                    {
                        foreach (string fileName in Clipboard.GetDataObject().GetData(DataFormats.FileDrop) as string[])
                        {
                            if ("*.jpg;*.jpeg;*.png;*.gif".Contains(Path.GetExtension(fileName)))
                            {
                                this.InputImageMessageContent(new FileInfo(fileName), true);
                            }
                            else
                            {
                                this.InputFileMessageContent(new FileInfo(fileName), true);
                            }
                        }
                        this.InputImageMessageContent(Clipboard.GetDataObject().GetData(DataFormats.Bitmap) as Image);
                    }
                    else if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Bitmap))
                    {
                        this.InputImageMessageContent(Clipboard.GetDataObject().GetData(DataFormats.Bitmap) as Image);
                    }
                    else if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Html))
                    {
                        this.InputHtmlMessageContent(Clipboard.GetDataObject().GetData(DataFormats.Html) as string);
                    }
                    else if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
                    {
                        this.InputTextMessageContent(Clipboard.GetDataObject().GetData(DataFormats.Text) as string);
                    }
                });
            }
        }
        #endregion

        #region 内容拖拽
        public void DragEnterMessageContentCommand(object sender, DragEventArgs e)
        {
            this.IsDragMessageContent = true;

            e.Handled = true;
        }

        public void DragLeaveMessageContentCommand(object sender, DragEventArgs e)
        {
            this.IsDragMessageContent = false;

            e.Handled = true;
        }

        public void DragOverMessageContentCommand(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effects = DragDropEffects.Copy;
            else if (e.Data.GetDataPresent(DataFormats.Bitmap)) e.Effects = DragDropEffects.Copy;
            else if (e.Data.GetDataPresent(DataFormats.Html)) e.Effects = DragDropEffects.Copy;
            else if (e.Data.GetDataPresent(DataFormats.Text)) e.Effects = DragDropEffects.Copy;
            else e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        public void DropMessageContentCommand(object sender, DragEventArgs e)
        {
            this.IsDragMessageContent = false;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                foreach (string fileName in e.Data.GetData(DataFormats.FileDrop) as string[])
                {
                    if ("*.jpg;*.jpeg;*.png;*.gif".Contains(Path.GetExtension(fileName)))
                    {
                        this.InputImageMessageContent(new FileInfo(fileName), true);
                    }
                    else
                    {
                        this.InputFileMessageContent(new FileInfo(fileName), true);
                    }
                }
                e.Handled = true;
            }
            else if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                this.InputImageMessageContent(e.Data.GetData(DataFormats.Bitmap) as Image);
                e.Handled = true;
            }
            else if (e.Data.GetDataPresent(DataFormats.Html))
            {
                this.InputHtmlMessageContent(e.Data.GetData(DataFormats.Html) as string);
                e.Handled = true;
            }
            else if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        #endregion
        #endregion

        #region 方法

        private async void InputTextMessageContent(string text)
        {
            await this.eventAggregator.PublishAsync(new InputMessageContentEventArgs() { MessageContent = MessageTools.CreateTextMessageContent(text) });
        }

        private async void InputHtmlMessageContent(string html)
        {
            await this.eventAggregator.PublishAsync(new InputMessageContentEventArgs() { MessageContent = MessageTools.CreateHtmlMessageContent(html) });
        }

        private async void InputImageMessageContent(FileInfo file, bool isSync = false)
        {
            if (isSync) this.eventAggregator.Publish(new InputMessageContentEventArgs() { MessageContent = MessageTools.CreateImageMessageContent(file) });
            else await this.eventAggregator.PublishAsync(new InputMessageContentEventArgs() { MessageContent = MessageTools.CreateImageMessageContent(file) });
        }

        private async void InputImageMessageContent(Image image)
        {
            if (image == null) return;
            FileInfo file = await new Func<FileInfo>(() =>
            {
                string tempFileName = Path.Combine(Path.GetTempPath(), AppTools.AppName, Guid.NewGuid().ToString() + ".png");
                image.ImageToStream(ImageFormat.Png).StreamToFile(tempFileName);
                return new FileInfo(tempFileName);
            }).ExecuteInTask();
            await this.eventAggregator.PublishAsync(new InputMessageContentEventArgs() { MessageContent = MessageTools.CreateImageMessageContent(file) });
        }

        private async void InputFileMessageContent(FileInfo file, bool isSync = false)
        {
            if (isSync) this.eventAggregator.Publish(new InputMessageContentEventArgs() { MessageContent = MessageTools.CreateFileMessageContent(file) });
            else await this.eventAggregator.PublishAsync(new InputMessageContentEventArgs() { MessageContent = MessageTools.CreateFileMessageContent(file) });
        }

        private async void InputObjectMessageContent(MessageContentInfo messageContent)
        {
            await this.eventAggregator.PublishAsync(new InputMessageContentEventArgs() { MessageContent = messageContent });
        }

        public override bool Equals(object obj)
        {
            return this.chat.Equals((obj as ChatViewModel)?.chat);
        }

        public override int GetHashCode()
        {
            return this.chat.GetHashCode();
        }

        public virtual void Dispose()
        {
            this.eventAggregator.Unsubscribe(this);
        }
        #endregion
    }
}
