using System;
using System.Windows.Input;

namespace BasicShop.Commands
{
    public class SimpleCommand : ICommand
    {
        private Action _execute;

        public SimpleCommand(Action executeMethod)
        {
            _execute = executeMethod;
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
