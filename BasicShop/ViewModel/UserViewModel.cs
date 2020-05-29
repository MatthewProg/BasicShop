using BasicShop.Commands;
using BasicShop.Managers;
using BasicShop.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BasicShop.ViewModel
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private string _firstName;
        private string _surname;
        private string _email;
        private string _phone;
        private string _username;
        private string _emailMain;
        private string _oldPassword;
        private string _newPassword;
        private string _confirmPassword;
        private Visibility _loadingScreen;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (value == _firstName) return;

                _firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        public string Surname
        {
            get { return _surname; }
            set
            {
                if (value == _surname) return;

                _surname = value;
                OnPropertyChanged("Surname");
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                if (value == _email) return;

                _email = value;
                OnPropertyChanged("Email");
            }
        }
        public string Phone
        {
            get { return _phone; }
            set
            {
                if (value == _phone) return;

                _phone = value;
                OnPropertyChanged("Phone");
            }
        }
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
        public string EmailMain
        {
            get { return _emailMain; }
            set
            {
                if (value == _emailMain) return;

                _emailMain = value;
                OnPropertyChanged("EmailMain");
            }
        }
        public string OldPassword
        {
            get { return _oldPassword; }
            set
            {
                if (value == _oldPassword) return;

                _oldPassword = value;
                OnPropertyChanged("OldPassword");
            }
        }
        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                if (value == _newPassword) return;

                _newPassword = value;
                OnPropertyChanged("NewPassword");
            }
        }
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                if (value == _confirmPassword) return;

                _confirmPassword = value;
                OnPropertyChanged("ConfirmPassword");
            }
        }
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
        public MaterialDesignThemes.Wpf.SnackbarMessageQueue MessageQueue { get; set; }

        public SimpleCommand SaveCommand { get; set; }
        public SimpleCommand ChangePasswordCommand { get; set; }

        public UserViewModel()
        {
            MessageQueue = new MaterialDesignThemes.Wpf.SnackbarMessageQueue(new TimeSpan(0, 0, 4));
            MessageQueue.IgnoreDuplicate = true;

            LoadingScreen = Visibility.Collapsed;
            SaveCommand = new SimpleCommand(Save);
            ChangePasswordCommand = new SimpleCommand(ChangePassword);

            Refresh();
        }

        private void Refresh()
        {
            LoadingScreenProcess(() =>
            {
                LoadSettings();
            });
        }
        private void LoadSettings()
        {
            person p = new person();
            account a = new account();
            try
            {
                var dataContext = new shopEntities();
                a = dataContext.account.FirstOrDefault(y => y.account_id == AccountManager.LoggedId);
                p = dataContext.person.FirstOrDefault(x => x.person_id == a.person_id);
            }
            catch (Exception e)
            {
                string mess = "Podczas ładowania danych wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
            }

            FirstName = p.firstname;
            Surname = p.surname;
            Email = p.email;
            Phone = p.phone;

            Username = a.username;
            EmailMain = a.email;
        }
        private void Save()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                MessageQueue.Enqueue("Imię nie może być puste");
                return;
            }
            if (string.IsNullOrWhiteSpace(Surname))
            {
                MessageQueue.Enqueue("Nazwisko nie może być puste");
                return;
            }

            FirstName = FirstName.Trim();
            Surname = Surname.Trim();
            if (Email != null)
            {
                Email = Email.Trim();
                if (Email == string.Empty)
                    Email = null;
                else
                {
                    if (!StringValidator.ValidateEmail(Email))
                    {
                        MessageQueue.Enqueue("E-mail nie jest poprawny");
                        return;
                    }
                }
            }
            if (Phone != null)
            {
                Phone = Phone.Trim();
                if (Phone == string.Empty)
                    Phone = null;
                else
                {
                    if (!StringValidator.ValidatePhoneNumber(Phone))
                    {
                        MessageQueue.Enqueue("Telefon nie jest poprawny");
                        return;
                    }
                }
            }
            if(!StringValidator.ValidateName(FirstName))
            {
                MessageQueue.Enqueue("Imię nie jest poprawne");
                return;
            }
            if (!StringValidator.ValidateName(Surname))
            {
                MessageQueue.Enqueue("Nazwisko nie jest poprawne");
                return;
            }

            MessageQueue.Enqueue("Zapisywanie...");
            RunInBackground(() =>
            {
                try
                {
                    person p = new person();
                    var dataContext = new shopEntities();
                    p = dataContext.person.FirstOrDefault(x => x.person_id == dataContext.account.FirstOrDefault(y => y.account_id == AccountManager.LoggedId).person_id);
                    if (p != null)
                    {
                        p.firstname = FirstName;
                        p.surname = Surname;
                        p.email = Email;
                        p.phone = Phone;
                        dataContext.person.AddOrUpdate(p);
                        dataContext.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    string mess = "Podczas ładowania danych wystąpił błąd!\n";
                    StandardMessages.Error(mess + e.Message);
                }
            }, () => 
            {
                MessageQueue.Enqueue("Zapisano!");
            });
        }
        private void ChangePassword()
        {
            if(string.IsNullOrEmpty(OldPassword))
            {
                MessageQueue.Enqueue("Należy podać stare hasło!");
                return;
            }
            if (string.IsNullOrEmpty(NewPassword))
            {
                MessageQueue.Enqueue("Należy podać nowe hasło!");
                return;
            }
            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                MessageQueue.Enqueue("Należy powtórzyć hasło!");
                return;
            }

            if (NewPassword != ConfirmPassword)
            {
                MessageQueue.Enqueue("Hasła nie są takie same!");
                return;
            }

            MessageQueue.Enqueue("Zapisywanie..", null, null,null,false,false,new TimeSpan(0,0,2));
            RunInBackground(() =>
            {
                if (!AccountManager.ChangePassword(OldPassword, NewPassword))
                    MessageQueue.Enqueue("Podane hasło jest błędne!");
                else
                {
                    MessageQueue.Enqueue("Hasło zostało zmienione");
                    OldPassword = null;
                    NewPassword = null;
                    ConfirmPassword = null;
                }
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
        private async void RunInBackground(Action action, Action onMain = null)
        {
            await Task.Factory.StartNew(action);
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
