namespace IRISChatServer.Models
{
    public class UpdateUserModel
    {
        #region "Properties"
        public string OldUsername { get; set; }
        public string NewUsername { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }
        #endregion

        #region "Constructors"
        public UpdateUserModel()
        {
            OldUsername = "";
            NewUsername = "";
            FirstName = "";
            LastName = "";
            Email = "";
            DateOfBirth = "";
        }
        public UpdateUserModel(string OldUsername, string NewUsername, string FirstName, string LastName, string Email, string DateOfBirth)
        {
            this.OldUsername = OldUsername;
            this.NewUsername = NewUsername;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.DateOfBirth = DateOfBirth;
        }
        #endregion
    }
}