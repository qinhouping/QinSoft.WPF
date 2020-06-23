using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Runtime.CompilerServices;

namespace QinSoft.WPF.Control
{
    /// <summary>
    /// 多选下拉框
    /// </summary>
    [TemplatePart(Name = "PART_ListBox", Type = typeof(ListBox))]
    public class MCombobox : ComboBox
    {
        private ListBox DataListBox;
        public static readonly DependencyProperty SelectedItemsProperty =
           DependencyProperty.Register("SelectedItems", typeof(IList), typeof(MCombobox), new PropertyMetadata(new ObservableCollection<object>()));

        public IList SelectedItems
        {
            get
            {
                return this.GetValue(SelectedItemsProperty) as IList;
            }
            set
            {
                this.SetValue(SelectedItemsProperty, value);
            }
        }

        public static readonly DependencyProperty SeparatorProperty =
            DependencyProperty.Register("Separator", typeof(string), typeof(MCombobox), new PropertyMetadata(","));

        public string Separator
        {
            get
            {
                return this.GetValue(SeparatorProperty) as string;
            }
            set
            {
                this.SetValue(SeparatorProperty, value);
            }
        }

        static MCombobox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MCombobox), new FrameworkPropertyMetadata(typeof(MCombobox)));
        }

        public MCombobox()
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            DataListBox = GetTemplateChild("PART_ListBox") as ListBox;
            this.Text = GetText();
            DataListBox.SelectionChanged += (s, e) =>
            {
                this.Text = GetText();
            };
        }

        protected string GetText()
        {
            IList<string> data = new List<string>();
            if (this.SelectedItems != null)
            {
                foreach (object item in DataListBox.SelectedItems)
                {
                    Type type = item.GetType();
                    PropertyInfo propertyInfo = null;
                    if ((propertyInfo = type.GetProperty(this.DisplayMemberPath)) != null)
                    {
                        data.Add(Convert.ToString(propertyInfo.GetValue(item, null)));
                    }
                    else
                    {
                        data.Add(Convert.ToString(item));
                    }
                }
            }
            return string.Join(Separator, data.ToArray());
        }
    }
}
