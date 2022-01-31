namespace IRISChatServer.DataAccessLayer.Messages
{
    public class RecoverUserPasswordResultMessage : IMessage
    {
        #region "Properties"
        public bool IsResultSuccess { get; set; }
        public string ResultMessage { get; set; }
        #endregion

        #region "Constructors"
        public RecoverUserPasswordResultMessage()
        {
            IsResultSuccess = false;
            ResultMessage = "";
        }
        public RecoverUserPasswordResultMessage(bool IsResultSuccess, string ResultMessage)
        {
            this.IsResultSuccess = IsResultSuccess;
            this.ResultMessage = ResultMessage;
        }
        #endregion
    }
}