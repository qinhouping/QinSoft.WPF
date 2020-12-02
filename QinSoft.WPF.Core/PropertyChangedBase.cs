using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace QinSoft.WPF.Core
{
    public abstract class PropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonIgnore]
        public bool IsNotifying { get; set; }

        public PropertyChangedBase()
        {
            this.IsNotifying = true;
        }

        public void NotifyPropertyChange(string propertyName)
        {
            if (!IsNotifying) return;
            PropertyChangedEventArgs propertyChangedEventArgs = new PropertyChangedEventArgs(propertyName);
            this.PropertyChanged?.Invoke(this, propertyChangedEventArgs);
            this.OnPropertyChanged(propertyChangedEventArgs);
        }

        public void NotifyPropertyChange<TProperty>(Expression<Func<TProperty>> property)
        {
            this.NotifyPropertyChange(GetMemberInfo(property).Name);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {

        }

        private static MemberInfo GetMemberInfo(Expression expression)
        {
            LambdaExpression lambdaExpression = (LambdaExpression)expression;
            MemberExpression memberExpression = (!(lambdaExpression.Body is UnaryExpression)) ? ((MemberExpression)lambdaExpression.Body) : ((MemberExpression)((UnaryExpression)lambdaExpression.Body).Operand);
            return memberExpression.Member;
        }
    }
}
