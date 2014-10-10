using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

namespace PiUI
{
    public partial class AppSettingsPage : PhoneApplicationPage
    {
 
        /* app settings INstance variable */
        AppSettings settings;
 
        public AppSettingsPage()
        {
            InitializeComponent();
            settings = (AppSettings)this.Resources["appSettings"];


            if (settings.ServerSetting != "Not Connected")
            {
                ServerAddressBox.Text = settings.ServerSetting;
            }

            if (settings.PortSetting != 0)
            {
                PortNumberBox.Text = settings.PortSetting.ToString();
            }

           
            if (settings.InkColor == "green")
            {
                GreenButton.IsChecked = true;
            }
            else if (settings.InkColor == "red")
            {
                RedButton.IsChecked = true;
            }
            else
            {
                BlackButton.IsChecked = true;
            }
            
        }

        /* back button clicked */
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        /* connect click */
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateRemoteHost())
            {  
                /* check the settings */
                settings.ServerSetting = ServerAddressBox.Text;
                settings.PortSetting = Convert.ToInt32(PortNumberBox.Text);
                /*********************/
                /* create socket for client */
                SocketClient client = new SocketClient();
                Console.WriteLine("Establishing a Connection to {0} portNumber {1}", settings.ServerSetting, settings.PortSetting);
                string result = client.Connect(settings.ServerSetting, settings.PortSetting);
                Console.WriteLine("Connection established");
                settings.isConnected = true;
                settings.socketClient = client;
                MessageBox.Show("Connected!");
            }
            else
            {
                MessageBox.Show("Cannot Connect Please enter a valid connection");
            }
        }

        
        /* Change stylus stroke color settings*/
        private void BlackButtonClick(object sender, RoutedEventArgs e)
        {
            settings.InkColor = "black";
        }
        private void GreenButtonClick(object sender, RoutedEventArgs e)
        {
            settings.InkColor = "green";
        }
        private void RedButtonClick(object sender, RoutedEventArgs e)
        {
            settings.InkColor = "red";
        }
        
        public bool ValidateRemoteHost()
        {
            if (String.IsNullOrWhiteSpace(ServerAddressBox.Text))
            {
                MessageBox.Show("please enter a host name");
                return false;
            }

            return true;
        }

    }
}