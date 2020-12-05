using EMChat2.Model.BaseInfo;
using EMChat2.Model.IM;
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
                    ImUserId = "5419712458897862370",
                    State = UserStateEnum.Online,
                    BusinessList = new ObservableCollection<BusinessEnum>() {
                        BusinessEnum.Inside,
                        BusinessEnum.Advisor,
                        BusinessEnum.Expert
                    }
                },
                TestIMUser= new IMUserModel()
                {
                    Id = "5419712458897862370",
                    AppKey = "GunKnHXhJK",
                    Token = "LelmsBHVpZXoRmrLUn/L0a3MktjsJf/6OOpkir1+9RPoMHfW7CvE9P4la25TgGztayoDarZdDczaipX+O+eA+WBsWPQEEeWw4cAlBCEGa+474R7x+F/nW+70b5mSz4+kk7X4hM4LudgFpU2zXZCnng9IGlkkt5mZL7pl095zfDv6RrlLsQOA04cdHTBS0dwhj84iVfVHuuTnrowfrSvaNY2pE4PtNHMKtCzCIy5NG1ugwj+CTTxMpkkKcYKxnxx5b91m4c+aQfkuy4euv7XyCA==",
                    RefreshToken = "PSBuWJ0RlE2UxSseq9AjJVRJbjFctdhBz35ehfc0BPDJ/U70KPXigbI2u4BX8hU/ex9JPqeN6lISvtRwd5mB53liDjFRMwXqO9smvywQ3H4="
                }
            },
            new TestDataItem()
            {
                TestStaff= new StaffModel()  {
                    Id = "C170056",
                    WorkCode = "C170056",
                    Name = "C170056",
                    HeaderImageUrl = "https://ss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=1764219719,2359539133&fm=26&gp=0.jpg",
                    ImUserId = "4842645605049376593",
                    State = UserStateEnum.Online,
                    BusinessList = new ObservableCollection<BusinessEnum>() {
                        BusinessEnum.Inside,
                        BusinessEnum.Advisor,
                        BusinessEnum.Expert
                    }
                 },
                TestIMUser= new IMUserModel()
                {
                    Id = "4842645605049376593",
                    AppKey = "GunKnHXhJK",
                    Token = "LelmsBHVpZXoRmrLUn/L0a3MktjsJf/6OOpkir1+9RPoMHfW7CvE9P4la25TgGztj6n9MMFII6ZBfrj6gMwuta9lRMMiJYSU9kXFgZtSgcELNd4/1pVGu7ZjcWjWVNeGac2UAJRGAJTcQhqvnYR/jRRHBHkuC4Ip1Fm9G5fjUc7OzF1d7Kvo7thri/ATXgqcdTKHn9PwCTfAuSaeaQAzxOxBq/OyxRS8CXT6KF89iMD7dTfvnRzFgu6JyYspSxnVpH+XV++iLo/vlzUIGrvVxA==",
                    RefreshToken = "OsEJyLwycTJ0VvwmK/qMKjX160mSgpVzWReyOoPgAycZ6pRlVLVf5wqSldw5eT4x5hCi3rfM4ztF7VneELXxO8cQ2JQ/SkVCyqSdXqk5Bx0="
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
