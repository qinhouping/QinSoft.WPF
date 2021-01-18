using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace EMChat2.Common
{
    public static class TaskbarIconAttach
    {
        #region BalloonTip
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
            else
            {
                if (balloonTipInfo.Content != null && balloonTipInfo.Content.Length > 20)
                {
                    balloonTipInfo.Content = balloonTipInfo.Content.Substring(0, 20) + "...";
                }
                taskbarIcon.ShowBalloonTip(balloonTipInfo.Title, balloonTipInfo.Content, balloonTipInfo.Icon);
            }
        }
        #endregion


        #region IsFlash
        private static Dictionary<TaskbarIcon, FlashTimerInfo> TaskbarIconFlashTimers = new Dictionary<TaskbarIcon, FlashTimerInfo>();

        public static readonly DependencyProperty IsFlashProperty =
           DependencyProperty.RegisterAttached("IsFlash", typeof(bool), typeof(TaskbarIconAttach), new PropertyMetadata(OnIsFlashPropertyChangedCallback));
        public static void SetIsFlash(DependencyObject dp, bool value)
        {
            dp.SetValue(IsFlashProperty, value);
        }
        public static bool GetIsFlash(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsFlashProperty);
        }

        private static void OnIsFlashPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TaskbarIcon taskbarIcon = d as TaskbarIcon;
            if (taskbarIcon == null) return;
            bool isFlash = GetIsFlash(taskbarIcon);
            if (isFlash)
            {
                if (TaskbarIconFlashTimers.TryGetValue(taskbarIcon, out FlashTimerInfo flashTimerInfo))
                {
                    //移除
                    flashTimerInfo.Timer.Stop();
                    taskbarIcon.IconSource = flashTimerInfo.Image;
                    TaskbarIconFlashTimers.Remove(taskbarIcon);
                }
                else
                {
                    //创建
                    FlashTimerInfo newFlashTimerInfo = new FlashTimerInfo();
                    newFlashTimerInfo.Timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 500) };
                    newFlashTimerInfo.Timer.Tick += (sender, _event) =>
                    {
                        if (taskbarIcon.IconSource == null)
                            taskbarIcon.IconSource = newFlashTimerInfo.Image;
                        else
                            taskbarIcon.IconSource = null;
                    };
                    newFlashTimerInfo.Image = taskbarIcon.IconSource;
                    TaskbarIconFlashTimers.Add(taskbarIcon, newFlashTimerInfo);
                    newFlashTimerInfo.Timer.Start();
                }
            }
            else
            {
                if (TaskbarIconFlashTimers.TryGetValue(taskbarIcon, out FlashTimerInfo flashTimerInfo))
                {
                    //移除
                    flashTimerInfo.Timer.Stop();
                    taskbarIcon.IconSource = flashTimerInfo.Image;
                    TaskbarIconFlashTimers.Remove(taskbarIcon);
                }
            }
        }
        #endregion
    }
    public class BalloonTipInfo
    {
        public string Title { get; set; } = "系统提示";
        public string Content { get; set; }
        public BalloonIcon Icon { get; set; } = BalloonIcon.Info;
    }

    public class FlashTimerInfo
    {
        public DispatcherTimer Timer;
        public ImageSource Image;
    }
}
