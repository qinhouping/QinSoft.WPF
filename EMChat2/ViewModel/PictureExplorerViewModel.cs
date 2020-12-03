using EMChat2.Common;
using EMChat2.Model.BaseInfo;
using EMChat2.Service;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Controls;
using EMChat2.Event;

namespace EMChat2.ViewModel
{
    public class PictureExplorerViewModel : PropertyChangedBase, IDisposable
    {
        #region 构造函数
        public PictureExplorerViewModel(IWindowManager windowManager, EventAggregator eventAggregator, SystemService systemService, string[] sources, int currentIndex)
        {
            if (windowManager == null) throw new ArgumentNullException();
            if (sources.Length <= 0 || currentIndex < 0 || currentIndex >= sources.Length) throw new ArgumentOutOfRangeException();
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.systemService = systemService;
            this.Sources = sources;
            this.CurrentIndex = currentIndex;
        }

        public PictureExplorerViewModel(IWindowManager windowManager, string source)
        {
            if (windowManager == null || string.IsNullOrEmpty(source)) throw new ArgumentNullException();
            this.windowManager = windowManager;
            this.Sources = new string[1] { source };
            this.CurrentIndex = 0;
        }
        #endregion

        #region 属性
        private bool isMoving;
        private Point point;
        private string[] sources;
        public string[] Sources
        {
            get
            {
                return this.sources;
            }
            private set
            {
                this.sources = value;
                this.NotifyPropertyChange(() => this.Sources);
            }
        }
        public string CurrentSource
        {
            get
            {
                return this.Sources[this.currentIndex];
            }
        }
        private int currentIndex;
        public int CurrentIndex
        {
            get
            {
                return this.currentIndex;
            }
            set
            {
                this.currentIndex = value;
                this.NotifyPropertyChange(() => this.CurrentIndex);
                this.NotifyPropertyChange(() => this.CurrentSource);
            }
        }
        private double left = 0;
        public double Left
        {
            get
            {
                return this.left;
            }
            set
            {
                this.left = value;
                this.NotifyPropertyChange(() => this.Left);
            }
        }
        private double top = 0;
        public double Top
        {
            get
            {
                return this.top;
            }
            set
            {
                this.top = value;
                this.NotifyPropertyChange(() => this.Top);
            }
        }
        private double scale = 1;
        public double Scale
        {
            get
            {
                return this.scale;
            }
            private set
            {
                if (value < 0.5) value = 0.5;
                if (value > 2) value = 2;
                this.scale = value;
                this.NotifyPropertyChange(() => this.Scale);
            }
        }
        private double angle = 0;
        public double Angle
        {
            get
            {
                return this.angle;
            }
            private set
            {
                this.angle = value;
                this.NotifyPropertyChange(() => this.Angle);
            }
        }
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private SystemService systemService;
        #endregion

        #region 命令
        public ICommand CloseCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.windowManager.CloseWindow(this);
                });
            }
        }

        public ICommand PreviousCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.Left = 0;
                    this.Top = 0;
                    this.Angle = 0;
                    this.Scale = 1;
                    this.CurrentIndex--;
                }, () => this.currentIndex > 0);
            }
        }

        public ICommand NextCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.Left = 0;
                    this.Top = 0;
                    this.Angle = 0;
                    this.Scale = 1;
                    this.CurrentIndex++;
                }, () => this.currentIndex < this.Sources.Length - 1);
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    FileDialog fileDialog = new SaveFileDialog();
                    fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                    fileDialog.FileName = Path.GetFileName(this.CurrentSource);
                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        if (this.CurrentSource.IsNetUrl())
                        {
                            DownloadImage(fileDialog.FileName);
                        }
                        else
                        {
                            CopyImage(fileDialog.FileName);
                        }
                    }
                }, () => !string.IsNullOrEmpty(this.CurrentSource));
            }
        }

        public ICommand RepeatCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Angle += 90;
                });
            }
        }

        public void ScaleCommand(object sender, MouseWheelEventArgs e)
        {
            this.Scale += (double)e.Delta / 3000;
        }
        public void TranslateBeginCommand(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isMoving = true;
                point = e.GetPosition(sender as Image);
            }
        }
        public void TranslateEndCommand(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                isMoving = false;
                point = new Point();
            }
        }
        public void TranslateCommand(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (isMoving)
                {
                    Left += e.GetPosition(sender as Image).X - point.X;
                    Top += e.GetPosition(sender as Image).Y - point.Y;
                }
            }
        }
        #endregion

        #region 方法
        private async void DownloadImage(string filePath)
        {
            try
            {
                Stream stream = await HttpTools.DownloadAsync(this.CurrentSource, null, null);
                await new Action(() => stream.StreamToFile(filePath)).ExecuteInTask();
                systemService.StoreUrlMapping(new UrlMappingModel() { Url = this.CurrentSource, LocalFilePath = filePath });
                await this.eventAggregator.PublishAsync(new ShowBalloonTipEventArgs() { BalloonTip = new BalloonTipInfo() { Content = string.Format("图片[" + this.CurrentSource + "]下载完成") } });
            }
            catch (Exception e)
            {
                using (AlertViewModel alertViewModel = new AlertViewModel(this.windowManager, this.eventAggregator, "图片下载失败" + e.Message, "提示", AlertType.Error))
                {
                    new Action(() => this.windowManager.ShowDialog(alertViewModel)).ExecuteInUIThread();
                }
            }
        }

        private async void CopyImage(string filePath)
        {
            try
            {
                await new Action(() =>
                {
                    this.CurrentSource.FileToStream().StreamToFile(filePath);
                }).ExecuteInTask();
                await this.eventAggregator.PublishAsync(new ShowBalloonTipEventArgs() { BalloonTip = new BalloonTipInfo() { Content = string.Format("图片[" + this.CurrentSource + "]保存成功") } });
            }
            catch (Exception e)
            {
                using (AlertViewModel alertViewModel = new AlertViewModel(this.windowManager, this.eventAggregator, "保存图片失败" + e.Message, "提示", AlertType.Error))
                {
                    new Action(() => this.windowManager.ShowDialog(alertViewModel)).ExecuteInUIThread();
                }
            }
        }

        public void Dispose()
        {
            this.eventAggregator.Unsubscribe(this);
        }
        #endregion
    }
}
