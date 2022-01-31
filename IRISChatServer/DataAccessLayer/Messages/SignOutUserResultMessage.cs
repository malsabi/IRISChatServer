namespace IRISChatServer.DataAccessLayer.Messages
{
    public class SignOutUserResultMessage : IMessage
    {
        #region "Properties"
        public string ResultMessage { get; set; }
        public bool IsResultSuccess { get; set; }
        #endregion

        #region "Constructors"
        public SignOutUserResultMessage()
        {
            ResultMessage = "";
            IsResultSuccess = false;
        }
        public SignOutUserResultMessage(string ResultMessage, bool IsResultSuccess)
        {
            this.ResultMessage = ResultMessage;
            this.IsResultSuccess = IsResultSuccess;
        }
        #endregion
    }
}