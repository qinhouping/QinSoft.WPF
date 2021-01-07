using EMChat2.Model.BaseInfo;
using EMChat2.Model.Enum;
using EMChat2.Model.IM;
using IEMWorks.Common;
using IM.SDK;
using IM.SDK.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Common
{
    public static class TestData
    {
        private static string AppKey = "ImeUhwobjC";
        private static string AppSecret = "FoeTj0rcl8PWpxSBu1mK";

        static TestData()
        {
            HttpUserClient userClient = new HttpUserClient(TestIMServer.ApiUrl);
            foreach (TestDataItem item in TestUsers)
            {
                JsonResult<IMToken> result = userClient.OtherLoginAuth(new OtherAuthRequest()
                {
                    AppKey = AppKey,
                    AppSecret = AppSecret,
                    NickName = item.TestStaff.Name,
                    UserID = item.TestStaff.Id,
                    Version = AppTools.AppVersion.Major,
                    Device = AppTools.AppName
                });
                if (result.data != null)
                {
                    item.TestStaff.ImUserId = result.data.IMUserID;
                    item.TestIMUser = new IMUserModel()
                    {
                        AppKey = AppKey,
                        Id = result.data.IMUserID,
                        Token = result.data.Token,
                        RefreshToken = result.data.RefreshToken
                    };
                }
            }
        }

        public readonly static IMServerModel TestIMServer = new IMServerModel
        {
            ApiUrl = "https://61.152.230.122:16060",
            IP = "61.152.230.122",
            Port = 17070
        };

        public readonly static TestDataItem[] TestUsers = new TestDataItem[]
        {
            new TestDataItem()
            {
                TestStaff= new StaffModel() {
                    Id = "C170052",
                    WorkCode = "C170052",
                    Name = "C170052",
                    HeaderImageUrl = "https://tse3-mm.cn.bing.net/th/id/OIP.BiS73OXRCWwEyT1aajtTpAAAAA?w=175&h=180&c=7&o=5&pid=1.7",
                    State = UserStateEnum.Online,
                    BusinessList = new ObservableCollection<BusinessEnum>() {
                        BusinessEnum.Inside,
                        BusinessEnum.Advisor,
                        BusinessEnum.Expert
                    }
                }
            },
            new TestDataItem()
            {
                TestStaff= new StaffModel()  {
                    Id = "C170056",
                    WorkCode = "C170056",
                    Name = "C170056",
                    HeaderImageUrl = "https://ss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=1764219719,2359539133&fm=26&gp=0.jpg",
                    State = UserStateEnum.Online,
                    BusinessList = new ObservableCollection<BusinessEnum>() {
                        BusinessEnum.Inside,
                        BusinessEnum.Advisor,
                        BusinessEnum.Expert
                    }
                 }
            }
        };

        public readonly static TestDataItem Current = TestUsers[1];

        public readonly static TestDataItem Opposite = TestUsers[0];
    }

    public class TestDataItem
    {
        public StaffModel TestStaff { get; set; }
        public IMUserModel TestIMUser { get; set; }
    }
}
