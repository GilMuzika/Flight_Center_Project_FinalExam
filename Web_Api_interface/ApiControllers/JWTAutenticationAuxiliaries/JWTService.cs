using Flight_Center_Project_FinalExam_DAL;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Web_Api_interface.Controllers.JWTAutenticationAuxiliaries
{
    class JWTService : IAuthService
    {
        #region Members
        /// <summary>
        /// The secret key we use to encrypt our token with
        /// </summary>
        public string SecretKey { get; set; }
        #endregion
        #region Constructor
        public JWTService(string Secretkey)
        {
            SecretKey = Secretkey;
        }
        #endregion

        #region Private methods
        private SecurityKey GetSymmetricSecurityKey()
        {
            byte[] symmetricKey = Convert.FromBase64String(SecretKey);
            return new SymmetricSecurityKey(symmetricKey);
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = GetSymmetricSecurityKey()
            };
        }

        public bool IsTokenValid(string token)
        {
            if (String.IsNullOrEmpty(token)) throw new ArgumentException("Given token is null or empty.");

            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters();

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        //-------------------------
        public string CreateToken(Utility_class_User validatedUserModel)
        {
            //set issued at date
            DateTime issuedAt = DateTime.UtcNow;
            //set the time when it expires
            DateTime expires = DateTime.UtcNow.AddDays(7);

            //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            var claimsIdentity = new ClaimsIdentity(new[]

            {
                new Claim(ClaimTypes.Name, validatedUserModel.USER_NAME),
                new Claim(ClaimTypes.Role, validatedUserModel.USER_KIND),
                new Claim("Password", validatedUserModel.PASSWORD),
            }

                );


            var signingCredentials = new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256Signature);


            //Create the jwt (JSON Web Token)
            //Replace the issuer and audience with your URL (ex. http:localhost:12345)
            var token =
                (JwtSecurityToken)
                tokenHandler.CreateJwtSecurityToken(
                    issuer: "https://localhost:44361/",
                    audience: "https://localhost:44361/",
                    subject: claimsIdentity,
                    notBefore: issuedAt,
                    expires: expires,
                    signingCredentials: signingCredentials);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
        //-------------------------

        public IEnumerable<Claim> GetTokenClaims(string token)
        {
            if (String.IsNullOrEmpty(token)) throw new ArgumentException("Given token is null or empty.");

            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters();

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return tokenValid.Claims;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
