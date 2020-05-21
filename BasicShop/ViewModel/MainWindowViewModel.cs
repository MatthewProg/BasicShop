using BasicShop.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicShop.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private object _mainFrame;

        public object MainFrame
        {
            get { return _mainFrame; }
            set
            {
                if (value == _mainFrame) return;

                _mainFrame = value;
                OnPropertyChanged("MainFrame");
            }
        }
        public MainWindowViewModel()
        {
            var productList = new ProductList();
            (productList.DataContext as ProductListViewModel).CurrentSearch = null;
            (productList.DataContext as ProductListViewModel).CurrentCategoryId = 2;
            MainFrame = productList;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
