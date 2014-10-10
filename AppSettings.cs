using System;
using System.IO.IsolatedStorage;

namespace PiUI
{
    public class AppSettings
    {
        IsolatedStorageSettings settings;

        const string ServerSettingKeyName = "ServerSettings";
        const string PortNumKeyName = "PortNumber";
        const string IsConnectedKeyName = "IsConnected";
        const string SocketClientKeyName = "SocketClient";
        const string InkColorKeyName = "InkColor";        

        const string ServerSettingDefault = "Not Connected";
        const int PortNumKeyDefault = 0;
        const bool IsConnectedDefault = false;
        const SocketClient SocketClientDefaultValue = null;
        const String InkColorDefaultValue = "black";

        //Constructor
        public AppSettings()
        {
            if (!System.ComponentModel.DesignerProperties.IsInDesignTool)
            {
                settings = IsolatedStorageSettings.ApplicationSettings;
            }

        }

        public bool UpdateandConnect(string Key, Object value)
        {
            bool valueChanged = false;

            if (settings.Contains(Key))
            {
                if (settings[Key] != value)
                {
                    settings[Key] = value;
                    valueChanged = true;
                }

            }
            else
            {
                settings.Add(Key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        public T GetValueOrDefault<T>(string Key, T defaultValue){
            T value;

            if (settings.Contains(Key))
            {
                value = (T)settings[Key];
            }

            else
            {
                value = defaultValue;
            }
            return value;

        }

        public string ServerSetting
        {
            get
            {
                return GetValueOrDefault<string>(ServerSettingKeyName, ServerSettingDefault);
            }
            set
            {
                if (UpdateandConnect(ServerSettingKeyName, value))
                {
                    Save();
                }
            }
        }

        public int PortSetting
        {
            get
            {
                return GetValueOrDefault<int>(PortNumKeyName, PortNumKeyDefault);
            }
            set{
                if (UpdateandConnect(PortNumKeyName, value))
                {
                    Save();
                }
            }
        }

        public SocketClient socketClient
        {
            get
            {
                return GetValueOrDefault<SocketClient>(SocketClientKeyName, SocketClientDefaultValue);
            }
            set
            {
                if (UpdateandConnect(SocketClientKeyName, value))
                {
                    Save();
                }
            }
        }
        
        public String InkColor
        {
            get
            {
                return GetValueOrDefault<String>(InkColorKeyName, InkColorDefaultValue);
            }
            set
            {
                if (UpdateandConnect(InkColorKeyName, value))
                {
                    Save();
                }
            }
        }
        
        
        public bool isConnected
        {
            get
            {
                return GetValueOrDefault<bool>(IsConnectedKeyName, IsConnectedDefault);
            }
            set
            {
                if (UpdateandConnect(IsConnectedKeyName, value))
                {
                    Save();
                }
            }
        }

        public void Save()
        {
            settings.Save();
        }

    }
}
