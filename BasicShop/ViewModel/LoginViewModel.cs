using BasicShop.Commands;
using BasicShop.Managers;
using BasicShop.Validators;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace BasicShop.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private MainWindowViewModel _mainVM;

        private Visibility _loadingScreen;

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public MaterialDesignThemes.Wpf.SnackbarMessageQueue MessageQueue { get; set; }
        public Visibility LoadingScreen
        {
            get { return _loadingScreen; }
            set
            {
                if (value == _loadingScreen) return;

                _loadingScreen = value;
                OnPropertyChanged("LoadingScreen");
            }
        }

        public SimpleCommand LoginCommand { get; set; }
        public SimpleCommand RegisterCommand { get; set; }

        public LoginViewModel(MainWindowViewModel mvm)
        {
            _mainVM = mvm;

            LoadingScreen = Visibility.Collapsed;

            LoginCommand = new SimpleCommand(Login);
            RegisterCommand = new SimpleCommand(Register);

            MessageQueue = new MaterialDesignThemes.Wpf.SnackbarMessageQueue(new TimeSpan(0, 0, 2));
            MessageQueue.IgnoreDuplicate = true;
        }

        private void Login()
        {
            if (Username == null || Username == string.Empty)
            {
                MessageQueue.Enqueue("Nazwa użytkownika nie może być pusta");
                return;
            }

            if (Password == null || Password == string.Empty)
            {
                MessageQueue.Enqueue("Hasło nie może być puste");
                return;
            }

            Username = Username.Trim();

            LoadingScreenProcess(() =>
            {
                if (!AccountManager.Login(Username, Password))
                    MessageQueue.Enqueue("Błędne hasło lub nazwa użytkownika!");
            }, () =>
            {
                if (AccountManager.LoggedId != null)
                    _mainVM.LoadPage("account");
            });
        }
        private void Register()
        {
            if (Firstname == null || Firstname == string.Empty)
            {
                MessageQueue.Enqueue("Imię nie może być puste");
                return;
            }
            if (Surname == null || Surname == string.Empty)
            {
                MessageQueue.Enqueue("Nazwisko nie może być puste");
                return;
            }
            if (Username == null || Username == string.Empty)
            {
                MessageQueue.Enqueue("Nazwa użytkownika nie może być pusta");
                return;
            }
            if (Email == null || Email == string.Empty)
            {
                MessageQueue.Enqueue("Email nie może być pusty");
                return;
            }
            if (Password == null || Password == string.Empty || RepeatPassword == null || RepeatPassword == string.Empty)
            {
                MessageQueue.Enqueue("Hasło nie może być puste");
                return;
            }
            if (Password != RepeatPassword)
            {
                MessageQueue.Enqueue("Hasła nie są identyczne");
                return;
            }

            Firstname = Firstname.Trim();
            Firstname = char.ToUpper(Firstname[0]) + Firstname.Substring(1);
            Surname = Surname.Trim();
            Surname = char.ToUpper(Surname[0]) + Surname.Substring(1);
            Email = Email.Trim();

            if (!StringValidator.ValidateEmail(Email))
            {
                MessageQueue.Enqueue("Email nie jest poprawny");
                return;
            }
            if (!StringValidator.ValidateName(Firstname))
            {
                MessageQueue.Enqueue("Imię nie jest poprawne");
                return;
            }
            if (!StringValidator.ValidateName(Surname))
            {
                MessageQueue.Enqueue("Nazwisko nie jest poprawne");
                return;
            }


            LoadingScreenProcess(() =>
            {
                string messOut = string.Empty;
                if (!AccountManager.Register(Username, Email, Password, Firstname, Surname, out messOut))
                    MessageQueue.Enqueue(messOut);
                else
                {
                    if (!AccountManager.Login(Username, Password))
                        MessageQueue.Enqueue("Wystąpił problem przy logowaniu");
                }
            }, () =>
            {
                _mainVM.LoadPage("account");
            });
        }
        private async void LoadingScreenProcess(Action action, Action onMain = null)
        {
            LoadingScreen = Visibility.Visible;

            await Task.Factory.StartNew(action).ContinueWith((Task) =>
            {
                LoadingScreen = Visibility.Collapsed;
            });

            if (onMain != null)
                onMain();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
