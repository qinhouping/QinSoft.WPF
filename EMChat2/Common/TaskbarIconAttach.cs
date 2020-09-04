using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EMChat2.Common
{
    public static class TaskbarIconAttach
    {
        public static readonly DependencyProperty BalloonTipProperty =
            DependencyProperty.RegisterAttached("BalloonTip", typeof(BalloonTipInfo), typeof(TaskbarIconAttach), new PropertyMetadata(OnBalloonTipPropertyChangedCallback));
        public static void SetBalloonTip(DependencyObject dp, BalloonTipInfo value)
        {
            dp.SetValue(BalloonTipProperty, value);
        }
        public static BalloonTipInfo GetBalloonTip(DependencyObject dp)
        {
            return dp.GetValue(BalloonTipProperty) as BalloonTipInfo;
        }
        private static void OnBalloonTipPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TaskbarIcon taskbarIcon = d as TaskbarIcon;
            if (taskbarIcon == null) return;
            BalloonTipInfo balloonTipInfo = GetBalloonTip(taskbarIcon);
            if (balloonTipInfo == null) taskbarIcon.HideBalloonTip();
            else taskbarIcon.ShowBalloonTip(balloonTipInfo.Title, balloonTipInfo.Content, balloonTipInfo.Icon);
        }

    }
    public class BalloonTipInfo
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public BalloonIcon Icon { get; set; }
    }
}
