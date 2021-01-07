using DotLiquid.Util;
using EMChat2.Common;
using EMChat2.Model.BaseInfo;
using EMChat2.Event;
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
using EMChat2.Model.Enum;

namespace EMChat2.ViewModel.Main.Tabs.Chat
{
    public abstract class ChatViewModel : PropertyChangedBase, IDisposable, ISelectable, IEventHandle<SettingChangedEventArgs>
    {
        #region 构造函数
        public ChatViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, EmotionPickerAreaViewModel emotionPickerAreaViewModel, ChatModel chat, ChatService chatService, SystemService systemService)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.emotionPickerAreaViewModel = emotionPickerAreaViewModel;
            this.chat = chat;
            this.chatService = chatService;
            this.systemService = systemService;
            this.Messages = new ObservableCollection<MessageModel>();
        }
        #endregion

        #region 属性
        protected IWindowManager windowManager;
        protected EventAggregator eventAggregator;
        protected ChatService chatService;
        protected SystemService systemService;
        private ChatModel chat;
        public ChatModel Chat
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
        private ObservableCollection<MessageModel> messages;
        public ObservableCollection<MessageModel> Messages
        {
            get
            {
                return this.messages;
            }
            set
            {
                this.messages = value;
                this.NotifyPropertyChange(() => this.Messages);
                this.NoticeMessagesChange();
                this.messages.CollectionChanged += (s, e) =>
                {
                    lock (this)
                    {
                        this.NoticeMessagesChange();
                        this.ReadMessage();
                    }
                };

                lock (this.messages)
                {
                    ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.messages);
                    collectionView.SortDescriptions.Add(new SortDescription("Time", ListSortDirection.Ascending));
                    this.MessagesCollectionView = collectionView;
                }
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
        private MessageContentModel inputMessageContent;
        public MessageContentModel InputMessageContent
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
        private MessageContentModel temporaryInputMessagContent;
        public MessageContentModel TemporaryInputMessagContent
        {
            get
            {
                MessageContentModel messageContent = this.temporaryInputMessagContent;
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
        public MessageModel[] NotReadMessages
        {
            get
            {
                lock (this.messages)
                {
                    return this.messages.Where(u => !u.IsSendFrom(ApplicationContextViewModel.CurrentStaff) && u.State.Equals(MessageStateEnum.Received)).ToArray();
                }
            }
        }
        public int NotReadMessagesCount
        {
            get
            {
                return NotReadMessages.Count();
            }
        }
        public MessageModel LastMessage
        {
            get
            {
                lock (this.messages)
                {
                    this.messagesCollectionView.MoveCurrentToLast();
                    return this.messagesCollectionView.CurrentItem as MessageModel;
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
        private BusinessSettingModel BusinessSetting
        {
            get
            {
                BusinessSettingModel value = ApplicationContextViewModel.Setting?.BusinessSettings.FirstOrDefault(u => u.Business == chat.Business);
                if (value == null)
                {
                    value = new BusinessSettingModel();
                }
                return value;
            }
        }
        public bool AllowInputText
        {
            get
            {
                return BusinessSetting.AllowInputText;
            }
        }
        public bool IsSelected { get; set; }

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
                return this.LastMessage?.Time;
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
                }, () =>
                {
                    return this.InputMessageContent != null;
                });
            }
        }

        private async void CaptureScreen(CaptureScreenTypeEnum type)
        {
            if (type == CaptureScreenTypeEnum.HideApplication)
            {
                await this.eventAggregator.PublishAsync<CaptureScreenEventArgs>(new CaptureScreenEventArgs() { Action = CaptureScreenAction.Begin });
                await Task.Delay(500);//TODO 通过延迟解决窗口还未关闭就启动截图子进程的bug
                this.InputImageMessageContent(CaptureScreenTools.CallCaptureScreenProcess());
                await this.eventAggregator.PublishAsync<CaptureScreenEventArgs>(new CaptureScreenEventArgs() { Action = CaptureScreenAction.End });
            }
            else if (type == CaptureScreenTypeEnum.NotHideApplication)
            {
                await Task.Delay(500);
                this.InputImageMessageContent(CaptureScreenTools.CallCaptureScreenProcess());
            }
        }

        public ICommand CaptureScreenCommand
        {
            get
            {
                return new RelayCommand<CaptureScreenTypeEnum>((type) =>
                {
                    CaptureScreen(type);
                }, (type) =>
                {
                    return BusinessSetting.AllowCaptureScreen;
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
                    fileDialog.Filter = "图片|" + BaseTools.GetImageFileExtension();
                    fileDialog.Multiselect = true;

                    if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        foreach (string fileName in fileDialog.FileNames)
                        {
                            this.InputImageMessageContent(new FileInfo(fileName), true);
                        }
                    }
                }, () =>
                {
                    return BusinessSetting.AllowSelectImage;
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
                }, () =>
                {
                    return BusinessSetting.AllowSelectFile;
                });
            }
        }

        public ICommand SendMessageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    MessageModel message = MessageTools.CreateMessage(applicationContextViewModel.CurrentStaff, this.Chat, this.InputMessageContent);
                    if (message == null) return;
                    new Action(() =>
                    {
                        this.InputMessageContent = null;
                        lock (this.Messages)
                        {
                            this.Messages.Add(message);
                        }
                    }).ExecuteInUIThread();
                    chatService.SendMessage(message);
                }, () =>
                {
                    return BusinessSetting.AllowSendMessage && this.InputMessageContent != null;
                });
            }
        }

        public ICommand ResendMessageCommand
        {
            get
            {
                return new RelayCommand<MessageModel>((oldMessage) =>
                {
                    MessageModel message = MessageTools.CreateMessage(applicationContextViewModel.CurrentStaff, this.Chat, oldMessage);
                    if (message == null) return;
                    new Action(() =>
                    {
                        lock (this.Messages)
                        {
                            this.Messages.Remove(oldMessage);
                            this.Messages.Add(message);
                        }
                    }).ExecuteInUIThread();
                    chatService.SendMessage(message);
                }, (oldMessage) =>
                {
                    return oldMessage != null && BusinessSetting.AllowSendMessage && oldMessage.IsSendFrom(ApplicationContextViewModel.CurrentStaff) && (oldMessage.State == MessageStateEnum.SendFailure || oldMessage.State == MessageStateEnum.Refused);
                });
            }
        }

        public ICommand RevokeMessageCommand
        {
            get
            {
                return new RelayCommand<MessageModel>((oldMessage) =>
                {
                    if (ModifyMessageState(oldMessage, MessageStateEnum.Revoked))
                    {
                        MessageModel revokeMessageEvent = MessageTools.CreateMessage(applicationContextViewModel.CurrentStaff, this.Chat, MessageTools.CreateRevokeMessageEventMessageContent(oldMessage));
                        this.chatService.SendMessage(revokeMessageEvent);
                    }
                }, (oldMessage) =>
                {
                    return oldMessage != null && BusinessSetting.AllowRevokeMessage && oldMessage.IsSendFrom(ApplicationContextViewModel.CurrentStaff) && (DateTime.Now - oldMessage.Time).TotalMinutes < BusinessSetting.MaxRollbackMessageTotalMinutes && (oldMessage.State == MessageStateEnum.SendSuccess || oldMessage.State == MessageStateEnum.Received || oldMessage.State == MessageStateEnum.Readed);
                });
            }
        }

        #region 内容复制粘贴

        public ICommand CopyMessageContentCommand
        {
            get
            {
                return new RelayCommand<MessageContentModel>((messageContent) =>
                {
                    IDataObject dataObject = new DataObject();
                    dataObject.SetData(typeof(MessageContentModel).FullName, messageContent.ObjectToJson());
                    Clipboard.SetDataObject(dataObject, false);
                });
            }
        }

        protected virtual bool CanPaste(MessageContentModel messageContent)
        {
            bool canPaste = true;
            switch (messageContent.Type)
            {
                case MessageTypeConst.Text: canPaste = BusinessSetting.AllowInputText; break;
                case MessageTypeConst.Emotion: canPaste = true; break;
                case MessageTypeConst.Image: canPaste = BusinessSetting.AllowSelectImage; break;
                case MessageTypeConst.Voice: canPaste = true; break;
                case MessageTypeConst.Video: canPaste = true; break;
                case MessageTypeConst.Link: canPaste = true; break;
                case MessageTypeConst.File: canPaste = BusinessSetting.AllowSelectFile; break;
                case MessageTypeConst.Mixed:
                    {
                        foreach (MessageContentModel mixedItem in (messageContent.Content as MixedMessageContent).Items)
                            canPaste &= CanPaste(mixedItem);
                    }
                    break;
                default: canPaste = false; break;
            }
            return canPaste;
        }

        public ICommand PasteMessageContentCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (Clipboard.GetDataObject().GetDataPresent(typeof(MessageContentModel).FullName))
                    {
                        MessageContentModel messageContent = (Clipboard.GetDataObject().GetData(typeof(MessageContentModel).FullName) as string).JsonToObject<MessageContentModel>();
                        if (CanPaste(messageContent))
                            this.InputObjectMessageContent(messageContent);
                    }
                    else if (Clipboard.GetDataObject().GetDataPresent(DataFormats.FileDrop))
                    {
                        foreach (string fileName in Clipboard.GetDataObject().GetData(DataFormats.FileDrop) as string[])
                        {
                            if (fileName.IsImageFile())
                            {
                                if (BusinessSetting.AllowSelectImage)
                                    this.InputImageMessageContent(new FileInfo(fileName), true);
                            }
                            else
                            {
                                if (BusinessSetting.AllowSelectFile)
                                    this.InputFileMessageContent(new FileInfo(fileName), true);
                            }
                        }
                        this.InputImageMessageContent(Clipboard.GetDataObject().GetData(DataFormats.Bitmap) as Image);
                    }
                    else if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Bitmap))
                    {
                        if (BusinessSetting.AllowSelectImage)
                            this.InputImageMessageContent(Clipboard.GetDataObject().GetData(DataFormats.Bitmap) as Image);
                    }
                    else if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Html))
                    {
                        if (BusinessSetting.AllowInputText)
                            this.InputHtmlMessageContent(Clipboard.GetDataObject().GetData(DataFormats.Html) as string);
                    }
                    else if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
                    {
                        if (BusinessSetting.AllowInputText)
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
                    if (fileName.IsImageFile())
                    {
                        if (BusinessSetting.AllowSelectImage)
                            this.InputImageMessageContent(new FileInfo(fileName), true);
                    }
                    else
                    {
                        if (BusinessSetting.AllowSelectFile)
                            this.InputFileMessageContent(new FileInfo(fileName), true);
                    }
                }
                e.Handled = true;
            }
            else if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                if (BusinessSetting.AllowSelectImage)
                    this.InputImageMessageContent(e.Data.GetData(DataFormats.Bitmap) as Image);
                e.Handled = true;
            }
            else if (e.Data.GetDataPresent(DataFormats.Html))
            {
                if (BusinessSetting.AllowInputText)
                    this.InputHtmlMessageContent(e.Data.GetData(DataFormats.Html) as string);
                e.Handled = true;
            }
            else if (e.Data.GetDataPresent(DataFormats.Text))
            {
                if (BusinessSetting.AllowInputText)
                    e.Handled = false;
                else
                    e.Handled = true;
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
            if (string.IsNullOrEmpty(text)) return;
            await this.eventAggregator.PublishAsync(new TemporaryInputMessagContentChangedEventArgs() { MessageContent = MessageTools.CreateTextMessageContent(text) });
        }

        private async void InputHtmlMessageContent(string html)
        {
            if (string.IsNullOrEmpty(html)) return;
            await this.eventAggregator.PublishAsync(new TemporaryInputMessagContentChangedEventArgs() { MessageContent = MessageTools.CreateHtmlMessageContent(html) });
        }

        private async void InputImageMessageContent(FileInfo file, bool isSync = false)
        {
            if (file == null) return;
            if (isSync) this.eventAggregator.Publish(new TemporaryInputMessagContentChangedEventArgs() { MessageContent = MessageTools.CreateImageMessageContent(file) });
            else await this.eventAggregator.PublishAsync(new TemporaryInputMessagContentChangedEventArgs() { MessageContent = MessageTools.CreateImageMessageContent(file) });
        }

        private async void InputImageMessageContent(Image image)
        {
            if (image == null) return;
            string filePath = Path.Combine(Path.GetTempPath(), AppTools.AppName, Guid.NewGuid().ToString() + ".png");
            await new Action(() => { image.ImageToStream().StreamToFile(filePath); }).ExecuteInTask();
            await this.eventAggregator.PublishAsync(new TemporaryInputMessagContentChangedEventArgs() { MessageContent = MessageTools.CreateImageMessageContent(new FileInfo(filePath)) });
        }

        private async void InputFileMessageContent(FileInfo file, bool isSync = false)
        {
            if (file == null) return;
            if (isSync) this.eventAggregator.Publish(new TemporaryInputMessagContentChangedEventArgs() { MessageContent = MessageTools.CreateFileMessageContent(file) });
            else await this.eventAggregator.PublishAsync(new TemporaryInputMessagContentChangedEventArgs() { MessageContent = MessageTools.CreateFileMessageContent(file) });
        }

        private async void InputObjectMessageContent(MessageContentModel messageContent)
        {
            if (messageContent == null) return;
            await this.eventAggregator.PublishAsync(new TemporaryInputMessagContentChangedEventArgs() { MessageContent = messageContent });
        }

        protected virtual async void NoticeMessagesChange()
        {
            this.NotifyPropertyChange(() => this.NotReadMessagesCount);
            this.NotifyPropertyChange(() => this.LastMessage);
            this.NotifyPropertyChange(() => this.LastMessageTimeSort);
            await this.eventAggregator.PublishAsync(new NotReadMessageCountChangedEventArgs());
            await this.eventAggregator.PublishAsync(new RefreshChatsEventArgs());
        }

        public abstract bool RecvMessage(MessageModel message);

        public abstract int ReadMessage();

        public virtual bool UpdateMessage(string messageId, MessageStateEnum state)
        {
            if (messageId == null) return false;
            lock (this.Messages)
            {
                MessageModel message = this.Messages.FirstOrDefault(u => u.Id.Equals(messageId));
                if (message == null) return false;
                return ModifyMessageState(message, state);
            }
        }

        public virtual bool UpdateMessage(string messageId, string newMessageId)
        {
            if (messageId == null || newMessageId == null) return false;
            lock (this.Messages)
            {
                MessageModel message = this.Messages.FirstOrDefault(u => u.Id.Equals(messageId));
                if (message == null) return false;
                message.Id = newMessageId;
                return true;
            }
        }

        protected virtual bool ModifyMessageState(MessageModel message, MessageStateEnum state)
        {
            if (message == null) return false;
            if (state > message.State)
            {
                message.State = state;
                this.NoticeMessagesChange();
                return true;
            }
            return false;
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

        #region 事件处理
        public void Handle(SettingChangedEventArgs Message)
        {
            this.NotifyPropertyChange(() => this.BusinessSetting);
            this.NotifyPropertyChange(() => this.AllowInputText);
        }
        #endregion
    }
}
