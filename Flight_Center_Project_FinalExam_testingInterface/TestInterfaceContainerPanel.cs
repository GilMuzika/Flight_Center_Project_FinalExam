using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Flight_Center_Project_FinalExam_DAL;

namespace Flight_Center_Project_FinalExam_testingInterface
{
    class TestInterfaceContainerPanel<T>: Panel where T : class, IPoco, IUser, new()
    {        
        private DAO<T> _currentInControlDAO;

        public TextBox _txtPasswprd;
        public TextBox _txtUserName;

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


        private Dictionary<Type, createControl> _typeControlPair = new Dictionary<Type, createControl>();
        private delegate void createControl(string name, int count);
        private Random _rnd = new Random();
        private Point thisLocation { get; set; }

        private int count = 0;

        public TestInterfaceContainerPanel()
        {
            _currentInControlDAO = CreateAppropriateDAO();
            
            createControl createUpDown = new createControl((string name, int inLambdaCount) => 
                {
                    NumericUpDown locUpDown = new NumericUpDown();
                    locUpDown.Value = _rnd.Next((int)locUpDown.Minimum, (int)locUpDown.Maximum);
                    locUpDown.Tag = count;
                    locUpDown.Width = 50;
                    if(inLambdaCount == 0) locUpDown.Enabled = false;
                    Label propName = new Label();
                    propName.Tag = -2;
                    propName.AutoSize = true;
                    propName.Text = name;
                    propName.Location = new Point(5, 5 + 25 * count);
                    propName.Width = propName.Text.Length * 10;
                    locUpDown.Location = new Point(propName.Location.X + propName.Width + 10, 5 + 25 * count);                   
                    count++;                    
                    this.Controls.Add(propName);
                    this.Controls.Add(locUpDown);
                });
            _typeControlPair.Add(typeof(long), createUpDown);
            _typeControlPair.Add(typeof(int), createUpDown);

            createControl createTextBox = new createControl((string name, int inLambdaCount) => 
                {
                    TextBox locTextBox = new TextBox();
                    locTextBox.Text = Statics.GetUniqueKeyOriginal_BIASED(_rnd.Next(5, 10)).FirstLetterToupper();
                    locTextBox.Tag = count;
                    Label propName = new Label();
                    propName.Tag = -2;
                    propName.AutoSize = true;
                    propName.Text = name;
                    if (name == "Password") _txtPasswprd = locTextBox;
                    if (name == "UserName") _txtUserName = locTextBox;
                    propName.Location = new Point(5, 5 + 25 * count);
                    propName.Width = propName.Text.Length * 10;
                    locTextBox.Location = new Point(propName.Location.X + propName.Width + 10, 5 + 25 * count);
                    count++;
                    this.Controls.Add(propName);
                    this.Controls.Add(locTextBox);
                });
            _typeControlPair.Add(typeof(string), createTextBox);
            
            createControl createDateTimePicker = new createControl((string name, int inLambdaCount) => 
                {
                    DateTimePicker locPicker = new DateTimePicker();
                    locPicker.Tag = count;                    
                    locPicker.Value = new DateTime(_rnd.Next(locPicker.MinDate.Year, locPicker.MaxDate.Year), _rnd.Next(locPicker.MinDate.Month, locPicker.MaxDate.Month), _rnd.Next(locPicker.MinDate.Day, locPicker.MaxDate.Day));
                    Label propName = new Label();
                    propName.Tag = -2;
                    propName.AutoSize = true;
                    propName.Text = name;
                    propName.Location = new Point(5, 5 + 25 * count);
                    propName.Width = propName.Text.Length * 10;
                    locPicker.Location = new Point(propName.Location.X + propName.Width + 10, 5 + 25 * count);
                    count++;
                    this.Controls.Add(propName);
                    this.Controls.Add(locPicker);
                });
            _typeControlPair.Add(typeof(DateTime), createDateTimePicker);

            AddingToMyself();

        }

        public TestInterfaceContainerPanel(Point thisLocation) :this()
        {
            this.thisLocation = thisLocation;
            this.Location = thisLocation;
        }

