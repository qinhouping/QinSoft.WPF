using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace QinSoft.WPF.Theme.Default
{
    public class DelegateValueConverter : IValueConverter
    {
        private Func<object, Type, object, CultureInfo, object> _convertFunc;
        private Func<object, Type, object, CultureInfo, object> _convertBackFunc;

        public DelegateValueConverter(Func<object, Type, object, CultureInfo, object> convertFunc, Func<object, Type, object, CultureInfo, object> convertBackFunc)
            : this(convertFunc)
        {
            this._convertBackFunc = convertBackFunc;
        }

        public DelegateValueConverter(Func<object, Type, object, CultureInfo, object> convertFunc)
        {
            this._convertFunc = convertFunc;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (this._convertFunc != null)
                return this._convertFunc.Invoke(value, targetType, parameter, culture);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (this._convertBackFunc == null)
                return value;
            return this._convertBackFunc.Invoke(value, targetType, parameter, culture);
        }
    }

    public static class Converts
    {
        public static IValueConverter SolidColorBrushColorConverter = new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
        {
            if (value is SolidColorBrush)
                return (value as SolidColorBrush).Color;
            else
                return Colors.Black;
        });

        public static IValueConverter FlatButtonEllipseConvert = new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
        {
            FrameworkElement element = value as FrameworkElement;
            int count = Convert.ToInt32(parameter);
            return (element.ActualWidth > element.ActualHeight ? element.ActualWidth : element.ActualHeight) / count;
        });
    }
}
