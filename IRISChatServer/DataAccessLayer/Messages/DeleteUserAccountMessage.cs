namespace IRISChatServer.DataAccessLayer.Messages
{
    public class DeleteUserAccountMessage : IMessage
    {
        #region "Properties"
        public string Username { get; set; }
        #endregion

        #region "Constructors"
        public DeleteUserAccountMessage()
        {
            Username = "";
        }
        public DeleteUserAccountMessage(string Username)
        {
            this.Username = Username;
        }
        #endregion
    }
}