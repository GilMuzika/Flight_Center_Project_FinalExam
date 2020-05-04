using System;
using System.Collections.Generic;
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

namespace DataBase_Generator_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<long> _combosItemSource = new List<long>();       

        public MainWindow()
        {
            InitializeComponent();
            Initialize();
            
        }
        private void Initialize()
        {
            for(int i = 10; i <= 100; i++) _combosItemSource.Add(i);

            cmbAdministratorsNum.ItemsSource = _combosItemSource;
            cmbFlightsNum.ItemsSource = _combosItemSource;
            
        }
    }
}
