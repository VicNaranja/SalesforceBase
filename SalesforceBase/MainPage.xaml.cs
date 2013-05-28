using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SalesforceBase.Resources;
using SalesforceBase.SFDC;
using System.IO;
using System.Text;
using System.Collections.ObjectModel;
using SalesforceBase.ViewModels;

namespace SalesforceBase
{
    public partial class MainPage : PhoneApplicationPage
    {

        public ListaLogsModel listaLogs { get; set; }

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            listaLogs = new ListaLogsModel();
            DataContext = listaLogs;

            var v = (Visibility)Resources["PhoneLightThemeVisibility"];

            // Código de ejemplo para traducir ApplicationBar
            //BuildLocalizedApplicationBar();           
        }

        private async void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            var loginNecesario = await SFDCSession.Instance.oAuthUserAgentFlow();

            if (loginNecesario)
            {
                //Inicio de la app
                App.FirstTimeApp = true;
                this.NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
            }
            else
            {
                //Hacemos una peticion y recuperamos los logs
                if (!App.Offline)
                {
                    listaLogs.obtenerUltimosLogs();                    
                }


            }
        }


        // Código de ejemplo para compilar una ApplicationBar traducida
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Establecer ApplicationBar de la página en una nueva instancia de ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Crear un nuevo botón y establecer el valor de texto en la cadena traducida de AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Crear un nuevo elemento de menú con la cadena traducida de AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}