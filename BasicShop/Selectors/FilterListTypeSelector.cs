using BasicShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BasicShop.Selectors
{
    public class FilterListTypeSelector : DataTemplateSelector
    {
        public DataTemplate CheckListTemplate { get; set; }
        public DataTemplate SliderListTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is CheckListViewModel)
                return CheckListTemplate;
            if (item is SliderListViewModel)
                return SliderListTemplate;
            return base.SelectTemplate(item,container);
        }
    }
}
