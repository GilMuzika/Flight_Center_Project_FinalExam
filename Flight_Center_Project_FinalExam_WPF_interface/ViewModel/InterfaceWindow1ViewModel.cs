using Prism.Commands;
using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Threading;
using System.Dynamic;
using System.Windows.Controls;
using System.Windows.Data;
using System.Text.RegularExpressions;

namespace Flight_Center_Project_FinalExam_WPF_interface
{
    public class InterfaceWindow1ViewModel : INotifyPropertyChanged
    {
        #region Properties, fields and "On" methods
        private Flight _flight = null;
        private FlightDAOMSSQL<Flight> _currentFlightDAOMSSQL = new FlightDAOMSSQL<Flight>();


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            //if (name.Equals(this.GetType().GetProperty("TxtFlightProperties").Name)) MessageBox.Show(this.GetType().GetProperty(name).GetValue(this).ToString());
        }

        /// <summary>
        /// this property binded to the "txtFlightNumber" TextBox in the "InterfaceWindow1" window
        /// </summary>
        private string _flightNumber;
        public string FlightNumber
        {
            get => _flightNumber;
            set
            {
                if(!string.Equals(_flightNumber, value)) _flightNumber = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// this property is binded to the "tblCFlightProperties" TextBlock in the "InterfaceWindow1" window
        /// </summary>
        private string _txtFlightProperties;
        public string TxtFlightProperties
        {
            get => _txtFlightProperties;
            set
            {
                if (_txtFlightProperties == value) return;
                _txtFlightProperties = value;
                OnPropertyChanged();
            }
        }

        private ExpandoObject _flightExpando = new ExpandoObject();

        private ExpandoObject _actualFlightObj;
        public ExpandoObject ActualFlightObj
        {
            get => _actualFlightObj;
            set
            {
                if (_actualFlightObj == value) return;
                _actualFlightObj = value;
                OnPropertyChanged();
            }
        }

        private DataTemplate _pocoDataTemplate;
        public DataTemplate PocoDataTemplateProp
        {
            get => _pocoDataTemplate;
            set
            {
                if (_pocoDataTemplate == value) return;
                _pocoDataTemplate = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand BuyTicket_buttonClick_delegComm { get; set; }
        public DelegateCommand FlightNumber_buttonClick_delegComm { get; set; }

        #endregion Properties, fields and "On" methods

        #region Constructor
        public InterfaceWindow1ViewModel()
        {
            Initialize();
        }
        #endregion Constructor

        #region Methods
        private void Initialize()
        {           
            FlightNumber_buttonClick_delegComm = new DelegateCommand(FlightNumber_OnButtonClick, () => { return true; });
            BuyTicket_buttonClick_delegComm = new DelegateCommand(BuyTicket_OnButtonClick, CanExecute);

            Task.Run(() => 
            { 
                while(true)
                {
                    BuyTicket_buttonClick_delegComm.RaiseCanExecuteChanged();
                    Thread.Sleep(100);
                }
            });
        }

        private ExpandoObject createExpando<T>(T @object)
        {
            ExpandoObject expando = new ExpandoObject();

            AddProperty(expando, "----", $"{typeof(T).Name}'s properties:");
            foreach(var s in typeof(T).GetProperties())
            {
                AddProperty(expando, s.Name, $"{s.Name.Replace("_", " ")}: {s.GetValue(@object)}");
            }
            return expando;

        }
        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        private void FlightNumber_OnButtonClick()
        {
            
          

            if (Int32.TryParse(FlightNumber, out int fNum)) _flight = _currentFlightDAOMSSQL.Get(fNum);
            else { MessageBox.Show(InputIsNotANumberException.GetMessageWithoutThrowing(FlightNumber)); return; }

            if (_flight.Equals(new Flight())) { MessageBox.Show(ItemWithThisIdIsntExistsException<Flight>.GetMessageWithoutThrowing(fNum)); return; }

            ExpandoObject expandoFromFlight = createExpando(_flight);

            PocoDataTemplateProp = createPocoDataTemplateForContentPresenter(typeof(Flight));

            foreach (KeyValuePair<string, object> s in expandoFromFlight)
            {
                if(s.Key.Contains("REMAINING_TICKETS"))
                {
                    IDictionary<string, object> expDict = expandoFromFlight;
                    expDict[s.Key] = new NumToStringConverter().Convert(s.Value.ToString().Substring(s.Value.ToString().LastIndexOf(" "), s.Value.ToString().Length - s.Value.ToString().LastIndexOf(" ")), typeof(string), new object(), new System.Globalization.CultureInfo(0x00000C0A));
                }
            }

                ActualFlightObj = expandoFromFlight;
        }
        private void BuyTicket_OnButtonClick()
        {
            TicketDAOMSSQL<Ticket> localTicketDAOMSSQL = new TicketDAOMSSQL<Ticket>();

            ExpandoObject expandoFromTicket = createExpando(localTicketDAOMSSQL.GetByFlightID(_flight.ID));

            PocoDataTemplateProp = createPocoDataTemplateForContentPresenter(typeof(Ticket));

            ActualFlightObj = expandoFromTicket;

            MessageBox.Show($"Ticket for flight {_flight.ID}:\n\n {localTicketDAOMSSQL.GetByFlightID(_flight.ID).ToString()}");

        }
        private bool CanExecute()
        {
            if (_flight == null) return false;
            DateTime departureTime = _flight.DEPARTURE_TIME;
            if (departureTime <= DateTime.Now) return false;
            if (_flight.REMAINING_TICKETS <= 0) return false;

            return true;                       
        }











        public DataTemplate createPocoDataTemplateForContentPresenter(Type type)
        {
            //create the data template
            DataTemplate pocoDataTemplate = new DataTemplate();
            pocoDataTemplate.DataType = type;

            //set up the stack panel
            FrameworkElementFactory spFactory = new FrameworkElementFactory(typeof(StackPanel));
            spFactory.Name = "StackPanelForpocoDataTemplateFactory";
            spFactory.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);


            FrameworkElementFactory propertyBefore = new FrameworkElementFactory(typeof(Label));
            propertyBefore.SetBinding(Label.ContentProperty, new Binding("----"));
            propertyBefore.SetValue(Label.HeightProperty, 25d);
            propertyBefore.SetValue(Label.WidthProperty, 300d);
            propertyBefore.SetValue(Label.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            propertyBefore.SetValue(Label.VerticalAlignmentProperty, VerticalAlignment.Top);
            propertyBefore.SetValue(Label.MarginProperty, new Thickness(208, 27, 0, 0));
            spFactory.AppendChild(propertyBefore);

            double marginThicknessTop = 30;
            //foreach (var s in type.GetProperties())
            for(int i = 1; i < type.GetProperties().Length; i++)
            {
                FrameworkElementFactory property = new FrameworkElementFactory(typeof(Label));
                property.SetBinding(Label.ContentProperty, new Binding(type.GetProperties()[i].Name));
                //property.SetValue(Label.ContentProperty, s.GetValue(pocoObject));
                property.SetValue(Label.HeightProperty, 25d);
                property.SetValue(Label.WidthProperty, 300d);
                property.SetValue(Label.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                property.SetValue(Label.VerticalAlignmentProperty, VerticalAlignment.Top);
                property.SetValue(Label.MarginProperty, new Thickness(208, marginThicknessTop, 0, 0));
 
                spFactory.AppendChild(property);

                marginThicknessTop += 2;
            }

            //set the visual tree of the data template
            pocoDataTemplate.VisualTree = spFactory;

            return pocoDataTemplate;



        }
        #endregion Methods        



    }
}
