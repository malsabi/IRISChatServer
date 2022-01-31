using System;
using System.Threading;
using IRISChatServer.Extensions;

namespace IRISChatServer.DataAccessLayer.Messages
{
    public class RequestMessage
    {
        #region "Properties"
        public DateTime TimeStamp { get; set; }
        public string ExpectedMessageType { get; set; }
        public string MessageType { get; set; }
        public object Message { get; set; }
        public bool IsTimeout { get; set; }
        public ManualResetEvent Handler { get; set; }
        public byte[] Id { get; set; }
        #endregion

        #region "Constructors"
        public RequestMessage()
        {
            TimeStamp = DateTime.Now;
            ExpectedMessageType = null;
            Message = null;
            MessageType = null;
            IsTimeout = false;
            Handler = new ManualResetEvent(false);
            Id = new byte[8].Randomize();
        }
        public RequestMessage(DateTime TimeStamp, string ExpectedMessageType)
        {
            this.TimeStamp = TimeStamp;
            this.ExpectedMessageType = ExpectedMessageType;
            Message = null;
            MessageType = null;
            IsTimeout = false;
            Handler = new ManualResetEvent(false);
            Id = new byte[8].Randomize();
        }
        #endregion

        #region "Disposable"
        public void Dispose()
        {
            Handler.Dispose();
        }
        #endregion
    }
}