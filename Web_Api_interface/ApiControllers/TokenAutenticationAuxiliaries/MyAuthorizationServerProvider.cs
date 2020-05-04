using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Web_Api_interface
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            /*await Task.Run(() => 
            {
                context.Validated();
            });*/

            context.Validated();

        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            /*await Task.Run(() => 
            {
                using (Utility_class_UserRepository _repo = new Utility_class_UserRepository())
                {
                    var user = _repo.ValidateUser(context.UserName, context.Password);
                    if (user == null)
                    {
                        context.SetError("invalid_grant", "Provided username and password is incorrect");
                        return;
                    }

                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim(ClaimTypes.Role, user.USER_KIND));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.USER_NAME));                 
                    identity.AddClaim(new Claim("Password", user.PASSWORD));

                    context.Validated(identity);
                }
            });*/

            using (Utility_class_UserRepository _repo = new Utility_class_UserRepository())
            {
                var user = _repo.ValidateUser(context.UserName, context.Password);
                if (user == null)
                {
                    context.SetError("invalid_grant", "Provided username and password is incorrect");
                    return;
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Role, user.USER_KIND));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.USER_NAME));
                identity.AddClaim(new Claim("Password", user.PASSWORD));
                


                context.Validated(identity);
            }


        }

    }
}