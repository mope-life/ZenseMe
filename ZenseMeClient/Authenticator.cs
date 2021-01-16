using ZenseMe.Lib.Managers;
using System.Configuration;

namespace ZenseMe.Client.Forms
{
    class Authenticator
    {
        /**
         * Do the whole authentication process described here:
         * https://www.last.fm/api/desktopauth
         *
         * Returns true if authentication was successful or if we were already
         * authenticated.
         */
        public bool Authenticate()
        {
            ApiManager apiManager = new ApiManager();

            if (ConfigurationManager.AppSettings["LastFM_SessionKey"] == "")
            {
                if (!apiManager.GetToken())
                {
                    return false;
                }
                Login login = new Login();

                login.webBrowser1.Navigate(apiManager.GetAuthUrl());
                login.ShowDialog();

                if (!apiManager.GetSession())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
