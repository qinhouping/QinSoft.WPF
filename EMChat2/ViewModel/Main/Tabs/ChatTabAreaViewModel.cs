using EMChat2.Common;
using EMChat2.Model.Entity;
using EMChat2.View.Main.Tabs.Chat;
using EMChat2.ViewModel.Main.Tabs.Chat;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.ViewModel.Main.Tabs
{
    [Component]
    public class ChatTabAreaViewModel : PropertyChangedBase
    {
        #region 构造函数
        public ChatTabAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, EmotionPickerAreaViewModel emotionPickerAreaViewModel)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.emotionPickerAreaViewModel = emotionPickerAreaViewModel;

            //TODO 测试数据
            this.chatTabItems = new ObservableCollection<ChatTabItemAreaView>()
            {
                new PrivateChatTabItemAreaView() {
                DataContext=new PrivateChatTabItemAreaViewModel(this.windowManager,this.eventAggregator,this.applicationContextViewModel,this.emotionPickerAreaViewModel,this.CreatePrivateChat(new CustomerInfo(){
                     Id=Guid.NewGuid().ToString(),
                     ImUserId="1",
                     Name="私聊-投顾",
                     HeaderImageUrl="https://tse3-mm.cn.bing.net/th/id/OIP.BiS73OXRCWwEyT1aajtTpAAAAA?w=175&h=180&c=7&o=5&pid=1.7",
                     State= UserStateEnum.Online,
                     Business=BusinessEnum.Advisor,
                     Uid="1"
                }, BusinessEnum.Advisor))
            }, new PrivateChatTabItemAreaView() {
                DataContext=new PrivateChatTabItemAreaViewModel(this.windowManager,this.eventAggregator,this.applicationContextViewModel,this.emotionPickerAreaViewModel,this.CreatePrivateChat(new CustomerInfo(){
                     Id=Guid.NewGuid().ToString(),
                     ImUserId="2",
                     Name="私聊-专家",
                     HeaderImageUrl="https://tse3-mm.cn.bing.net/th/id/OIP.9QpPBCb9vEIfWQq7hQogJAD6D6?w=175&h=180&c=7&o=5&pid=1.7",
                     State= UserStateEnum.Offline,
                     Business=BusinessEnum.Expert,
                     Uid="2"
                }, BusinessEnum.Expert))
            },new PrivateChatTabItemAreaView() {
                DataContext=new PrivateChatTabItemAreaViewModel(this.windowManager,this.eventAggregator,this.applicationContextViewModel,this.emotionPickerAreaViewModel,this.CreatePrivateChat(new CustomerInfo(){
                     Id=Guid.NewGuid().ToString(),
                     ImUserId="3",
                     Name="私聊-售后",
                     HeaderImageUrl="https://dss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=3582194852,1481557220&fm=26&gp=0.jpg",
                     State= UserStateEnum.Offline,
                     Business=BusinessEnum.PostSale,
                     Uid="3"
                }, BusinessEnum.PostSale))
            },new PrivateChatTabItemAreaView() {
                DataContext=new PrivateChatTabItemAreaViewModel(this.windowManager,this.eventAggregator,this.applicationContextViewModel,this.emotionPickerAreaViewModel,this.CreatePrivateChat(new CustomerInfo(){
                     Id=Guid.NewGuid().ToString(),
                     ImUserId="4",
                     Name="私聊-售前",
                     HeaderImageUrl="https://tse3-mm.cn.bing.net/th/id/OIP.9QpPBCb9vEIfWQq7hQogJAD6D6?w=175&h=180&c=7&o=5&pid=1.7",
                     State= UserStateEnum.Offline,
                     Business=BusinessEnum.PreSale,
                     Uid="4"
                }, BusinessEnum.PreSale))
            },new PrivateChatTabItemAreaView() {
                DataContext=new PrivateChatTabItemAreaViewModel(this.windowManager,this.eventAggregator,this.applicationContextViewModel,this.emotionPickerAreaViewModel,this.CreatePrivateChat(new SystemUserInfo(){
                     Id=Guid.NewGuid().ToString(),
                     ImUserId="5",
                     Name="私聊-广播",
                     HeaderImageUrl="https://dss2.bdstatic.com/70cFvnSh_Q1YnxGkpoWK1HF6hhy/it/u=1783781708,2939608912&fm=26&gp=0.jpg",
                     State= UserStateEnum.Offline
                }))
            },new PrivateChatTabItemAreaView() {
                DataContext=new PrivateChatTabItemAreaViewModel(this.windowManager,this.eventAggregator,this.applicationContextViewModel,this.emotionPickerAreaViewModel,this.CreatePrivateChat(new StaffInfo(){
                     Id=Guid.NewGuid().ToString(),
                     ImUserId="5",
                     Name="私聊-内部员工",
                     HeaderImageUrl="https://dss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=2228998571,2651049899&fm=26&gp=0.jpg",
                     State= UserStateEnum.Offline,
                     WorkCode="180333"
                }))
            }};
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
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
        private ObservableCollection<ChatTabItemAreaView> chatTabItems;
        public ObservableCollection<ChatTabItemAreaView> ChatTabItems
        {
            get
            {
                return this.chatTabItems;
            }
            set
            {
                this.chatTabItems = value;
                this.NotifyPropertyChange(() => this.ChatTabItems);
            }
        }
        #endregion

        #region 内部方法
        private ChatInfo CreatePrivateChat(UserInfo userInfo, BusinessEnum? business = null)
        {
            List<string> ids = new List<string>() { applicationContextViewModel.CurrentStaff.ImUserId, userInfo.ImUserId, business?.ToString() };
            ids.Sort();
            ChatInfo chat = new ChatInfo();
            chat.Id = Guid.NewGuid().ToString();
            chat.ChatId = string.Join("_", ids).MD5();
            chat.RoomId = null;
            chat.Business = business;
            chat.Name = userInfo.Name;
            chat.Type = ChatType.Private;
            chat.HeaderImageUrl = userInfo.HeaderImageUrl;
            chat.IsTop = true;
            chat.IsInform = false;
            chat.ChatUsers = new ObservableCollection<UserInfo>(new UserInfo[] { applicationContextViewModel.CurrentStaff, userInfo });
            return chat;
        }
        #endregion
    }
}
