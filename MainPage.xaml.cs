using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PiUI.Resources;

namespace PiUI
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            
            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault();

            IconicTileData iconictile = new IconicTileData();
            iconictile.Title = "PiUI";
            iconictile.Count = 0;


            iconictile.SmallIconImage = new Uri("Assets/110.png", UriKind.Relative);
            iconictile.IconImage = new Uri("Assets/IconicMediumLarge.png", UriKind.Relative);

            tile.Update(iconictile);
            //ShellTile.Create(new Uri("/Mainpage.xaml", UriKind.Relative), iconictile, true);
        }

       
            // Listener for button press
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AppSettingsPage.xaml", UriKind.Relative));
        }

        private void MouseButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MousePadPage.xaml", UriKind.Relative));
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SendDataPage.xaml", UriKind.Relative));
        }

    }
}