        private void AddingToMyself()
        {
            int locCount = 0;
            foreach (PropertyInfo s in typeof(T).GetProperties())
            {
                _typeControlPair[s.PropertyType](s.Name, locCount);
                locCount++;
            }
            if (_currentInControlDAO is UserBaseMSSQLDAO<T>)
            {
                _typeControlPair[typeof(String)]("Password", locCount++);
                _typeControlPair[typeof(String)]("UserName", locCount++);
            }


            Button btnAddRecord = new Button();
            btnAddRecord.AutoSize = true;
            btnAddRecord.Text = $"Add {typeof(T).Name} [Add]";
            btnAddRecord.Tag = -2;
            btnAddRecord.Click += (object sender, EventArgs e) => 
                {
                    bool ifSucseeded = false;
                    try
                    {
                        if (_currentInControlDAO is UserBaseMSSQLDAO<T>)
                        {
                            (_currentInControlDAO as UserBaseMSSQLDAO<T>).Add(BuildingAPoco(), _txtUserName.Text, _txtPasswprd.Text);
                        }
                        else _currentInControlDAO.Add(BuildingAPoco());

                        foreach(var s in this.Controls)
                        {
                            if(s is TextBox) (s as TextBox).Text = Statics.GetUniqueKeyOriginal_BIASED(_rnd.Next(5, 10)).FirstLetterToupper();
                            if(s is NumericUpDown) (s as NumericUpDown).Value = _rnd.Next((int)(s as NumericUpDown).Minimum, (int)(s as NumericUpDown).Maximum); 
                            if(s is DateTimePicker) (s as DateTimePicker).Value = new DateTime(_rnd.Next((s as DateTimePicker).MinDate.Year, (s as DateTimePicker).MaxDate.Year), _rnd.Next((s as DateTimePicker).MinDate.Month, (s as DateTimePicker).MaxDate.Month), _rnd.Next((s as DateTimePicker).MinDate.Day, (s as DateTimePicker).MaxDate.Day));
                        }

                        ifSucseeded = true;
                    }
                    catch(Exception ex)
                    {
                        ifSucseeded = false;
                        MessageBox.Show($"{ex.GetType().Name}\n\n{ex.Message}\n\n{ex.StackTrace}");
                    }            
                    string messageToUser = string.Empty;
                    if (ifSucseeded) messageToUser = "The record was added sucsessfully";
                    else messageToUser = "Sorry, something went wrong. Adding filed ): ";
                    MessageBox.Show(messageToUser);
                };            
            btnAddRecord.Location = new Point(5, 5 + 25 * count);
            this.Controls.Add(btnAddRecord);

            int thisHeight = 0;
            foreach (var s in this.Controls)
            {
                if (s.GetType().Name != "Label")
                    thisHeight += (int)s.GetType().GetProperty("Height").GetValue(s) + 5;
            }

            this.Height = thisHeight + 10;
            this.Width = determiningThisLength();
            this.drawBorder(1, Color.Black);
        }
        /// <summary>
        /// this methos is builds a poco<T> object basing on the values that come from the Windows controls
        /// </summary>
        /// <returns></returns>
        public T BuildingAPoco()
        {
            //Buildong a poco.
            //in this section a poco<T> object is built basing on the values that come from the Windows controls
            T poco = new T();
            var propInfos = typeof(T).GetProperties();
            for (int i = 1; i < propInfos.Length; i++)
            {
                Control control = null;
                foreach (Control c in this.Controls) if ((int)c.Tag == i) control = c;

                Object controlValue = null;
                if (control is TextBox) controlValue = (control as TextBox).Text;
                if (control is NumericUpDown && propInfos[i].PropertyType.Name == "Int64") controlValue = Convert.ToInt64((control as NumericUpDown).Value);
                if (control is NumericUpDown && propInfos[i].PropertyType.Name == "Int32") controlValue = Convert.ToInt32((control as NumericUpDown).Value);
                if (control is DateTimePicker) controlValue = (control as DateTimePicker).Value;               

                propInfos[i].SetValue(poco, controlValue);
            }
            return poco;
        }
        public T BuildAPocoWithIDOfAnotherPoco(T anotherPoco)
        {
            var poco = BuildingAPoco();
            poco.GetType().GetProperties()[0].SetValue(poco, anotherPoco.GetType().GetProperties()[0].GetValue(anotherPoco));
            return poco;
        }
        /// <summary>
        /// This methos sets the Width of the whole Panel basing on the control located most right
        /// </summary>
        /// <returns></returns>        
        private int determiningThisLength()
        {
            bool switcher = true;
            List<int> controlLengthes = new List<int>();
            int temp = 0;
            foreach (Control s in this.Controls)
            {
                if (s is Label || s is NumericUpDown || s is DateTimePicker || s is TextBox ||s is Button)
                {
                    //s.MouseHover += new EventHandler((object sender, EventArgs e) => { MessageBox.Show(s.Location.X.ToString()); });
                    if (!switcher) temp = s.Location.X + s.Width;
                }
                if (!switcher) controlLengthes.Add(temp);
                if (switcher) switcher = false; else switcher = true;
            }
            var controlLengthesArr = controlLengthes.ToArray();
            Array.Sort(controlLengthesArr);
            return this.Width = controlLengthesArr[controlLengthesArr.Length - 1] + 5;
        }


    }
}
