using EMChat2.Model.Entity;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMChat2.ViewModel
{
    [Component]
    public class ApplicationContextViewModel : PropertyChangedBase
    {
        public ApplicationContextViewModel()
        {
            //TODO 测试数据
            this.currentStaff = new StaffInfo()
            {
                Id = Guid.NewGuid().ToString(),
                Code = "180366",
                Name = "秦后平",
                HeaderImageUrl = "https://ss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=1764219719,2359539133&fm=26&gp=0.jpg",
                ImUserId = "1111",
                State = UserStateEnum.Online
            };
        }

        public static readonly ApplicationContextViewModel Instance = new ApplicationContextViewModel();

        private bool isShowChatSlider = true;
        public bool IsShowChatSlider
        {
            get
            {
                return this.isShowChatSlider;
            }
            set
            {
                this.isShowChatSlider = value;
                this.NotifyPropertyChange(() => this.IsShowChatSlider);
            }
        }

        private StaffInfo currentStaff;
        public StaffInfo CurrentStaff
        {
            get
            {
                return this.currentStaff;
            }
            set
            {
                this.currentStaff = value;
                this.NotifyPropertyChange(() => this.CurrentStaff);
                this.NotifyPropertyChange(() => this.IsLogin);
            }
        }

        public bool IsLogin
        {
            get
            {
                return this.CurrentStaff != null;
            }
        }
    }
}
