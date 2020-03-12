using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Center_Project_FinalExam_DAL
{   
    public abstract class DAO<T>: IBasicDB<T> where T : class, IPoco, new()
    {
        protected SqlConnection _connection = new SqlConnection();
        protected SqlCommand _command = new SqlCommand();

        protected AirlineDAOMSSQL<AirlineCompany> _currentAirlineDAOMSSQL;
        protected CountryDAOMSSQL<Country> _currentCountryDAOMSSQL;
        protected CustomerDAOMSSQL<Customer> _currentCustomerDAOMSSQL;
        protected FlightDAOMSSQL<Flight> _currentFlightDAOMSSQL;
        protected TicketDAOMSSQL<Ticket> _currentTicketDAOMSSQL;
        protected AdministratorDAOMSSQL<Administrator> _currentAdministratorDAOMSSQL;
        protected Utility_class_UserDAOMSSQL<Utility_class_User> _currentUserDAOMSSQL;


        private delegate void createDAOInstance();
        
        public DAO()
        {
            _command.CommandType = System.Data.CommandType.StoredProcedure;
            _command.Connection = _connection;

            this.SetConnectionString();
        }

        #region private and protected methods for DAL internal usage
        private void GetServerAndInstanceNames(out string serverName, out string instanceName)
        {
            string servername = string.Empty;
            string instancename = string.Empty;
            var table = SqlDataSourceEnumerator.Instance.GetDataSources();

            string str = string.Empty;
            foreach (DataRow row in table.AsEnumerable())
            {
                if (!(row["ServerName"] is DBNull)) servername = (string)row["ServerName"];
                if (!(row["InstanceName"] is DBNull)) servername = "\\" + (string)row["InstanceName"];
            }
            serverName = servername;
            instanceName = instancename;
        }
        protected void SetConnectionString_back()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);


            //string connStrAddition = "\\SQLEXPRESS";
            //if (Environment.MachineName.ToLower().Contains("muzika")) connStrAddition = string.Empty;
            //string connStr = $"Data Source={Environment.MachineName}{connStrAddition};Initial Catalog=The_very_important_Flight_Center_Project;Integrated Security=True";

            GetServerAndInstanceNames(out string sqlServerName, out string SqlServerInstanceName);

            string connStr = $"Data Source={sqlServerName}{SqlServerInstanceName};Initial Catalog=The_very_important_Flight_Center_Project;Integrated Security=True";
            config.ConnectionStrings.ConnectionStrings[0].ConnectionString = connStr;
            config.ConnectionStrings.ConnectionStrings[0].ProviderName = "System.Data.SqlClient";
            config.Save(ConfigurationSaveMode.Modified);

            _connection.ConnectionString = config.ConnectionStrings.ConnectionStrings[0].ConnectionString;            

        }
        protected void SetConnectionString()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);


            //string connStrAddition = @"\SQLEXPRESS"; - for class machines
            string connStrAddition = "";
            //if (Environment.MachineName.ToLower().Contains("muzika")) connStrAddition = string.Empty;
            //string connStr = $"Data Source={Environment.MachineName}{connStrAddition};Initial Catalog=The_very_important_Flight_Center_Project;Integrated Security=True";

            //GetServerAndInstanceNames(out string sqlServerName, out string SqlServerInstanceName);

            string connStr = $"Data Source={Environment.MachineName}{connStrAddition};Initial Catalog=The_very_important_Flight_Center_Project;Integrated Security=True";
            config.ConnectionStrings.ConnectionStrings[0].ConnectionString = connStr;
            config.ConnectionStrings.ConnectionStrings[0].ProviderName = "System.Data.SqlClient";
            config.Save(ConfigurationSaveMode.Modified);

            _connection.ConnectionString = config.ConnectionStrings.ConnectionStrings[0].ConnectionString;

        }
        /// <summary>
        /// Gets all the table names.
        /// because this function is intended to be used bu other functions, 
        /// it's don't need to acsess the connection.
        /// </summary>  
        private List<string> GetAllTableNames()
        {
            List<string> tableNames = new List<string>();            
            var schema = _connection.GetSchema("Tables");
            foreach (DataRow s in schema.Rows)
            {
                string tablename = (string)s[2];
                tableNames.Add(tablename);
            }
            return tableNames;
        }
        protected string GetTableName(Type pocoType)
        {
            string tableName = string.Empty;
            foreach (var s in this.GetAllTableNames())
            {
                if (s.Contains(pocoType.Name.ChopCharsFromTheEnd(1))) tableName = s;
            }
            return tableName;
        }
        public T GetSomethingBySomethingInternal(object identifier, int propertyNumber)
        {
            try
            {
                _connection.Open();
                T something = new T();
                string tableName = this.GetTableName(typeof(T));
                string relevantColumnName = something.GetType().GetProperties()[propertyNumber].Name;
                string identifierInQMarks = string.Empty;
                if (identifier.GetType().Name == "String") identifierInQMarks = $"'{identifier}'";
                else identifierInQMarks = identifier.ToString();
                //_command.CommandText = $"SELECT * FROM {tableName} WHERE {relevantColumnName} = {identifierInQMarks}";
                _command.CommandText = "DAO_BASE_GetSomethingBySomethingInternal_METHOD_QUERY";

                _command.Parameters.Clear();
                _command.Parameters.AddWithValue("TABLE_NAME", tableName);
                _command.Parameters.AddWithValue("RELEVANT_COLUMN_NAME", relevantColumnName);
                _command.Parameters.AddWithValue("COLUMN_IDENTIFIER", identifierInQMarks);

                using (SqlDataReader reader = _command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            typeof(T).GetProperties()[i].SetValue(something, reader.GetValue(i));
                        }
                    }
                }
                return something;

            }
            finally { _connection.Close(); }
        }

        public List<T> GetManyBySomethingInternal(object identifier, int propertyNumber)
        {
            try
            {
                _connection.Open();
                List<T> manyOfSomething = new List<T>();
                string tableName = this.GetTableName(typeof(T));
                string relevantColumnName = typeof(T).GetProperties()[propertyNumber].Name;
                string identifierInQMarks = string.Empty;
                if (identifier.GetType().Name == "String") identifierInQMarks = $"'{identifier}'";
                else identifierInQMarks = identifier.ToString();
                //_command.CommandText = $"SELECT * FROM {tableName} WHERE {relevantColumnName} = {identifierInQMarks}";

                _command.Parameters.Clear();
                _command.CommandText = "DAO_BASE_GetManyBySomethingInternal_METHOD_QUERY";
                _command.Parameters.AddWithValue("TABLE_NAME", tableName);
                _command.Parameters.AddWithValue("RELEVANT_COLUMN_NAME", relevantColumnName);
                _command.Parameters.AddWithValue("COLUMN_IDENTIFIER", identifierInQMarks);

                using (SqlDataReader reader = _command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        T something = new T();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            typeof(T).GetProperties()[i].SetValue(something, reader.GetValue(i));
                        }
                        manyOfSomething.Add(something);
                    }
                }
                return manyOfSomething;

            }
            finally { _connection.Close(); }
        }
        protected T GetSomethingInOneTableBySomethingInAnotherInternal(object byWhatInOneTable, int anotherPocoTypePropertyNumber, Type anotherPocoType)
        {
            try
            {
                _connection.Open();
                T something = new T();
                string tableName = this.GetTableName(typeof(T));
                string anothertablename = this.GetTableName(anotherPocoType);
                string relevantColumnName = something.GetType().GetProperties()[anotherPocoTypePropertyNumber].Name;
                string relevantColumnNameInAnotherTable = anotherPocoType.GetProperties()[anotherPocoTypePropertyNumber].Name;
                string identifyer_ByWhatInQMarks = string.Empty;
                if (byWhatInOneTable.GetType().Name == "String") identifyer_ByWhatInQMarks = $"'{byWhatInOneTable}'";
                else identifyer_ByWhatInQMarks = byWhatInOneTable.ToString();

                string leftSideOfONStatement = "USER_ID";
                string rightSideOfONStatement = "ID";
                if (typeof(T).Name == "Utility_class_User")
                {
                    leftSideOfONStatement = "ID";
                    rightSideOfONStatement = "USER_ID";
                }

                //_command.CommandText = $"SELECT {tableName}.* FROM {tableName} JOIN {anothertablename} ON {tableName}.{leftSideOfONStatement} = {anothertablename}.{rightSideOfONStatement} WHERE {anothertablename}.{relevantColumnNameInAnotherTable} = {identifyer_ByWhatInQMarks}";
                _command.CommandText = "DAO_BASE_GetSomethingInOneTableBySomethingInAnotherInternal_METHOD_QUERY";

                _command.Parameters.Clear();
                _command.Parameters.AddWithValue("TABLE_NAME", tableName);
                _command.Parameters.AddWithValue("ANOTHER_TABLE_NAME", anothertablename);
                _command.Parameters.AddWithValue("LEFT_SIDE_OF_ONSTATEMENT", leftSideOfONStatement);
                _command.Parameters.AddWithValue("RIGHT_SIDE_OF_ONSTATEMENT", rightSideOfONStatement);
                _command.Parameters.AddWithValue("RELEVANT_COLUMN_NAME_IN_ANOTHER_TABLE", relevantColumnNameInAnotherTable);
                _command.Parameters.AddWithValue("COLUMN_IDENTIFIER", identifyer_ByWhatInQMarks);

                using (SqlDataReader reader = _command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            typeof(T).GetProperties()[i].SetValue(something, reader.GetValue(i));
                        }
                    }
                }
                return something;

            }
            finally
            { _connection.Close(); }
        }                
        protected void SetAnotherDAOInstance(Type daoType)
        {            
            Dictionary<Type, createDAOInstance> instanceTypeCorrelation = new Dictionary<Type, createDAOInstance>();
            instanceTypeCorrelation.Add(typeof(AirlineCompany), () => { _currentAirlineDAOMSSQL = new AirlineDAOMSSQL<AirlineCompany>(); });
            instanceTypeCorrelation.Add(typeof(Country), () => { _currentCountryDAOMSSQL = new CountryDAOMSSQL<Country>(); });
            instanceTypeCorrelation.Add(typeof(Customer), () => { _currentCustomerDAOMSSQL = new CustomerDAOMSSQL<Customer>(); });
            instanceTypeCorrelation.Add(typeof(Flight), () => { _currentFlightDAOMSSQL = new FlightDAOMSSQL<Flight>(); });
            instanceTypeCorrelation.Add(typeof(Ticket), () => { _currentTicketDAOMSSQL = new TicketDAOMSSQL<Ticket>(); });
            instanceTypeCorrelation.Add(typeof(Utility_class_User), () => { _currentUserDAOMSSQL = new Utility_class_UserDAOMSSQL<Utility_class_User>(); });
            instanceTypeCorrelation.Add(typeof(Administrator), () => { _currentAdministratorDAOMSSQL = new AdministratorDAOMSSQL<Administrator>(); });

            instanceTypeCorrelation[daoType]();
        }

        #endregion

        #region Public service methods 
        public List<T> GetAll()
        {            
            try
            {
                List<T> toReturn = new List<T>();
                _connection.Open();
                //_command.CommandText = $"select * from {GetTableName(typeof(T))}";
                _command.CommandText = "DAO_BASE_GetAll_METHOD_QUERY";

                _command.Parameters.Clear();
                _command.Parameters.AddWithValue("TABLE_NAME", GetTableName(typeof(T)));
                using (SqlDataReader reader = _command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        T poco = new T();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            object value = reader[i];
                            if (reader[i] is DBNull && typeof(T).GetProperties()[i].GetType().Name.ToLower().Equals("string"))
                            {
                                typeof(T).GetProperties()[i].SetValue(poco, string.Empty);
                            }
                            if (reader[i] is DBNull && typeof(T).GetProperties()[i].GetType().Name.ToLower().Contains("int"))
                            {
                                typeof(T).GetProperties()[i].SetValue(poco, 0);
                            }

                            if (!(reader[i] is DBNull)) { typeof(T).GetProperties()[i].SetValue(poco, value); }

                        }
                        toReturn.Add(poco);
                    }
                }
                return toReturn;
            }
            finally { _connection.Close(); }
            
            
        }


        public virtual void Add(T poco)
        {
            _connection.Open();
            string tableName = GetTableName(typeof(T));
            try
            {
                _command.CommandType = CommandType.Text;
                _command.CommandText = $"INSERT INTO {tableName} ({typeof(T).GetProperties()[1].Name}) VALUES ('{typeof(T).GetProperties()[1].GetValue(poco)}') SELECT SCOPE_IDENTITY()";
                /*_command.CommandText = "DAO_BASE_Add_METHOD_QUERY_for_insert";
                _command.Parameters.AddWithValue("TABLE_NAME", tableName);
                var name = typeof(T).GetProperties()[1].Name;
                var value = typeof(T).GetProperties()[1].GetValue(poco);
                _command.Parameters.AddWithValue("SECOND_COLUMN_NAME", name);
                _command.Parameters.AddWithValue("SECOND_COLUMN_VALUE", value);*/

                var IDvalue = Convert.ToInt64(_command.ExecuteScalar());
                poco.GetType().GetProperties()[0].SetValue(poco, IDvalue);

                for (int i = 2; i < typeof(T).GetProperties().Length; i++)
                {
                    _command.CommandText = $"UPDATE {tableName} SET {typeof(T).GetProperties()[i].Name} = '{typeof(T).GetProperties()[i].GetValue(poco)}' WHERE {typeof(T).GetProperties()[0].Name} = '{typeof(T).GetProperties()[0].GetValue(poco)}'";
                    /*_command.CommandText = "DAO_BASE_Add_METHOD_QUERY_for_update";
                    _command.Parameters.AddWithValue("TABLE_NAME", tableName);
                    _command.Parameters.AddWithValue("SUBSEQUENT_COLUMN_NAME", typeof(T).GetProperties()[i].Name);
                    _command.Parameters.AddWithValue("SUBSEQUENT_COLUMN_VALUE", typeof(T).GetProperties()[i].GetValue(poco));
                    _command.Parameters.AddWithValue("FIRST_COLUMN_NAME", typeof(T).GetProperties()[0].Name);
                    _command.Parameters.AddWithValue("FIRST_COLUMN_VALUE", typeof(T).GetProperties()[0].GetValue(poco));*/
                    _command.ExecuteNonQuery();
                }
            }
            finally { _connection.Close(); }
        }

        public T Get(long ID)
        {
            
            try
            {
                T poco = new T();
                _connection.Open();
                string tableName = this.GetTableName(typeof(T));                
                string firstColumnName = poco.GetType().GetProperties()[0].Name;
                _command.CommandType = CommandType.Text;
                _command.CommandText = $"SELECT * FROM {tableName} WHERE {firstColumnName} = {ID}";
                using (SqlDataReader reader = _command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            object value = reader[i];
                            if (reader[i] is DBNull && typeof(T).GetProperties()[i].GetType().Name.ToLower().Equals("string"))
                            {
                                typeof(T).GetProperties()[i].SetValue(poco, string.Empty);
                            }
                            if (reader[i] is DBNull && typeof(T).GetProperties()[i].GetType().Name.ToLower().Contains("int"))
                            {
                                typeof(T).GetProperties()[i].SetValue(poco, 0);
                            }

                            if (!(reader[i] is DBNull)) { typeof(T).GetProperties()[i].SetValue(poco, value); }
                        }

                    }
                }

                return poco;
            }
            finally { _connection.Close(); }            

        }

        public virtual void Remove(T poco)
        {
            try
            {                
                _connection.Open();
                string tableName = this.GetTableName(typeof(T));
                string firstColumnName = poco.GetType().GetProperties()[0].Name;
                object firstColumnValue = poco.GetType().GetProperties()[0].GetValue(poco);
                _command.CommandType = CommandType.Text;
                _command.CommandText = $"DELETE FROM {tableName} WHERE {firstColumnName} = {firstColumnValue}";
                _command.ExecuteNonQuery();
            }
            finally { _connection.Close(); }
        }
        public virtual void Remove(long ID)
        {
            try
            {
                _connection.Open();
                string tableName = this.GetTableName(typeof(T));
                string firstColumnName = "ID";
                object firstColumnValue = ID;
                _command.CommandType = CommandType.Text;
                _command.CommandText = $"DELETE FROM {tableName} WHERE {firstColumnName} = {firstColumnValue}";
                _command.ExecuteNonQuery();
            }
            finally { _connection.Close(); }
        }

        public virtual void Update(T poco)
        {
            try
            {                
                _connection.Open();
                string tableName = this.GetTableName(typeof(T));
                var propInfos = poco.GetType().GetProperties();
                string firstColumnName = propInfos[0].Name;
                object firstColumnValue = propInfos[0].GetValue(poco);                
                for (int i = 1; i < propInfos.Length; i++)
                {
                    var value = typeof(T).GetProperties()[i].GetValue(poco);

                    if (propInfos[i].GetValue(poco).GetType().Name == "String")
                    {
                        if (value == null) value = String.Empty;
                        else value = $"'{value}'";
                    }
                    if (propInfos[i].GetValue(poco).GetType().Name.ToLower().Contains("int") && value == null) value = 0;
                    if (propInfos[i].GetValue(poco).GetType().Name == "DateTime" && value == null) value = DateTime.MinValue;

                    _command.CommandType = CommandType.Text;
                    _command.CommandText = $"UPDATE {tableName} SET {propInfos[i].Name} = {value} WHERE {firstColumnName} = {firstColumnValue}";
                    _command.ExecuteNonQuery();
                }                
            }
            finally { _connection.Close(); }
        }
        #endregion






    }


}
