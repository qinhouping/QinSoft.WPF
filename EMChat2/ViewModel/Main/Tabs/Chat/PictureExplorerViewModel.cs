using EMChat2.Common;
using EMChat2.Model.Entity;
using EMChat2.Service;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace EMChat2.ViewModel.Main.Tabs.Chat
{
    public class PictureExplorerViewModel : PropertyChangedBase
    {
        #region 构造函数
        public PictureExplorerViewModel(IWindowManager windowManager, SystemService systemService, string[] sources, int currentIndex)
        {
            if (windowManager == null) throw new ArgumentNullException();
            if (sources.Length <= 0 || currentIndex < 0 || currentIndex >= sources.Length) throw new ArgumentOutOfRangeException();
            this.windowManager = windowManager;
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
                            try
                            {
                                HttpTools.DownloadAsync(this.CurrentSource, null, null).ContinueWith(task =>
                                {
                                    try
                                    {
                                        task.Result.StreamToFile(fileDialog.FileName);
                                        systemService.StoreUrlMapping(new UrlMappingInfo() { Url = this.CurrentSource, LocalFilePath = fileDialog.FileName });
                                        new Action(() => this.windowManager.ShowDialog(new AlertViewModel(windowManager, "保存成功", "提示", AlertType.Success))).ExecuteInUIThread();
                                    }
                                    catch (Exception e)
                                    {
                                        new Action(() => this.windowManager.ShowDialog(new AlertViewModel(windowManager, "保存失败" + e.Message, "提示", AlertType.Error))).ExecuteInUIThread();
                                    }
                                });
                            }
                            catch (Exception e)
                            {
                                new Action(() => this.windowManager.ShowDialog(new AlertViewModel(windowManager, "保存失败" + e.Message, "提示", AlertType.Error))).ExecuteInUIThread();
                            }
                        }
                        else
                        {
                            new Action(() =>
                            {
                                try
                                {
                                    this.CurrentSource.FileToStream().StreamToFile(fileDialog.FileName);
                                    new Action(() => this.windowManager.ShowDialog(new AlertViewModel(windowManager, "保存成功", "提示", AlertType.Success))).ExecuteInUIThread();
                                }
                                catch (Exception e)
                                {
                                    new Action(() => this.windowManager.ShowDialog(new AlertViewModel(windowManager, "保存失败" + e.Message, "提示", AlertType.Error))).ExecuteInUIThread();
                                }
                            }).ExecuteInTask();
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
        #endregion
    }
}
