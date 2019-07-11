using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace QinSoft.WPF.Themes.Default
{
    public class ValueConverters
    {
        public static IValueConverter BooleanToVisibilityConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    bool? v = value as bool?;
                    if (v == true)
                    {
                        return Visibility.Visible;
                    }
                    else
                    {
                        return Visibility.Collapsed;
                    }
                });
            }
        }

        public static IValueConverter AvgCornerRadiusConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    double v = Convert.ToDouble(value);
                    int avg = Convert.ToInt32(parameter);
                    return new CornerRadius((double)(v / avg));
                });
            }
        }
    }


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

}
