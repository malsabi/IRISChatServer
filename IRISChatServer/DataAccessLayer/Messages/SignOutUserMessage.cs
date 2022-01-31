namespace IRISChatServer.DataAccessLayer.Messages
{
    public class SignOutUserMessage : IMessage
    {
        #region "Properties"
        public string Username { get; set; }
        #endregion

        #region "Constructors"
        public SignOutUserMessage()
        {
            Username = "";
        }
        public SignOutUserMessage(string Username)
        {
            this.Username = Username;
        }
        #endregion
    }
}