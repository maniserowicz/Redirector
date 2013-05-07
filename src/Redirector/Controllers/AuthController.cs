using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Procent.Redirector.Configuration;
using WorldDomination.Web.Authentication;
using WorldDomination.Web.Authentication.Providers;
using System.Linq;

namespace Procent.Redirector.Controllers
{
    public class AuthController : Controller
    {
        private AuthenticationService _authenticationService;

        public AuthController()
        {
            var googleProvider = new GoogleProvider(new ProviderParams()
                {
                    Key = RedirectorConfiguration.Value("Auth.Google.Key"),
                    Secret = RedirectorConfiguration.Value("Auth.Google.Secret"),
                });
            //googleProvider.DefaultAuthenticationServiceSettings.CallBackUri = new Uri("authentication/authenticatecallback?providerKey=Google", UriKind.Relative);

            _authenticationService = new AuthenticationService();
            _authenticationService.AddProvider(googleProvider);
        }

        public ActionResult Index()
        {
            return View();
        }

        public RedirectResult RedirectToAuthenticate(string providerKey)
        {
            var authenticateServiceSettings = this._authenticationService.GetAuthenticateServiceSettings(providerKey, this.Request.Url);
            var uri = _authenticationService.RedirectToAuthenticationProvider(authenticateServiceSettings);
            return Redirect(uri.AbsoluteUri);
        }

        public ActionResult AuthenticateCallback(string providerKey)
        {
            if (string.IsNullOrEmpty(providerKey))
            {
                throw new ArgumentNullException("providerKey");
            }

            var authenticateServiceSettings = this._authenticationService.GetAuthenticateServiceSettings(providerKey, this.Request.Url);
            var authenticatedClient = _authenticationService.GetAuthenticatedClient(authenticateServiceSettings, Request.Params);

            var allowedEmails = RedirectorConfiguration.Value("Auth.AllowedEmails").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (allowedEmails.Contains(authenticatedClient.UserInformation.Email))
            {
                FormsAuthentication.SetAuthCookie(authenticatedClient.UserInformation.Email, false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return new HttpUnauthorizedResult();
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}