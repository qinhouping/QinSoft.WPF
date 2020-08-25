using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Entity
{
    public class StaffInfo : PropertyChangedBase
    {
        private string id;
        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
                this.NotifyPropertyChange(() => this.Id);
            }
        }

        private string imUserId;
        public string ImUserId
        {
            get
            {
                return this.imUserId;
            }
            set
            {
                this.imUserId = value;
                this.NotifyPropertyChange(() => this.ImUserId);
            }
        }

        private string code;
        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
                this.NotifyPropertyChange(() => this.Code);
            }
        }

        private string name;
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                this.NotifyPropertyChange(() => this.Name);
            }
        }

        private string headerImageUrl;
        public string HeaderImageUrl
        {
            get
            {
                return this.headerImageUrl;
            }
            set
            {
                this.headerImageUrl = value;
                this.NotifyPropertyChange(() => this.HeaderImageUrl);
            }
        }

        private UserStateEnum state;
        public UserStateEnum State
        {
            get
            {
                return this.state;
            }
            set
            {
                this.state = value;
                this.NotifyPropertyChange(() => this.State);
            }
        }
    }
}
