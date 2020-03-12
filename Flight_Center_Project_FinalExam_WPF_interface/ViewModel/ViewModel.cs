using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Flight_Center_Project_FinalExam_WPF_interface
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private FlightDAOMSSQL<Flight> _flightDAO = new FlightDAOMSSQL<Flight>();

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



        
    }
}
