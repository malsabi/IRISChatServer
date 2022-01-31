namespace IRISChatServer.DataAccessLayer.Messages
{
    public class MessageWrapper
    {
        #region "Properties"
        public string MessageType;
        public object Message;
        #endregion

        #region "Constructors"
        public MessageWrapper(string MessageType, object Message)
        {
            this.MessageType = MessageType;
            this.Message = Message;
        }
        #endregion
    }
}