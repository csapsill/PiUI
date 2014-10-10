using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Ink;
using System.IO;
using Windows.Devices;
using Microsoft.Devices;
using Windows.UI.Input;
using Windows.UI;


namespace PiUI
{
    public partial class MousePadPage : PhoneApplicationPage
    {
        AppSettings settings = new AppSettings();
        
        // Socket connection
        SocketClient client;

        //Stroke Variable
        Stroke stroke;

        System.Windows.Media.Color color;
        public MousePadPage()
        {
            InitializeComponent();

            settings = (AppSettings)this.Resources["appSettings"];

            if (settings.InkColor == "black")
            {
                color = System.Windows.Media.Color.FromArgb(1, 0, 0, 0);
            }
            else if (settings.InkColor == "green")
            {
                color = System.Windows.Media.Color.FromArgb(1, 57, 226, 15);
            }
            else if (settings.InkColor == "red")
            {
                color = System.Windows.Media.Color.FromArgb(1, 255, 31, 31);
            }


            client = settings.socketClient;

            /* get the server name from the pi device that is connect and display below title*/
            string connectInfo = client.Receive();
            ConnectionBox.Text = connectInfo;
            

        }
        
        VibrateController vibrate = VibrateController.Default;

        private void MyIP_TouchDown(object sender, MouseEventArgs e)
        {
            MyIP.CaptureMouse();
            StylusPointCollection MyStylusPointCollection = new StylusPointCollection();
            MyStylusPointCollection.Add(e.StylusDevice.GetStylusPoints(MyIP));
            stroke = new Stroke(MyStylusPointCollection);
            stroke.DrawingAttributes.Color = color;
            MyIP.Strokes.Add(stroke);
            vibrate.Start(TimeSpan.FromMilliseconds(50));
        }

        private void MyIP_MouseMove(object sender, MouseEventArgs e)
        {
            if (stroke != null)
            {
                stroke.StylusPoints.Add(e.StylusDevice.GetStylusPoints(MyIP));
                CoordBox.Text = "X: " + (e.StylusDevice.GetStylusPoints(MyIP)[0].X) + "\nY: " + e.StylusDevice.GetStylusPoints(MyIP)[0].Y;
                string trans = System.Convert.ToString(e.StylusDevice.GetStylusPoints(MyIP)[0].X)
                    + " " + System.Convert.ToString(e.StylusDevice.GetStylusPoints(MyIP)[0].Y);
       
                /* Send Coordinates to the server connected to the program */
                if (settings.isConnected)
                {
                    client.Send(trans);
                }
            }
        }

        private void MyIP_LostTouch(object sender, MouseEventArgs e)
        {

            stroke = null;
            MyIP.Strokes.Clear();
            CoordBox.Text = string.Empty;
        }

        private void MyIP_Tap(object sender, GestureEventArgs e)
        {
            /*send a click event to the Piserver machine*/
            if (settings.isConnected)
            {
                client.Send("Click");
            }

            CoordBox.Text = "Click!";
        }
     
    }
}   