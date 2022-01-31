namespace IRISChatServer.DataAccessLayer.Messages
{
    public class RecoverUserPasswordMessage : IMessage
    {
        #region "Properties"
        public string Username { get; set; }
        #endregion

        #region "Constructors"
        public RecoverUserPasswordMessage(string Username)
        {
            this.Username = Username;
        }
        #endregion
    }
}