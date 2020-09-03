using EMChat2.Common;
using EMChat2.Model.Entity;
using QinSoft.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EMChat2.View
{
    public static class ValueConverters
    {
        public static IValueConverter MessageToContentMarkConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    MessageInfo message = value as MessageInfo;
                    if (message == null) return null;
                    else return MessageTools.GetMessageContentMark(message.Content, message.Type);
                });
            }
        }

        public static IMultiValueConverter MessageToHorizontalAlignment
        {
            get
            {
                return new DelegateMultiValueConverter((values, targetType, parameter, cultInfo) =>
                {
                    MessageInfo message = values[0] as MessageInfo;
                    StaffInfo staff = values[1] as StaffInfo;
                    if (message.FromUser.Equals(staff.ImUserId) == true) return HorizontalAlignment.Right;
                    else return HorizontalAlignment.Left;
                });
            }
        }
        public static IValueConverter MessageToContentConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    MessageInfo message = value as MessageInfo;
                    if (message == null) return null;
                    else return MessageTools.GetMessageContentMark(message.Content, message.Type);
                });
            }
        }

        public static IMultiValueConverter MessageToUserConverter
        {
            get
            {
                return new DelegateMultiValueConverter((values, targetType, parameter, cultInfo) =>
                {
                    MessageInfo message = values[0] as MessageInfo;
                    ChatInfo chat = values[1] as ChatInfo;
                    return chat.ChatUsers.FirstOrDefault(u => u.ImUserId.Equals(message.FromUser)).Name;
                });
            }
        }
    }
}
