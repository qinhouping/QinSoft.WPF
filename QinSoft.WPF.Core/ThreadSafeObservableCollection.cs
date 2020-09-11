﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;

namespace QinSoft.WPF.Core
{
    /// <summary>
    /// 线程安全的ObservableCollection
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public class ThreadSafeObservableCollection<T> : ObservableCollection<T>
    {
        #region 构造函数
        public ThreadSafeObservableCollection() : base()
        {

        }
        public ThreadSafeObservableCollection(List<T> list) : base(list)
        {

        }
        public ThreadSafeObservableCollection(IEnumerable<T> collection) : base(collection)
        {

        }
        #endregion

        #region 重写ObservableCollection
        protected readonly ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        public override event NotifyCollectionChangedEventHandler CollectionChanged;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            //base.OnCollectionChanged(e);

            NotifyCollectionChangedEventHandler notifyCollectionChangedEventHandler = this.CollectionChanged;

            if (notifyCollectionChangedEventHandler != null)
            {
                foreach (NotifyCollectionChangedEventHandler handler in notifyCollectionChangedEventHandler.GetInvocationList())
                {
                    DispatcherObject dispatcherObject = handler.Target as DispatcherObject;

                    if (dispatcherObject != null)
                    {
                        Dispatcher dispatcher = dispatcherObject.Dispatcher;
                        if (dispatcher != null && !dispatcher.CheckAccess())
                        {
                            dispatcher.BeginInvoke((Action)(() => handler.Invoke(this,
                                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))),
                                DispatcherPriority.DataBind);

                            continue;
                        }
                    }

                    handler.Invoke(this, e);
                }
            }
        }

        protected override void SetItem(int index, T item)
        {
            lockSlim.EnterWriteLock();

            try
            {
                base.SetItem(index, item);
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            lockSlim.EnterUpgradeableReadLock();

            try
            {
                lockSlim.EnterWriteLock();

                try
                {
                    base.OnPropertyChanged(e);
                }
                finally
                {
                    lockSlim.ExitWriteLock();
                }
            }
            finally
            {
                lockSlim.ExitUpgradeableReadLock();
            }
        }

        protected override void InsertItem(int index, T item)
        {
            lockSlim.EnterWriteLock();

            try
            {
                base.InsertItem(index, item);
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        protected override void RemoveItem(int index)
        {
            lockSlim.EnterWriteLock();

            try
            {
                base.RemoveItem(index);
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            lockSlim.EnterWriteLock();

            try
            {
                base.MoveItem(oldIndex, newIndex);
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        protected override void ClearItems()
        {
            lockSlim.EnterWriteLock();

            try
            {
                base.ClearItems();
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        public override bool Equals(object obj)
        {
            if (!lockSlim.IsWriteLockHeld)
            {
                lockSlim.EnterReadLock();
            }

            try
            {
                return base.Equals(obj);
            }
            finally
            {
                if (!lockSlim.IsWriteLockHeld)
                {
                    lockSlim.ExitReadLock();
                }
            }
        }

        public override int GetHashCode()
        {
            if (!lockSlim.IsWriteLockHeld)
            {
                lockSlim.EnterReadLock();
            }

            try
            {
                return base.GetHashCode();
            }
            finally
            {
                if (!lockSlim.IsWriteLockHeld)
                {
                    lockSlim.ExitReadLock();
                }
            }
        }
        #endregion
    }
}