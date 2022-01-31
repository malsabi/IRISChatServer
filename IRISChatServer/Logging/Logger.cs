using System;
using System.Collections.ObjectModel;
using IRISChatServer.Enums;

namespace IRISChatServer.Logging
{
    public static class Logger
    {
        #region "Fields"
        #endregion

        #region "Properties"
        public static ObservableCollection<LogEntry> Entries { get; }
        #endregion

        #region "Constructors"
        static Logger()
        {
            Entries = new ObservableCollection<LogEntry>();
        }
        #endregion

        #region "Logging"
        public static void Warning(string Message)
        {
            Log(LogLevel.Warning, Message);
        }

        public static void Information(string Message)
        {
            Log(LogLevel.Information, Message);
        }

        public static void Success(string Message)
        {
            Log(LogLevel.Success, Message);
        }

        public static void Error(string Message)
        {
            Log(LogLevel.Error, Message);
        }

        public static void None(string Message)
        {
            Log(LogLevel.None, Message);
        }

        public static void Log(LogLevel LogLevel, string Message)
        {
            if (App.Current != null)
            {
                App.Current.Dispatcher.BeginInvoke(new Action(() => Entries.Add(new LogEntry(LogLevel, Message))));
            }
        }
        #endregion
    }
}