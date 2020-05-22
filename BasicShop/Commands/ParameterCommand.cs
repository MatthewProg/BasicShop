using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BasicShop.Commands
{
    public class ParameterCommand : ICommand
    {
        private Action<object> _execute;

        public ParameterCommand(Action<object> executeMethod)
        {
            _execute = executeMethod;
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
