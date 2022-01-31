using System;
using IRISChatServer.Enums;

namespace IRISChatServer.Logging
{
    public class LogEntry
    {
        #region "Properties"
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
        #endregion

        #region "Constructors"
        public LogEntry()
        {
            LogLevel = LogLevel.None;
            TimeStamp = DateTime.Now;
        }
        public LogEntry(LogLevel LogLevel, string Message)
        {
            this.LogLevel = LogLevel;
            this.Message = Message;
            TimeStamp = DateTime.Now;
        }
        #endregion

        public override string ToString()
        {
            if (LogLevel != LogLevel.None)
            {
                return string.Format("{0,-30}\t {1,-10}\t {2}", TimeStamp.ToString("dd-MM-yyyy HH:mm:ss.ffff"), LogLevel.ToString().ToUpper(), Message);
            }
            else
            {
                return Message;
            }
        }
    }
}