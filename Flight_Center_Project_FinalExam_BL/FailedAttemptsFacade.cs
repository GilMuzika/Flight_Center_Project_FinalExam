using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Center_Project_FinalExam_BL
{
    public class FailedAttemptsFacade : AnonimousUserFacade
    {
        public bool AddBlackUser(FailedLoginAttempt blackUser)
        {
            _failedLoginAttemptsDAO.Add(blackUser);
            return IsSomethingExists(blackUser);            
        }

        public bool UpdateBlackUser(FailedLoginAttempt blackUser)
        {
            _failedLoginAttemptsDAO.Update(blackUser);

            FailedLoginAttempt blackUserForChecking = _failedLoginAttemptsDAO.Get(blackUser.ID);
            bool isUpdated =  Statics.BulletprofComparsion(blackUserForChecking, blackUser);
            return isUpdated;
        }

        public FailedLoginAttempt GetByUserName(string username)
        {
            return _failedLoginAttemptsDAO.GetSomethingBySomethingInternal(username, (int)FailedLoginAttemptPropertyNumber.FAILED_USERNAME);
        }

        public FailedLoginAttempt GetByPassword(string password)
        {
            return _failedLoginAttemptsDAO.GetSomethingBySomethingInternal(password, (int)FailedLoginAttemptPropertyNumber.FAILED_PASSWORD);
        }


    }
}
