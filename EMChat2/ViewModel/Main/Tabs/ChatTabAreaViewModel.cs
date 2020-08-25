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
        public ChatTabAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;

            //TODO 测试数据
            this.chatTabItems = new ObservableCollection<ChatTabItemAreaView>() { new PrivateChatTabItemAreaView() {
                DataContext=new PrivateChatTabItemAreaViewModel(this.windowManager,this.eventAggregator,this.applicationContextViewModel,new CustomerInfo(){
                     Id=Guid.NewGuid().ToString(),
                     ImUserId="1",
                     Name="测试客户",
                     HeaderImageUrl="https://tse3-mm.cn.bing.net/th/id/OIP.BiS73OXRCWwEyT1aajtTpAAAAA?w=175&h=180&c=7&o=5&pid=1.7",
                     Remark="测试客户-备注",
                     State= UserStateEnum.Online,
                     Uid="1"
                })
            }, new PrivateChatTabItemAreaView() {
                DataContext=new PrivateChatTabItemAreaViewModel(this.windowManager,this.eventAggregator,this.applicationContextViewModel,new CustomerInfo(){
                     Id=Guid.NewGuid().ToString(),
                     ImUserId="2",
                     Name="测试客户2",
                     HeaderImageUrl="https://tse3-mm.cn.bing.net/th/id/OIP.9QpPBCb9vEIfWQq7hQogJAD6D6?w=175&h=180&c=7&o=5&pid=1.7",
                     Remark="测试客户-备注2",
                     State= UserStateEnum.Offline,
                     Uid="2"
                })
            },new PrivateChatTabItemAreaView() {
                DataContext=new PrivateChatTabItemAreaViewModel(this.windowManager,this.eventAggregator,this.applicationContextViewModel,new CustomerInfo(){
                     Id=Guid.NewGuid().ToString(),
                     ImUserId="2",
                     Name="测试客户2",
                     HeaderImageUrl="https://tse3-mm.cn.bing.net/th/id/OIP.9QpPBCb9vEIfWQq7hQogJAD6D6?w=175&h=180&c=7&o=5&pid=1.7",
                     Remark="测试客户-备注2",
                     State= UserStateEnum.Offline,
                     Uid="2"
                })
            },new PrivateChatTabItemAreaView() {
                DataContext=new PrivateChatTabItemAreaViewModel(this.windowManager,this.eventAggregator,this.applicationContextViewModel,new CustomerInfo(){
                     Id=Guid.NewGuid().ToString(),
                     ImUserId="2",
                     Name="测试客户2",
                     HeaderImageUrl="https://tse3-mm.cn.bing.net/th/id/OIP.9QpPBCb9vEIfWQq7hQogJAD6D6?w=175&h=180&c=7&o=5&pid=1.7",
                     Remark="测试客户-备注2",
                     State= UserStateEnum.Offline,
                     Uid="2"
                })
            },new PrivateChatTabItemAreaView() {
                DataContext=new PrivateChatTabItemAreaViewModel(this.windowManager,this.eventAggregator,this.applicationContextViewModel,new CustomerInfo(){
                     Id=Guid.NewGuid().ToString(),
                     ImUserId="2",
                     Name="测试客户2",
                     HeaderImageUrl="https://tse3-mm.cn.bing.net/th/id/OIP.9QpPBCb9vEIfWQq7hQogJAD6D6?w=175&h=180&c=7&o=5&pid=1.7",
                     Remark="测试客户-备注2",
                     State= UserStateEnum.Offline,
                     Uid="2"
                })
            },new PrivateChatTabItemAreaView() {
                DataContext=new PrivateChatTabItemAreaViewModel(this.windowManager,this.eventAggregator,this.applicationContextViewModel,new CustomerInfo(){
                     Id=Guid.NewGuid().ToString(),
                     ImUserId="2",
                     Name="测试客户2",
                     HeaderImageUrl="https://tse3-mm.cn.bing.net/th/id/OIP.9QpPBCb9vEIfWQq7hQogJAD6D6?w=175&h=180&c=7&o=5&pid=1.7",
                     Remark="测试客户-备注2",
                     State= UserStateEnum.Offline,
                     Uid="2"
                })
            },new PrivateChatTabItemAreaView() {
                DataContext=new PrivateChatTabItemAreaViewModel(this.windowManager,this.eventAggregator,this.applicationContextViewModel,new CustomerInfo(){
                     Id=Guid.NewGuid().ToString(),
                     ImUserId="2",
                     Name="测试客户2",
                     HeaderImageUrl="https://tse3-mm.cn.bing.net/th/id/OIP.9QpPBCb9vEIfWQq7hQogJAD6D6?w=175&h=180&c=7&o=5&pid=1.7",
                     Remark="测试客户-备注2",
                     State= UserStateEnum.Offline,
                     Uid="2"
                })
            },new PrivateChatTabItemAreaView() {
                DataContext=new PrivateChatTabItemAreaViewModel(this.windowManager,this.eventAggregator,this.applicationContextViewModel,new CustomerInfo(){
                     Id=Guid.NewGuid().ToString(),
                     ImUserId="2",
                     Name="测试客户2",
                     HeaderImageUrl="https://tse3-mm.cn.bing.net/th/id/OIP.9QpPBCb9vEIfWQq7hQogJAD6D6?w=175&h=180&c=7&o=5&pid=1.7",
                     Remark="测试客户-备注2",
                     State= UserStateEnum.Offline,
                     Uid="2"
                })
            },new PrivateChatTabItemAreaView() {
                DataContext=new PrivateChatTabItemAreaViewModel(this.windowManager,this.eventAggregator,this.applicationContextViewModel,new CustomerInfo(){
                     Id=Guid.NewGuid().ToString(),
                     ImUserId="2",
                     Name="测试客户2",
                     HeaderImageUrl="https://tse3-mm.cn.bing.net/th/id/OIP.9QpPBCb9vEIfWQq7hQogJAD6D6?w=175&h=180&c=7&o=5&pid=1.7",
                     Remark="测试客户-备注2",
                     State= UserStateEnum.Offline,
                     Uid="2"
                })
            },new PrivateChatTabItemAreaView() {
                DataContext=new PrivateChatTabItemAreaViewModel(this.windowManager,this.eventAggregator,this.applicationContextViewModel,new CustomerInfo(){
                     Id=Guid.NewGuid().ToString(),
                     ImUserId="2",
                     Name="测试客户2",
                     HeaderImageUrl="https://tse3-mm.cn.bing.net/th/id/OIP.9QpPBCb9vEIfWQq7hQogJAD6D6?w=175&h=180&c=7&o=5&pid=1.7",
                     Remark="测试客户-备注2",
                     State= UserStateEnum.Offline,
                     Uid="2"
                })
            },new PrivateChatTabItemAreaView() {
                DataContext=new PrivateChatTabItemAreaViewModel(this.windowManager,this.eventAggregator,this.applicationContextViewModel,new CustomerInfo(){
                     Id=Guid.NewGuid().ToString(),
                     ImUserId="2",
                     Name="测试客户2",
                     HeaderImageUrl="https://tse3-mm.cn.bing.net/th/id/OIP.9QpPBCb9vEIfWQq7hQogJAD6D6?w=175&h=180&c=7&o=5&pid=1.7",
                     Remark="测试客户-备注2",
                     State= UserStateEnum.Offline,
                     Uid="2"
                })
            },new PrivateChatTabItemAreaView() {
                DataContext=new PrivateChatTabItemAreaViewModel(this.windowManager,this.eventAggregator,this.applicationContextViewModel,new CustomerInfo(){
                     Id=Guid.NewGuid().ToString(),
                     ImUserId="2",
                     Name="测试客户2",
                     HeaderImageUrl="https://tse1-mm.cn.bing.net/th/id/OIP.F-iTVM6jkobQNl9tDXmODwAAAA?w=146&h=150&c=7&o=5&pid=1.7",
                     Remark="测试客户-备注3",
                     State= UserStateEnum.Offline,
                     Uid="2"
                })
            }};
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private ApplicationContextViewModel applicationContextViewModel;
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
    }
}
