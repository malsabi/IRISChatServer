namespace IRISChatServer.DataAccessLayer.Messages
{
    public class DeleteUserAccountResultMessage : IMessage
    {
        #region "Properties"
        public string ResultMessage { get; set; }
        public bool IsResultSuccess { get; set; }
        #endregion

        #region "Constructors"
        public DeleteUserAccountResultMessage()
        {
            ResultMessage = "";
            IsResultSuccess = false;
        }
        public DeleteUserAccountResultMessage(string ResultMessage, bool IsResultSuccess)
        {
            this.ResultMessage = ResultMessage;
            this.IsResultSuccess = IsResultSuccess;
        }
        #endregion
    }
}