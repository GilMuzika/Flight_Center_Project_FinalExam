using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Flight_Center_Project_FinalExam_WPF_interface
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<object> _method;
        private Func<object, bool> _canExecute;

        public RelayCommand(Action<object> method, Func<object, bool> canExecute)
        {
            this._method = method;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this._canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this._method(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
