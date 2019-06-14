using Owin;
using System.Web.Http;
using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin.Security.Cookies;
using System.Security.Claims;

//[assembly: OwinStartup("Startup", typeof(TossWebApi.Startup))]

namespace TossWebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app); configure IS4 here (you just need this)
            //app.UseCookieAuthentication(new CookieAuthenticationOptions());
            //app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            //{
            //    Authority = "http://localhost:50050/",
            //    RequiredScopes = new[] { "TossWebApi" },

            //    ClientId = "TossWebApi",
            //    ClientSecret = "toss-web-api-secret",

            //    RoleClaimType = ClaimTypes.Role
            //});
        }

    }
}
