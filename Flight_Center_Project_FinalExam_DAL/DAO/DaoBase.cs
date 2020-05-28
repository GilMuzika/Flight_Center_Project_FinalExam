using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Flight_Center_Project_FinalExam_DAL
{
    public abstract class DaoBase<T> : IBasicDB<T> where T : class, IPoco, new()
    {
        #region Template methods
        /// <summary>
        /// Open the SQL connection
        /// </summary>
        protected abstract void OpenConnection();

        /// <summary>
        /// Close the SQL connection
        /// </summary>
        protected abstract void Close();

        /// <summary>
        /// Set the SQL command
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="storedProcedureParameters">Dictionary<string, string> that contains names and values of the stored proceture parameters</param>
        protected abstract void SetCommand(CommandType commandType, string commandText, Dictionary<string, object> storedProcedureParameters);

        /// <summary>
        /// Set the SQL command
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        protected abstract void SetCommand(CommandType commandType, string commandText);        

        protected abstract List<T> ReadFromDb();

        protected abstract long AddToDb();

        protected abstract void UpdateInDb();


        /// <summary>
        /// "storedProcedureParameters" is a Dictionary<string, string> that contains parameter for stored procedure.
        /// If it null so the CommandType is "Text"
        /// </summary>
        /// <param name="action"></param>
        /// <param name="commandType"></param>
        /// <param name="commandTextOrStoredProcedureName"></param>
        /// <param name="storedProcedureParametersDict"></param>
        /// <returns></returns>
        protected List<T> RunToRead(ExecuteCurrentMethosProcedure executeCurrentMethosProcedure, CommandType commandType)
        {
            List<T> value = null;
            Action a = () =>
            {
                try
                {
                    OpenConnection();
                    executeCurrentMethosProcedure(out string commandTextStoredProdedureName, out string commandTextForTextMode, out Dictionary<string, Object> stroredProsedureParametersDict);
                    if (commandType == CommandType.Text) SetCommand(commandType, commandTextForTextMode);
                    else SetCommand(commandType, commandTextStoredProdedureName, stroredProsedureParametersDict);
                    value = ReadFromDb();
                }
                finally { Close(); }
            };
            ProcessExceptions(a);
            return value;
        }

        /// <summary>
        /// "ExecuteScalar" for extract a first column ("ID") value
        /// </summary>
        /// <param name="action"></param>
        /// <param name="commandType"></param>
        /// <param name="commandTextOrStoredProcedureName"></param>
        /// <param name="storedProcedureParametersDict"></param>
        /// <returns></returns>
        protected long RunToAdd(ExecuteCurrentMethosProcedure executeCurrentMethosProcedure, CommandType commandType)
        {
            long value = -1;
            Action a = () =>
             {
                 try
                 {
                     OpenConnection();
                     executeCurrentMethosProcedure(out string commandTextStoredProdedureName, out string commandTextForTextMode, out Dictionary<string, Object> stroredProsedureParametersDict);
                     if (commandType == CommandType.Text) SetCommand(commandType, commandTextForTextMode);
                     else SetCommand(commandType, commandTextStoredProdedureName, stroredProsedureParametersDict);
                     value = AddToDb();
                 }
                 finally { Close(); }
            };
            ProcessExceptions(a);
            return value;
        }

        /// <summary>
        /// This methos us in se to execute UPDATE and also REMOVE sql statements
        /// </summary>
        /// <param name="action"></param>
        /// <param name="commandType"></param>
        /// <param name="commandTextOrStoredProcedureName"></param>
        /// <param name="storedProcedureParametersDict"></param>
        protected void RunToUpdate(ExecuteCurrentMethosProcedure executeCurrentMethosProcedure, CommandType commandType)
        {
            Action a = () =>
            {
                try
                {
                    OpenConnection();
                    executeCurrentMethosProcedure(out string commandTextStoredProdedureName, out string commandTextForTextMode, out Dictionary<string, Object> stroredProsedureParametersDict);
                    if (commandType == CommandType.Text) SetCommand(commandType, commandTextForTextMode);
                    else SetCommand(commandType, commandTextStoredProdedureName, stroredProsedureParametersDict);
                    UpdateInDb();
                }
                finally { Close(); }
            };
            ProcessExceptions(a);
        }

        protected long Run(Func<long> AddToDb_method, ExecuteCurrentMethosProcedure executeCurrentMethosProcedure, CommandType commandType)
        {
            //If the "ReadFromDb_method" and "UpdateInDb_method" parameters are null the method will execute "AddToDb_method" return "idValueAfterAdding". "payloadValueAfterGetting" will be null.
            RunInternal(AddToDb_method, null, null, out long idValueAfterAdding, out List<T> payloadValueAfterGetting, executeCurrentMethosProcedure, commandType);
            return idValueAfterAdding;
        }

        protected List<T> Run(Func<List<T>> ReadFromDb_method, ExecuteCurrentMethosProcedure executeCurrentMethosProcedure, CommandType commandType)
        {
            //If "AddToDb_method" and "UpdateInDb_method" are null "ReadFromDb_method" will be executed and "payloadValueAfterGetting" will be returned. "idValueAfterAdding" will be -1.
            RunInternal(null, ReadFromDb_method, null, out long idValueAfterAdding, out List<T> payloadValueAfterGetting, executeCurrentMethosProcedure, commandType);
            return payloadValueAfterGetting;
        }

        protected void Run(Action UpdateInDb_method, ExecuteCurrentMethosProcedure executeCurrentMethosProcedure, CommandType commandType)
        {
            //If "AddToDb_method" and "ReadFromDb_method" are null, "UpdateInDb_method" will be executed. Since "UpdateInDb_method" is Action and don't return any value, so "idValueAfterAdding" and "payloadValueAfterGetting" will be their respective default values, -1 and null. 
            //This methos don't need to return any value.
            RunInternal(null, null, UpdateInDb_method, out long idValueAfterAdding, out List<T> payloadValueAfterGetting, executeCurrentMethosProcedure, commandType);
        }
        

        private void RunInternal(Func<long> AddToDb_method, Func<List<T>> ReadFromDb_method, Action UpdateInDb_method, out long idValueAfterAdding, out List<T> payloadValueAfterGetting, ExecuteCurrentMethosProcedure executeCurrentMethosProcedure, CommandType commandType)
        {
            object funcReturnValue = null;            
            Func<object> f = () =>
            {
                try
                {
                    object returnValueInternal = null;

                    OpenConnection();
                    executeCurrentMethosProcedure(out string commandTextStoredProdedureName, out string commandTextForTextMode, out Dictionary<string, Object> stroredProsedureParametersDict);
                    if (commandType == CommandType.Text) SetCommand(commandType, commandTextForTextMode);
                    else SetCommand(commandType, commandTextStoredProdedureName, stroredProsedureParametersDict);

                    if (AddToDb_method != null) returnValueInternal = AddToDb_method();
                    if (ReadFromDb_method != null) returnValueInternal = ReadFromDb_method();
                    if (UpdateInDb_method != null) UpdateInDb_method();

                    return returnValueInternal;
                }
                finally { Close(); }
            };
            funcReturnValue = ProcessExceptions(f);
            if (funcReturnValue is long) idValueAfterAdding = (long)funcReturnValue; else idValueAfterAdding = -1;
            if (funcReturnValue is List<T>) payloadValueAfterGetting = (List<T>)funcReturnValue; else payloadValueAfterGetting = null;

        }

        #endregion Template methods



        public abstract long Add(T poco);

        public abstract T Get(long ID);

        public abstract List<T> GetAll();

        public abstract void Remove(T poco);

        public abstract void Update(T poco);


        #region Additional auxiliary methods
        private async Task ProcessExceptions(Func<Task> func)
        {
            try
            {
                await func();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"{ex.GetType().Name}\n\n{ex.Message}\n\n\n{ex.StackTrace}");
            }
        }
        private void ProcessExceptions(Action act)
        {
            try
            {
                act();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"{ex.GetType().Name}\n\n{ex.Message}\n\n\n{ex.StackTrace}");
            }
        }
        private object ProcessExceptions(Func<object> func)
        {
            object val = null;
            try
            {
                val = func();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"{ex.GetType().Name}\n\n{ex.Message}\n\n\n{ex.StackTrace}");
            }
            return val;
        }
        #endregion Additional auxiliary methods
    }
}
