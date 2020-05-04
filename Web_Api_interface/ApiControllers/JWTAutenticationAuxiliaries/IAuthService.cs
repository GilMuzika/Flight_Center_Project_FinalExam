using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Web_Api_interface.Controllers.JWTAutenticationAuxiliaries
{
    interface IAuthService
    {
        string SecretKey { get; set; }

        bool IsTokenValid(string token);

        string CreateToken(Utility_class_User validatedUserModel);

        IEnumerable<Claim> GetTokenClaims(string token);
    }
}
