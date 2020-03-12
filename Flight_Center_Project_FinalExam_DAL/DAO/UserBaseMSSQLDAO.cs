using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Threading.Tasks;

namespace Flight_Center_Project_FinalExam_DAL
{
    public class UserBaseMSSQLDAO<T> : DAO<T>, IBasicDB<T> where T : class, IPoco,  new()
    {        
        public UserBaseMSSQLDAO() : base() 
        {
        }
        private Utility_class_User BuildUser(T poco, long IdInUseTable)
        {
            return BuildUserInternal(poco, IdInUseTable, null, null);
        }
        private Utility_class_User BuildUser(T poco, long IdInUseTable, string userName, string password)
        {
            return BuildUserInternal(poco, IdInUseTable, userName, password);
        }
        private Utility_class_User BuildUserInternal(T poco, long IdInUseTable, string userName, string password)
        {
            Utility_class_User utility_class_User = new Utility_class_User();
            //updating Utility_class_User name from poco name             
            utility_class_User.GetType().GetProperties()[0].SetValue(utility_class_User, typeof(T).GetProperties()[0].GetValue(poco));
            utility_class_User.GetType().GetProperties()[1].SetValue(utility_class_User, typeof(T).GetProperties()[1].GetValue(poco));
            if(password != null) utility_class_User.GetType().GetProperty("PASSWORD").SetValue(utility_class_User, password);
            if(userName != null) utility_class_User.GetType().GetProperty("USER_NAME").SetValue(utility_class_User, userName);

            utility_class_User.GetType().GetProperties()[3].SetValue(utility_class_User, typeof(T).Name);
            for (int i = 0; i < utility_class_User.GetType().GetProperties().Length; i++)
            {
                if (utility_class_User.GetType().GetProperties()[i].Name.ToLower().Contains(typeof(T).Name.ToLower().ChopCharsFromTheEndInverted(7).ToString()))
                {   
                    utility_class_User.GetType().GetProperties()[i].SetValue(utility_class_User, IdInUseTable);
                }
            }

            return utility_class_User;
        }
        /*public override void Add(T poco)
        {
            T savedPoco = poco;


            base.Add(poco);
            var poco2 = base.Get((long)typeof(T).GetProperties()[0].GetValue(poco));
            if (poco2.Equals(new T())) throw new RecentlyAddedRecordCantBeRetrivedException<T>(poco);

            SetAnotherDAOInstance(typeof(Utility_class_User));
            var user = BuildUser(poco, (long)typeof(T).GetProperties()[0].GetValue(poco));
            _currentUserDAOMSSQL.Add(user);

            var user2 = _currentUserDAOMSSQL.Get(user.ID);
            if (user2 == new Utility_class_User()) throw new RecentlyAddedRecordCantBeRetrivedException<Utility_class_User>(user);
        }*/
        /// <summary>
        /// Adding a new row in one of the IUser tables, and also a new row in a corresponding Utility_class_User table
        /// _registered_user_.Add(poco);
        /// _Utility_class_User_with corresponding_USER_NAME_&_PASSWORD_.Add(poco);
        /// </summary>
        /// <param name="poco"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public void Add(T poco, string userName, string password)
        {
            SetAnotherDAOInstance(typeof(Utility_class_User));
            var user = BuildUser(poco, (long)typeof(T).GetProperty("ID").GetValue(poco), userName, password);
            _currentUserDAOMSSQL.Add(user);
            var user2 = _currentUserDAOMSSQL.Get(user.ID);
            if(user2 == new Utility_class_User()) throw new RecentlyAddedRecordCantBeRetrivedException<Utility_class_User>(user);

            typeof(T).GetProperty("USER_ID").SetValue(poco, user2.ID);
            base.Add(poco);
            var poco2 = base.Get((long)typeof(T).GetProperties()[0].GetValue(poco));
            if (poco2.Equals(new T())) throw new RecentlyAddedRecordCantBeRetrivedException<T>(poco);
                    
            //Utility_class_User user3 = _currentUserDAOMSSQL.GetUserByIdentifier2(poco2);            

        }


        public void Update(T poco, string userName, string password)
        {            
            SetAnotherDAOInstance(typeof(Utility_class_User));
            var poco2 = base.Get((long)poco.GetType().GetProperty("ID").GetValue(poco));
            Utility_class_User user = _currentUserDAOMSSQL.GetUserByIdentifier(poco2);
            user.USER_NAME = userName;
            user.PASSWORD = password;
            _currentUserDAOMSSQL.Update(user);
            var user2 = _currentUserDAOMSSQL.Get(user.ID);
            if (Statics.ComparsionByEveryPropertyEquals(user, user2)) throw new RecentlyUpdatedRecordDidntChangedException<Utility_class_User>(user);

            typeof(T).GetProperty("USER_ID").SetValue(poco, user.ID);
            base.Update(poco);
            var poco3 = Get((long)typeof(T).GetProperty("ID").GetValue(poco));
            if(Statics.ComparsionByEveryPropertyEquals<T>(poco3, poco2)) throw new RecentlyUpdatedRecordDidntChangedException<T>(poco3);
            
        }
        public override void Remove(T poco)
        {
            base.Remove(poco);
            var poco2 = base.Get((long)typeof(T).GetProperties()[0].GetValue(poco));
            if (!poco2.Equals(new T())) throw new RecentlyDeletedRecordStillExistsException<T>(poco);

            SetAnotherDAOInstance(typeof(Utility_class_User));
            _currentUserDAOMSSQL.Remove((long)typeof(T).GetProperty("USER_ID").GetValue(poco));
            var verificationUser1 = _currentUserDAOMSSQL.Get((long)typeof(T).GetProperty("USER_ID").GetValue(poco));
            if (verificationUser1 != new Utility_class_User()) throw new RecentlyDeletedRecordStillExistsException<Utility_class_User>(verificationUser1);

           
        }



    }
}
