using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using SalesforceBase.SFDC;
using System.Security.Cryptography;
using System.Text;

namespace SalesforceBase
{
    public partial class Login : PhoneApplicationPage
    {
        public Login()
        {
            InitializeComponent();

            //this.oAuthBrowser = new WebBrowser();
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            var session = SFDC.SFDCSession.Instance;
            
            //Cargamos en el explorador la URL de salesforce
            //prepare the request uri and the callback uri
            Uri requestUri = new Uri(session.urlLoginBase + session.ConsumerKey + "&redirect_uri=" + WebUtility.UrlEncode(session.RedirectUri),UriKind.Absolute);
            Uri callbackUri = new Uri(session.RedirectUri);

            //this.oAuthBrowser = new WebBrowser();
            this.oAuthBrowser.Navigating += oAuthBrowser_Navigating;

            if (session.LogOut)
            {
                Uri requestUriLogOut = new Uri(session.InstanceUrl + "/secur/logout.jsp", UriKind.Absolute);
                this.oAuthBrowser2.Navigate(requestUriLogOut);
                this.oAuthBrowser.Navigate(requestUri);
            }
            else
            {
                this.oAuthBrowser.Navigate(requestUri);
            }

        }

        async void oAuthBrowser_Navigating(object sender, NavigatingEventArgs e)
        {
            var response = e.Uri.ToString();
            if (response.LastIndexOf("access_token=") != -1)
            {
                var session = SFDC.SFDCSession.Instance;

                var parametros = response.Substring(response.IndexOf("#")+1).Split('&');
                foreach (var parametro in parametros)
                {
                    var name = parametro.Split('=')[0];
                    if (name == "access_token")
                        session.AccessToken = parametro.Split('=')[1];

                    if (name == "refresh_token")
                    {
                        session.RefreshToken = parametro.Substring("refresh_token=".Length);           
                    }

                    if (name == "instance_url")
                    {
                        session.InstanceUrl = parametro.Split('=')[1];
                        IsolatedStorageSettings.ApplicationSettings[SFDCSession.URL_INSTANCE] = session.InstanceUrl;
                    }

                }
                e.Cancel = true;                
                await oAuthBrowser.ClearCookiesAsync();
                await oAuthBrowser.ClearInternetCacheAsync();
                
                NavigationService.GoBack();
            }
        }


        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (App.FirstTimeApp)
            {
                if (NavigationService.CanGoBack)
                {
                    while (NavigationService.RemoveBackEntry() != null)
                    {
                        NavigationService.RemoveBackEntry();
                    }
                }
            }
        }

            
        
    }
}