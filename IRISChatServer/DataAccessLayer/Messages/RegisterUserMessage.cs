namespace IRISChatServer.DataAccessLayer.Messages
{
    public class RegisterUserMessage : IMessage
    {
        #region "Properties"
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        #endregion

        #region "Interface"
        #endregion

        #region "Constructors"
        public RegisterUserMessage()
        {
            FirstName = "";
            LastName = "";
            Username = "";
            Email = "";
            Password = "";
            DateOfBirth = "";
            Gender = "";
        }
        public RegisterUserMessage(string FirstName, string LastName, string Username, string Email, string Password, string DateOfBirth, string Gender)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Username = Username;
            this.Email = Email;
            this.Password = Password;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
        }
        #endregion
    }
}