using Flight_Center_Project_FinalExam_BL;
using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Flight_Center_Project_FinalExam_WPF_interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ViewModel _viewModel = new ViewModel();

        private readonly LoginToken<IPoco> _token = null;
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


        private LoginService<Customer> loginService = new LoginService<Customer>();
      
        private FlyingCenterSystem _fcs = null;
        private IAnonymousUserFacade _facade = null;        

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {        
            string username = txtUserName.Text;
            string password = txtPassword.Text;
            Type userType = LoginServiceHelper_IUserTypeEvaluator.Evaluate(username, password);
            IsLoginSucseeded = loginService.TryUserLogin(username, password, out LoginToken<Customer> _token);            


            
            if (_token != null && _token.UserAsUser.USER_NAME == txtUserName.Text && _token.UserAsUser.PASSWORD == txtPassword.Text)
            {
                tblLogintextBlock.Text = "Congratulations!";
                _fcs = FlyingCenterSystem.GetInstance();
                _facade = GetProperFacade(userType);

            }
            else MessageBox.Show("איזה לוזר!");     
        }

        private IAnonymousUserFacade GetProperFacade(Type type)
        {
            IAnonymousUserFacade facade = null;
            Dictionary<Type, Action<Type>> correlation = new Dictionary<Type, Action<Type>>();
            correlation.Add(typeof(Customer), (Customer) => { facade =  _fcs.getFacede<LoggedInCustomerFacade>(); });
            correlation.Add(typeof(AirlineCompany), (AirlineCompany) => { facade = _fcs.getFacede<LoggedInAirlineFacade>(); });
            correlation.Add(typeof(Administrator), (Administrator) => { facade = _fcs.getFacede<LoggedInAdministratorFacade>(); });

            correlation[type](type);
            return facade;
        }

        private void NotifyPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsLoginSucseeded"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Action act = () =>
            {
                lblFlyProperties.Content = _viewModel.GetFlightByNumber(txtFlightNumber.Text).ToString();
            };
            ProcessExceptions(act);                      
        }

        private void ProcessExceptions(Action act)
        {
            try
            {
                act();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"{ex.GetType().Name}\n\n{ex.Message}\n\n\n{ex.StackTrace}");
            }
        }
    }
}
