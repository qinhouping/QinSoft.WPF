﻿using EMChat2.Common.Cef;
using EMChat2.Model.Entity;
using EMChat2.ViewModel.Main.Tabs.User;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.ViewModel.Main.Tabs.Chat
{
    public class PrivateChatSliderAreaViewModel : PropertyChangedBase, IDisposable
    {
        #region 构造函数
        public PrivateChatSliderAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, ChatInfo chat)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.chat = chat;
            this.customerDetailAreaViewModel = new CustomerDetailAreaViewModel(windowManager, eventAggregator);
            this.staffDetailAreaViewModel = new StaffDetailAreaViewModel(windowManager, eventAggregator);
            this.address = "https://github.com/qinhouping";
            this.computerInfoCefJsObject = new ComputerInfoCefJsObject();
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private string address;
        public string Address
        {
            get
            {
                return this.address;
            }
            set
            {
                this.address = value;
                this.NotifyPropertyChange(() => this.Address);
            }
        }

        private ComputerInfoCefJsObject computerInfoCefJsObject;
        public ComputerInfoCefJsObject ComputerInfoCefJsObject
        {
            get
            {
                return this.computerInfoCefJsObject;
            }
            set
            {
                this.computerInfoCefJsObject = value;
                this.NotifyPropertyChange(() => this.ComputerInfoCefJsObject);
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
                this.NotifyPropertyChange(() => this.ChatUser);
            }
        }
        public UserInfo ChatUser
        {
            get
            {
                return this.chat.ChatUsers.FirstOrDefault(u => !u.Equals(ApplicationContextViewModel.CurrentStaff));
            }
        }
        private CustomerDetailAreaViewModel customerDetailAreaViewModel;
        public CustomerDetailAreaViewModel CustomerDetailAreaViewModel
        {
            get
            {
                CustomerDetailAreaViewModel data = this.customerDetailAreaViewModel;
                data.Customer = this.ChatUser as CustomerInfo;
                return data;
            }
            set
            {
                this.customerDetailAreaViewModel = value;
                this.NotifyPropertyChange(() => this.CustomerDetailAreaViewModel);
            }
        }
        private StaffDetailAreaViewModel staffDetailAreaViewModel;
        public StaffDetailAreaViewModel StaffDetailAreaViewModel
        {
            get
            {
                StaffDetailAreaViewModel data = this.staffDetailAreaViewModel;
                data.Staff = this.ChatUser as StaffInfo;
                return data;
            }
            set
            {
                this.staffDetailAreaViewModel = value;
                this.NotifyPropertyChange(() => this.StaffDetailAreaViewModel);
            }
        }

        #endregion

        public void Dispose()
        {
            this.eventAggregator.Unsubscribe(this);
            this.customerDetailAreaViewModel.Dispose();
            this.staffDetailAreaViewModel.Dispose();
        }
    }
}