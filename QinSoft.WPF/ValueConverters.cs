using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QinSoft.WPF
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

        public static IValueConverter ReverseBooleanToVisibilityConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    bool? v = value as bool?;
                    if (v == false)
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
        public static IValueConverter ReverseBooleanConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    bool? v = value as bool?;
                    if (v == false)
                    {
                        return true;
                    }
                    else if (v == true)
                    {
                        return false;
                    }
                    else
                    {
                        return null;
                    }
                });
            }
        }

        public static IValueConverter NotNullToVisibilityConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    if (value != null)
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

        public static IValueConverter NullToVisibilityConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    if (value == null)
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

        public static IValueConverter NotEmptyToVisibilityConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    string v = value as string;
                    if (!string.IsNullOrEmpty(v))
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

        public static IValueConverter EmptyToVisibilityConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    string v = value as string;
                    if (string.IsNullOrEmpty(v))
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

        public static IValueConverter NotZeroToVisibilityConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    int? v = value as int?;
                    if (v.HasValue && v != 0)
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

        public static IValueConverter ZeroToVisibilityConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    int? v = value as int?;
                    if (!v.HasValue || v == 0)
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

        public static IValueConverter EqualsToVisibilityConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    if (value?.Equals(parameter) == true)
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

        public static IValueConverter NotEqualsToVisibilityConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    if (value?.Equals(parameter) == false)
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

        public static IValueConverter EqualsToBooleanConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    return value?.Equals(parameter) == true;
                });
            }
        }

        public static IValueConverter NotEqualsToBooleanConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    return value?.Equals(parameter) == false;
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

        public static IValueConverter AvgConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    double v = Convert.ToDouble(value);
                    int avg = Convert.ToInt32(parameter);
                    return (int)(v / avg);
                });
            }
        }

        public static IValueConverter TimeToStringConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    DateTime? v = value as DateTime?;
                    string format = (parameter as string) ?? "yyyy-MM-dd HH:mm:ss";
                    if (v.HasValue)
                        return v.Value.ToString(format);
                    else
                        return string.Empty;
                });
            }
        }

        public static IValueConverter TimeToFriendlyStringConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    DateTime? v = value as DateTime?;
                    if (v.HasValue)
                    {
                        DateTime n = DateTime.Now;
                        //一天内
                        if ((n - v.Value).TotalDays < 1)
                        {
                            return v.Value.ToString("HH:mm");
                        }
                        //七天内
                        else if ((n - v.Value).TotalDays >= 1 && (n - v.Value).TotalDays < 7)
                        {
                            return v.Value.ToString("dd HH");
                        }
                        //一年内
                        else if ((n - v.Value).TotalDays >= 7 && (n - v.Value).TotalDays < 31)
                        {
                            return v.Value.ToString("MM-dd");
                        }
                        //超过一年
                        else
                        {
                            return v.Value.ToString("yy-MM");
                        }
                    }
                    else return string.Empty;
                });
            }
        }

        public static IValueConverter IntToFriendlyStringConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    int? v = value as int?;
                    if (v.HasValue) return v.Value > 99 ? 99 : v.Value;
                    else return string.Empty;
                });
            }
        }

        public static IValueConverter EnumToDescriptionConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    Enum enumValue = value as Enum;
                    if (enumValue == null)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        FieldInfo field = enumValue.GetType().GetField(value.ToString());
                        DescriptionAttribute[] descriptionAttributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
                        if (descriptionAttributes.Count() == 0) return string.Empty;
                        else return descriptionAttributes[0].Description;
                    }
                });
            }
        }

        public static IValueConverter DoubleToPercentageConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    double v = (double)value;
                    return (int)(v * 100) + "%";
                });
            }
        }

        public static IValueConverter UrlToImageConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    string url = value as string;
                    if (string.IsNullOrEmpty(url))
                        return null;
                    else
                    {
                        BitmapImage bitmapImage = new BitmapImage { CacheOption = BitmapCacheOption.None, CreateOptions = BitmapCreateOptions.DelayCreation | BitmapCreateOptions.IgnoreImageCache };
                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri(url);
                        bitmapImage.EndInit();
                        return bitmapImage;
                    }
                });
            }
        }

        public static IMultiValueConverter NotEqualsToVisibilityMultiConverter
        {
            get
            {
                return new DelegateMultiValueConverter((values, targetType, parameter, cultInfo) =>
                {
                    return values.Length == 2 && values[0]?.Equals(values[1]) == true ? Visibility.Collapsed : Visibility.Visible;
                });
            }
        }
    }


    public class DelegateValueConverter : IValueConverter
    {
        private Func<object, Type, object, CultureInfo, object> _convertFunc;
        private Func<object, Type, object, CultureInfo, object> _convertBackFunc;

        public DelegateValueConverter(Func<object, Type, object, CultureInfo, object> convertFunc, Func<object, Type, object, CultureInfo, object> convertBackFunc)
        {
            this._convertFunc = convertFunc;
            this._convertBackFunc = convertBackFunc;
        }

        public DelegateValueConverter(Func<object, Type, object, CultureInfo, object> convertFunc)
        {
            this._convertFunc = convertFunc;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (this._convertFunc != null)
                return this._convertFunc.Invoke(value, targetType, parameter, culture);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (this._convertBackFunc == null)
                return value;
            return this._convertBackFunc.Invoke(value, targetType, parameter, culture);
        }
    }
    public class DelegateMultiValueConverter : IMultiValueConverter
    {
        private Func<object[], Type, object, CultureInfo, object> _convertFunc;
        private Func<object, Type[], object, CultureInfo, object[]> _convertBackFunc;

        public DelegateMultiValueConverter(Func<object[], Type, object, CultureInfo, object> convertFunc, Func<object, Type[], object, CultureInfo, object[]> convertBackFunc)
        {
            this._convertFunc = convertFunc;
            this._convertBackFunc = convertBackFunc;
        }

        public DelegateMultiValueConverter(Func<object[], Type, object, CultureInfo, object> convertFunc)
        {
            this._convertFunc = convertFunc;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (this._convertFunc != null)
                return this._convertFunc.Invoke(values, targetType, parameter, culture);
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {

            if (this._convertBackFunc == null)
                return null;
            return this._convertBackFunc.Invoke(value, targetTypes, parameter, culture);
        }
    }

}
