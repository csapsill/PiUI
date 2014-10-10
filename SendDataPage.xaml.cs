using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace PiUI.Resources
{
    public partial class SendDataPage : PhoneApplicationPage
    {

        string serverHost;
        int port;
        const int ECHO_PORT = 5990;

        AppSettingsPage appSettings = new AppSettingsPage();
        AppSettings settings = new AppSettings();

        public SendDataPage()
        {
            InitializeComponent();
            serverHost = settings.ServerSetting;
            port = settings.PortSetting;
            textRemoteHost.DataContext = serverHost + ":" + port; // Attempting to use ServerName TextBox on AppSettingsPage
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            ClearLog();

            if (settings.isConnected && ValidateInput())
            {
                SocketClient client = settings.socketClient;

                /*
                Log(String.Format("Connecting to server '{0}' over port {1} (echo ...", textRemoteHost.Text, ECHO_PORT), true);
                string result = client.Connect(settings.ServerSetting, settings.PortSetting);
                Log(result, false);
                */

                Log(String.Format("Sending '{0}' to server {1}...", txtInput.Text, settings.ServerSetting), true);
                string result = client.Send(txtInput.Text);
                Log(result, false);

                Log("requesting Receive...", true);
                result = client.Receive();
                Log(result, false);
                client.Close();
            }

            else
            {
                MessageBox.Show("please enter text to server or a valid remote host ");
            }
        }


        #region UI Validation


        private bool ValidateInput()
        {
            if (String.IsNullOrWhiteSpace(txtInput.Text))
            {
                MessageBox.Show("Please enter some text to server");
                return false;
            }

            return true;
        }


        #endregion

        #region Logging

        private void Log(string message, bool isOutGoing)
        {
            string direction = (isOutGoing) ? ">> " : "<< ";
            txtOutput.Text += Environment.NewLine + direction + message;
        }

        private void ClearLog()
        {
            txtOutput.Text = String.Empty;
        }
        #endregion
    }
}