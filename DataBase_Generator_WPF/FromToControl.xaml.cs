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
    /// Interaction logic for FromToControl.xaml
    /// </summary>
    public partial class FromToControl : UserControl
    {
        public long From { get; private set; }
        public long To { get; private set; }

        public long FromDependency
        {
            get => (long)this.GetValue(FromDependencyProperty);
            set => this.SetValue(FromDependencyProperty, value);
        }
        public static readonly DependencyProperty FromDependencyProperty =
        DependencyProperty.Register("FromDependency", typeof(long), typeof(FromToControl), new PropertyMetadata(0L));

        public long ToDependency
        {
            get => (long)this.GetValue(ToDependencyProperty);
            set => this.SetValue(ToDependencyProperty, value);
        }
        public static readonly DependencyProperty ToDependencyProperty =
        DependencyProperty.Register("ToDependency", typeof(long), typeof(FromToControl), new PropertyMetadata(0L));

        private List<long> _cmbSource = new List<long>();
        public FromToControl()
        {
            InitializeComponent();
            Initialize();
        }
        private void Initialize()
        {
            From = 10L;
            for(long i = From; i <= 100; i++)  _cmbSource.Add(i);            
            cmb1.ItemsSource = _cmbSource;
            cmb1.SelectionChanged += (object sender, SelectionChangedEventArgs e) => 
            {
                _cmbSource.Clear();
                From = (long)cmb1.SelectedItem;
                FromDependency = From;
                for (long i = From; i <= 100; i++) _cmbSource.Add(i);
                cmb2.ItemsSource = null;
                cmb2.ItemsSource = _cmbSource;
                
            };
            cmb2.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
            {
                if (cmb2.ItemsSource != null)
                {
                    To = (long)cmb2.SelectedItem;
                    ToDependency = To;
                }
            };
        }

        
    }
}
