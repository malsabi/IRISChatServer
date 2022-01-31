namespace IRISChatServer.DataAccessLayer.Messages
{
    public class SignInUserMessage : IMessage
    {
        #region "Properties"
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsStaySignedIn { get; set; }
        #endregion

        #region "Constructors"
        public SignInUserMessage()
        {
            Username = "";
            Password = "";
        }
        public SignInUserMessage(string Username, string Password, bool IsStaySignedIn)
        {
            this.Username = Username;
            this.Password = Password;
            this.IsStaySignedIn = IsStaySignedIn;
        }
        #endregion
    }
}