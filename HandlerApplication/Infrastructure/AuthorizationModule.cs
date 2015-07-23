using HandlerApplication.DAL.Security;
using HandlerApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace HandlerApplication.Infrastructure
{
    public class AuthorizationModule : IHttpModule
    {
        public AuthorizationModule()
        {
        }

        public String ModuleName
        {
            get { return "AuthorizationModule"; }
        }

        public void Init(HttpApplication application)
        {
            application.BeginRequest +=
                (new EventHandler(this.OnAuthorization));
        }

        private void OnAuthorization(Object source,
             EventArgs e)
        {
            HttpApplication httpApp = (HttpApplication)source;
            HttpCookie authCookie = httpApp.Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {

                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                CustomPrincipalSerializeModel serializeModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);
                CustomPrincipal newUser = new CustomPrincipal(authTicket.Name);
                newUser.UserId = serializeModel.UserId;
                newUser.FirstName = serializeModel.FirstName;
                newUser.LastName = serializeModel.LastName;
                newUser.roles = serializeModel.roles;

                HttpContext.Current.User = newUser;
            }
        }

        public void Dispose() { }
    }
}