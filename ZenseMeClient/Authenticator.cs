using System.Configuration;

using ZenseMe.Lib.Managers;
using ZenseMe.Client.Forms;

namespace ZenseMe.Client
{
    class Authenticator
    {
        /**
         * Do the whole authentication process
         * @return  True if authentication was successful or if we were already
         *          authenticated
         */
        public bool Authenticate()
        {
            Audioscrobbler audioscrobbler = new Audioscrobbler();

            if (ConfigurationManager.AppSettings["LastFM_SessionKey"] == "")
            {
                if (!audioscrobbler.GetToken())
                {
                    return false;
                }
                Login login = new Login();

                login.webBrowser1.Navigate(audioscrobbler.GetAuthUrl());
                login.ShowDialog();

                if (!audioscrobbler.GetSession())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
