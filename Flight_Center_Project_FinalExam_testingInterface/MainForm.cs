using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Flight_Center_Project_FinalExam_DAL;
using Newtonsoft.Json;

namespace Flight_Center_Project_FinalExam_testingInterface
{
    public partial class MainForm : Form
    {
        private CustomerDAOMSSQL<Customer> _currentCustomersDAO = new Flight_Center_Project_FinalExam_DAL.CustomerDAOMSSQL<Customer>();        

        public MainForm()
        {
            InitializeComponent();
            Initialize();
            
        }
        private void Initialize()
        {
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 200;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 + this.Width / 2, 0);
            this.Controls.Add(new TestInterfaceBasePanel<Utility_class_User>(new Point(0, 0)));
            this.Controls.Add(new TestInterfaceBasePanel<Customer>(new Point(470, 0)));
        }
        private void ShowAll()
        {
            string str = string.Empty;
            foreach(var s in _currentCustomersDAO.GetAll())
            {
                str += JsonConvert.SerializeObject(s) + Environment.NewLine;
            }
            MessageBox.Show(str);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            this.ShowAll();
        }
    }
}
