using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicShop.ViewModel
{
    public class CheckoutViewModel
    {
        private MainWindowViewModel _mainVM;

        public CheckoutViewModel(MainWindowViewModel mvm)
        {
            _mainVM = mvm;
        }
    }
}
