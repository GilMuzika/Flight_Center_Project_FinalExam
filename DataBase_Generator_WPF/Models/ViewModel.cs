using Flight_Center_Project_FinalExam_BL;
using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase_Generator_WPF
{
    class ViewModel : ViewModelBase
    {
        //private List<Flight_Center_Project_FinalExam_DAL.Country> _allCounries = new List<Flight_Center_Project_FinalExam_DAL.Country>();

        private DataInterface<Customer> _dataInterfaceCustomer = new DataInterface<Customer>();
        private DataInterface<Administrator> _dataInterfaceAdministrator = new DataInterface<Administrator>();
        private DataInterface<Flight> _dataInterfaceFlight = new DataInterface<Flight>();
        private DataInterface<AirlineCompany> _dataInterfaceAirlineCompany = new DataInterface<AirlineCompany>();
        private DataInterface<Country> _dataInterfaceCountry = new DataInterface<Country>();
        private DataInterface<Ticket> _dataInterfaceTicket = new DataInterface<Ticket>();



        private long _minCustomersNum;
        public long MinCustomersNum
        {
            get => _minCustomersNum;
            set
            {
                _minCustomersNum = value;
                OnPropertyChanged();
            }
        }
        private long _maxCustomersNum;
        public long MaxCustomersNum
        {
            get => _maxCustomersNum;
            set
            {
                _maxCustomersNum = value;
                OnPropertyChanged();
            }
        }
        private long _fixedAdministratorsNum;
        public long FixedAdministratorsNum
        {
            get => _fixedAdministratorsNum;
            set
            {
                _fixedAdministratorsNum = value;
                OnPropertyChanged();
            }
        }
        private long _fixedAirlinesNum;
        public long FixedAirlinesNum
        {
            get => _fixedAirlinesNum;
            set
            {
                _fixedAirlinesNum = value;
                OnPropertyChanged();
            }
        }
        private long _minAirlinesNum;
        public long MinAirlinesNum
        {
            get => _minAirlinesNum;
            set
            {
                _minAirlinesNum = value;
                OnPropertyChanged();
            }
        }
        private long _maxAirlinesNum;
        public long MaxAirlinesNum
        {
            get => _maxAirlinesNum;
            set
            {
                _maxAirlinesNum = value;
                OnPropertyChanged();
            }
        }


        private long _fixedFlightsNum;
        public long FixedFlightsNum
        {
            get => _fixedFlightsNum;
            set
            {
                _fixedFlightsNum = value;
                OnPropertyChanged();
            }
        }
        private long _maxTicketsNum;
        public long MaxTicketsNum
        {
            get => _maxTicketsNum;
            set
            {
                _maxTicketsNum = value;
                OnPropertyChanged();
            }
        }
        private long _minTicketsNum;
        public long MinTicketsNum
        {
            get => _minTicketsNum;
            set
            {
                _minTicketsNum = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ButtonCllick_RelayComm { get; set; }
        public RelayCommand AddAllCountriesButtonClick_RelayComm { get; set; }

        public ViewModel()
        {
            ButtonCllick_RelayComm = new RelayCommand(TacklingButtonClick, CanExecute);
            AddAllCountriesButtonClick_RelayComm = new RelayCommand(AddAllCountriesButtonClick, CanExecute);
        }
        private void TacklingButtonClick(object o)
        {
            _dataInterfaceCustomer.Add(MinCustomersNum, MaxCustomersNum, -1);
            _dataInterfaceAdministrator.Add(-1, -1, FixedAdministratorsNum);
            _dataInterfaceFlight.Add(-1, -1, FixedFlightsNum);
            _dataInterfaceAirlineCompany.Add(MinAirlinesNum, MaxAirlinesNum, -1);
            _dataInterfaceTicket.Add(MinTicketsNum, MaxTicketsNum, -1);
        }
        private void AddAllCountriesButtonClick(object o)
        {
            _dataInterfaceCountry.Add(-1, -1, -1);
        }
        private bool CanExecute(object o)
        {
            return true;
        }
    }
}
