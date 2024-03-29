﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using QinSoft.Ioc;
using QinSoft.Ioc.Container;
using QinSoft.Ioc.Factory;
using QinSoft.Ioc.Scaner;

namespace QinSoft.WPF.Core
{
    public abstract class ApplicationBase
    {
        public IocApplicationContext IocApplicationContext { get; protected set; }

        public ApplicationBase()
        {
            this.OnInit();
        }

        public virtual void OnInit()
        {
            //初始化ioc容器
            this.IocApplicationContext = new IocApplicationContext()
                .RegisterObjectFactory<ObjectFactoryImp>()
                .RegisterDependencyInjectionScaner<AttributeDependencyInjectionScanerImp>()
                .BuildObjectContainer<ObjectContainerImp>();
        }

        public virtual void OnStartUp<T>(IDictionary<string, object> setting = null)
        {
            object viewModel = this.IocApplicationContext.ObjectContainer.Get(typeof(T));
            IWindowManager windowManager = this.IocApplicationContext.ObjectContainer.Get<WindowManagerImp>();
            windowManager.ShowWindow(viewModel, true, setting);
        }
    }
}
