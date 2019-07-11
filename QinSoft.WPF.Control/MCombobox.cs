using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System;

namespace QinSoft.WPF.Control
{
    /// <summary>
    /// 多选下拉框
    /// </summary>
    [TemplatePart(Name = "PART_Text", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_ListBox", Type = typeof(ListBox))]
    public class MCombobox : ComboBox
    {
        private TextBox EditText { get; set; }

        private ListBox DataList { get; set; }

        public static readonly DependencyProperty SelectionModeProperty =
            DependencyProperty.Register("SelectionMode", typeof(SelectionMode), typeof(MCombobox), new PropertyMetadata(SelectionMode.Single));

        public SelectionMode SelectionMode
        {
            get
            {
                return (SelectionMode)this.GetValue(SelectionModeProperty);
            }
            set
            {
                this.SetValue(SelectionModeProperty, value);
            }
        }

        public IList SelectedItems
        {
            get
            {
                return this.DataList.SelectedItems;
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

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            EditText = GetTemplateChild("PART_Text") as TextBox;
            DataList = GetTemplateChild("PART_ListBox") as ListBox;

            DataList.SelectionChanged += DataList_SelectionChanged;
        }

        private void DataList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IList<string> data = new List<string>();
            foreach (object item in DataList.SelectedItems)
            {
                Type type = item.GetType();
                PropertyInfo propertyInfo = null;
                if (!string.IsNullOrEmpty(DisplayMemberPath) && (propertyInfo = type.GetProperty(this.DisplayMemberPath)) != null)
                {
                    data.Add(Convert.ToString(propertyInfo.GetValue(item, null)));
                }
                else
                {
                    data.Add(Convert.ToString(item));
                }
            }
            EditText.Text = string.Join(Separator, data.ToArray());
        }
    }

}
