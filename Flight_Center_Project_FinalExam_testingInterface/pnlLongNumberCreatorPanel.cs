using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flight_Center_Project_FinalExam_testingInterface
{
    class pnlLongNumberCreatorPanel: Panel
    {
        private int _numberLength = 8;
        public int NumberOfCombos
        {
            get { return _numberLength; }
            set
            {
                _numberLength = value;
                this.Controls.Clear();
                this._comboBoxLocationX = 0;
                Initialize();
            }
        }

        private bool _randomInitialValue = true;
        public bool RandomInitialValue
        {
            get => _randomInitialValue;
            set
            {
                _randomInitialValue = value;
                this.Controls.Clear();
                this._comboBoxLocationX = 0;
                Initialize();
            }
        }
        public bool SplitToSections { get; set; } = true;
        
        private int _comboBoxLocationX = 0;
        private Size _innerComboSize = new Size(30, 21);
        private string[] _number;
        private Random _rnd = new Random();
        public string Number { get 
            {
                string str =  String.Join("", _number);
                string strToReturn = string.Empty;
                if (SplitToSections)
                {
                    for (int i = 0, count = 0; i < str.Length; i++)
                    {
                        count++;
                        strToReturn += str[i];
                        if (count == 3) { strToReturn += "-"; count = 0; }
                    }
                }
                else strToReturn = str;

                return strToReturn;
            }
        }

        public pnlLongNumberCreatorPanel()
        {
            Initialize();
        }
        public pnlLongNumberCreatorPanel(int numberLength)
        {
            _numberLength = numberLength;
            Initialize();
        }
        private void Initialize()
        {
            _number = new string[_numberLength];
            for (int i = 0; i < _numberLength; i++) _number[i] = "0";

            for (int i = 0; i < _numberLength; i++)
            {
                ComboBox cb = new ComboBox();
                cb.Location = new Point(_comboBoxLocationX, 0);              
                cb.Name = i.ToString();
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.MinimumSize = _innerComboSize;
                cb.Width = cb.MinimumSize.Width;
                cb.Height = cb.MinimumSize.Height;
                for (int ii = 0; ii <= 9; ii++) cb.Items.Add(ii);
                _comboBoxLocationX += cb.Width + 1;
                
                cb.SelectedIndexChanged += (object sender, EventArgs e) => 
                    {                        
                        _number[Convert.ToInt32((sender as ComboBox).Name)] = Convert.ToString((sender as ComboBox).SelectedIndex);
                    };
                if (RandomInitialValue) cb.SelectedIndex = _rnd.Next(0, 9);

                this.Controls.Add(cb);                
            }

            this.Width = _numberLength * _innerComboSize.Width + (_numberLength - 1);
            this.Height = _innerComboSize.Height;
        }

        public void RandomNumber()
        {
            foreach(var s in this.Controls)
            {
                if(s.GetType().Name.Equals("ComboBox")) (s as ComboBox).SelectedIndex = _rnd.Next(0, 9);
            }
        }

    }
}
