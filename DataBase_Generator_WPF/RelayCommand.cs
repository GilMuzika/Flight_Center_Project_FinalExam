using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Windows.Threading;

namespace DataBase_Generator_WPF
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private Action<object> _method;
        private Func<object, bool> _canExecute;

        public RelayCommand(Action<object> method, Func<object, bool> canExecute)
        {
            this._method = method;
            this._canExecute = canExecute;
        }
        public RelayCommand(Action<object> method) : this(method, null)
        {            

        }



        public bool CanExecute(object parameter)
        {
            if (this._canExecute == null) return true;
            else return this._canExecute(null);
            
        }

        public void Execute(object parameter)
        {
            this._method(parameter);            
        }



        









    }
}
