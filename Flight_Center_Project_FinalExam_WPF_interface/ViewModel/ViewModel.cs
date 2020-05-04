using Flight_Center_Project_FinalExam_BL;
using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Flight_Center_Project_FinalExam_WPF_interface
{
    public class ViewModel : INotifyPropertyChanged
    {        
        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand RelayCommand { get; set; }

        private FlightDAOMSSQL<Flight> _flightDAO = new FlightDAOMSSQL<Flight>();

        private LoginService<PocoBase> loginService = new LoginService<PocoBase>();

        private Flight _flight;
        public Flight Flight
        {
            get => _flight;
            set
            {
                if (_flight == value) return;
                _flight = value;
                OnPropertyChanged("Flight");
                
            }
        }

        private LoginToken<PocoBase> _token = null;
        private bool _isLoginSucseeded = false;
        public bool IsLoginSucseeded
        {
            get
            {
                return _isLoginSucseeded;
            }
            set
            {
                _isLoginSucseeded = value;
                NotifyPropertyChanged();

            }
        }

        private string _flightNumber;
        public string FlighNumber
        {
            get => _flightNumber;
            set
            {
                if (_flightNumber == value) return;
                _flightNumber = value;
                this.OnPropertyChanged("FlighNumber");
                this.RelayCommand.RaiseCanExecuteChanged();
            }
        }

        private void RaisePropertyChanged()
        {
            throw new NotImplementedException();
        }

        public ViewModel()
        {
            RelayCommand = new RelayCommand( (o) => 
            {



                Action act = () =>
                {
                    MessageBox.Show(FlighNumber);
                };
                ProcessExceptions(act);

                

            }
            , (o) => { return true; });
        }

        



        private void OnPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public Flight GetFlightByNumber(string IdStr)
        {
            if (!Int32.TryParse(IdStr, out int Id)) throw new InputIsNotANumberException(IdStr);
            
            var currentItem = _flightDAO.GetFlightById(Id);

            if (currentItem == new Flight()) throw new ItemWithThisIdIsntExistsException<Flight>(currentItem, Id);

            return currentItem;
        }

        private void NotifyPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsLoginSucseeded"));
        }

        public void ProcessExceptions(Action act)
        {
            try
            {
                act();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.GetType().Name}\n\n{ex.Message}\n\n\n{ex.StackTrace}");
            }
        }




    }
    
}
