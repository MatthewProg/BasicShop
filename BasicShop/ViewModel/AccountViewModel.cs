using BasicShop.Commands;
using BasicShop.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicShop.ViewModel
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        private MainWindowViewModel _mainVM;
        private string _username;

        public string Username
        {
            get { return _username; }
            set
            {
                if (value == _username) return;

                _username = value;
                OnPropertyChanged("Username");
            }
        }
        public SimpleCommand LogoutCommand { get; set; }

        public AccountViewModel(MainWindowViewModel mvm)
        {
            _mainVM = mvm;

            LogoutCommand = new SimpleCommand(Logout);
            try
            {
                var dataContext = new shopEntities();
                Username = dataContext.account.FirstOrDefault(x => x.account_id == AccountManager.LoggedId).username;
            }
            catch(Exception e)
            {
                string mess = "Podczas ładowania informacji o koncie wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
            }
        }

        private void Logout()
        {
            AccountManager.Logout();
            _mainVM.LoadPage("account");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
