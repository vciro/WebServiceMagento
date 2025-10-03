using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebServiceAllers.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            Dao.UsersDao oUsuD = new Dao.UsersDao();
            Models.UsuarioModel oUsuI = oUsuD.GetUsuario(context.UserName, context.Password);
            if (oUsuI != null)
            {
                var extraProperties = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "userFullName" , oUsuI.Nombre
                    },
                    {
                        "Login", oUsuI.Login
                    }
                });
                var oAuthIdentity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, oUsuI.Login)
                }, context.Options.AuthenticationType);
                var ticket = new AuthenticationTicket(new ClaimsIdentity(context.Options.AuthenticationType), extraProperties);
                context.Validated(ticket);
                return;
            }
            else
            {
                context.SetError("El nombre de usuario o la contraseña no son correctos.");
                return;
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                if (property.Key[0] != '.')
                {
                    context.AdditionalResponseParameters.Add(property.Key, property.Value);
                }
            }
            return Task.FromResult<object>(null);
        }
    }
}