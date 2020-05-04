using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flight_Center_Project_FinalExam_testingInterface
{
    class TestInterfaceBasePanel<T>: Panel where T : class, IPoco, new()
    {
        private DAO<T> _currentInControlDAO;      
        private delegate DAO<T> createAppropriateDAOInstance();
        private DAO<T> CreateAppropriateDAO()
        {

            Dictionary<Type, createAppropriateDAOInstance> pocoDaoCorrelation = new Dictionary<Type, createAppropriateDAOInstance>();
            pocoDaoCorrelation.Add(typeof(AirlineCompany), () => { return new AirlineDAOMSSQL<AirlineCompany>() as DAO<T>; });
            pocoDaoCorrelation.Add(typeof(Country), () => { return new CountryDAOMSSQL<Country>() as DAO<T>; });
            pocoDaoCorrelation.Add(typeof(Customer), () => { return new CustomerDAOMSSQL<Customer>() as DAO<T>; });
            pocoDaoCorrelation.Add(typeof(Flight), () => { return new FlightDAOMSSQL<Flight>() as DAO<T>; });
            pocoDaoCorrelation.Add(typeof(Ticket), () => { return new TicketDAOMSSQL<Ticket>() as DAO<T>; });
            pocoDaoCorrelation.Add(typeof(Administrator), () => { return new AdministratorDAOMSSQL<Administrator>() as DAO<T>; });
            pocoDaoCorrelation.Add(typeof(Utility_class_User), () => { return new Utility_class_UserDAOMSSQL<Utility_class_User>() as DAO<T>; });

            return pocoDaoCorrelation[typeof(T)]();

        }
        public TestInterfaceBasePanel(Point location)
        {
            _currentInControlDAO = CreateAppropriateDAO();

            this.Location = location;
            Initialize();

        }
        private void Initialize()
        {
            Button btnGetSelected = new Button();
            Button btnUpdateRecord = new Button();
            Button btnRemoveRecord = new Button();
            T selectedOne = null;

            TestInterfaceContainerPanel<T> currentTestInterfaceContainerPanel = new TestInterfaceContainerPanel<T>();

            var t = this.Parent;
            this.Height = 350;
            this.Width = 450;
            this.drawBorder(1, Color.Black);

            ComboBox cmbAllThePocos = new ComboBox();
            cmbAllThePocos.Items.AddRange(_currentInControlDAO.GetAll().Select(x => new ComboItem<T>(x)).ToArray());
            cmbAllThePocos.Location = new Point(5, 5);
            cmbAllThePocos.Width = 200;
            cmbAllThePocos.Text = $"All the {typeof(T).Name.Pluralize()} [GetAll]";
            
            currentTestInterfaceContainerPanel.Location = new Point(cmbAllThePocos.Location.X, cmbAllThePocos.Location.Y + cmbAllThePocos.Height + 5);
            int currentTestInterfaceContainerPanelLocationY = currentTestInterfaceContainerPanel.Location.Y;
            int currentTestInterfaceContainerPanelLocationX = currentTestInterfaceContainerPanel.Location.X;

            Label lblSelectedItem = new Label();
            lblSelectedItem.AutoSize = true;
            lblSelectedItem.Text = $"Selected {typeof(T).Name} will apear here";
            lblSelectedItem.Location = new Point(cmbAllThePocos.Location.X + cmbAllThePocos.Width + 15, cmbAllThePocos.Location.Y);
            lblSelectedItem.SizeChanged += (object sender, EventArgs e) => 
                {                    
                    currentTestInterfaceContainerPanel.Location = new Point(currentTestInterfaceContainerPanelLocationX, currentTestInterfaceContainerPanelLocationY + lblSelectedItem.Height - 15);
                    btnRemoveRecord.Location = new Point(currentTestInterfaceContainerPanel.Location.X, currentTestInterfaceContainerPanel.Location.Y + currentTestInterfaceContainerPanel.Height + 5);
                    this.Height = Screen.PrimaryScreen.WorkingArea.Height;
                    btnGetSelected.Visible = true;
                    btnUpdateRecord.Visible = true;


                };
            this.Controls.Add(lblSelectedItem);


            cmbAllThePocos.SelectedIndexChanged += (object sender, EventArgs e) =>
            {                 
                selectedOne = ((sender as ComboBox).SelectedItem as ComboItem<T>).Item;                
                lblSelectedItem.Text = selectedOne.ToString();

            };
            this.Controls.Add(cmbAllThePocos);
          
            //Selecting one item button
            btnGetSelected.Text = $"Get selected {typeof(T).Name} [Get]";
            btnGetSelected.Location = new Point(cmbAllThePocos.Location.X, cmbAllThePocos.Location.Y + cmbAllThePocos.Height + 2);
            btnGetSelected.AutoSize = true;
            btnGetSelected.Visible = false;
            btnGetSelected.Click += (object sender, EventArgs e) => 
                {
                    var selectedItem = _currentInControlDAO.Get((long)(cmbAllThePocos.SelectedItem as ComboItem<T>).Item.GetType().GetProperty("ID").GetValue((cmbAllThePocos.SelectedItem as ComboItem<T>).Item));
                    MessageBox.Show($"Selected {typeof(T).Name}:\n{selectedItem.ToString()}");
                };
            this.Controls.Add(btnGetSelected);
            //end: Selecting one item button

            //Updating button            
            btnUpdateRecord.AutoSize = true;
            btnUpdateRecord.Visible = false;
            btnUpdateRecord.Text = $"Update {typeof(T).Name} [Update]";
            btnUpdateRecord.Tag = -2;
            btnUpdateRecord.Click += (object sender, EventArgs e) =>
            {
                bool ifSucseeded = false;                
                try
                {
                    var tt = selectedOne;

                    if (_currentInControlDAO is UserBaseMSSQLDAO<T>)
                    {
                        (_currentInControlDAO as UserBaseMSSQLDAO<T>).Update(currentTestInterfaceContainerPanel.BuildAPocoWithIDOfAnotherPoco(selectedOne), currentTestInterfaceContainerPanel._txtUserName.Text, currentTestInterfaceContainerPanel._txtPasswprd.Text);//Add(BuildingAPoco(), _txtUserName.Text, _txtPasswprd.Text);
                    }
                    else _currentInControlDAO.Update(currentTestInterfaceContainerPanel.BuildAPocoWithIDOfAnotherPoco(selectedOne));                    
                    ifSucseeded = true;
                }
                catch (Exception ex)
                {
                    ifSucseeded = false;
                    MessageBox.Show($"{ex.GetType().Name}\n\n{ex.Message}\n\n{ex.StackTrace}");
                }
                string messageToUser = string.Empty;
                if (ifSucseeded) messageToUser = "The selected item was updated sucsessfully";
                else messageToUser = "Sorry, something went wrong. Adding filed ): ";
                MessageBox.Show(messageToUser);
            };
            btnUpdateRecord.Location = new Point(btnGetSelected.Location.X, btnGetSelected.Location.Y + btnGetSelected.Height + 2);
            this.Controls.Add(btnUpdateRecord);
            //end: Updating button

            //Remove Button
            btnRemoveRecord.AutoSize = true;
            btnRemoveRecord.Text = $"Delete {typeof(T).Name} [Remove]";
            btnRemoveRecord.Tag = -2;
            btnRemoveRecord.Click += (object sender, EventArgs e) =>
            {
                bool ifSucseeded = false;
                try
                {
                    _currentInControlDAO.Remove(selectedOne);
                    for(int i = 0; i < cmbAllThePocos.Items.Count; i++)
                        if ((cmbAllThePocos.Items[i] as ComboItem<T>).Item.Equals(selectedOne)) cmbAllThePocos.Items.Remove(cmbAllThePocos.Items[i]);
                    
                    ifSucseeded = true;
                }
                catch (Exception ex)
                {
                    ifSucseeded = false;
                    MessageBox.Show($"{ex.GetType().Name}\n\n{ex.Message}\n\n{ex.StackTrace}");
                }
                string messageToUser = string.Empty;
                if (ifSucseeded) messageToUser = "The selected item was removed sucsessfully";
                else messageToUser = "Sorry, something went wrong. Adding filed ): ";
                MessageBox.Show(messageToUser);
            };
            btnRemoveRecord.Location = new Point(currentTestInterfaceContainerPanel.Location.X, currentTestInterfaceContainerPanel.Location.Y + currentTestInterfaceContainerPanel.Height + 5);
            this.Controls.Add(btnRemoveRecord);
            //end: Remove Button



            this.Controls.Add(currentTestInterfaceContainerPanel);
            
        }

    }
}

