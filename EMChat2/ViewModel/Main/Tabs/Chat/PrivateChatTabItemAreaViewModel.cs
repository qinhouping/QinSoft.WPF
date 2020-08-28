using EMChat2.Common;
using EMChat2.Model.Entity;
using QinSoft.Event;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.ViewModel.Main.Tabs.Chat
{
    public class PrivateChatTabItemAreaViewModel : ChatTabItemAreaViewModel
    {
        #region 构造函数
        public PrivateChatTabItemAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, EmotionPickerAreaViewModel emotionPickerAreaViewModel, ChatInfo chat) : base(windowManager, eventAggregator, applicationContextViewModel, emotionPickerAreaViewModel, chat)
        {
            if (this.Chat.Type != ChatType.Private) throw new ArgumentOutOfRangeException("is not private chat");



            //TODO 测试数据
            this.Messages.Add(new TextMessageInfo()
            {
                Id = Guid.NewGuid().ToString(),
                MsgId = "1",
                FromUser = "1111",
                ToUsers = new string[] { "1" },
                Business = null,
                MsgTime = DateTime.Now.AddYears(-1),
                RoomId = null,
                ChatId = "123123123",
                State = MsgState.Received,
                Text = new TextMessageContext
                {
                    Content = "测试信息"
                }
            });
        }
        #endregion
    }
}
