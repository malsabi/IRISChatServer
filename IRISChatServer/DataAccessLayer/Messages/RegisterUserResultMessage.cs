namespace IRISChatServer.DataAccessLayer.Messages
{
    public class RegisterUserResultMessage : IMessage
    {
        #region "Properties"
        public bool IsResultSuccess { get; set; }
        public string ResultMessage { get; set; }
        #endregion

        #region "Constructors"
        public RegisterUserResultMessage()
        {
            IsResultSuccess = false;
            ResultMessage = "";
        }
        public RegisterUserResultMessage(bool IsResultSuccess, string ResultMessage)
        {
            this.IsResultSuccess = IsResultSuccess;
            this.ResultMessage = ResultMessage;
        }
        #endregion
    }
}