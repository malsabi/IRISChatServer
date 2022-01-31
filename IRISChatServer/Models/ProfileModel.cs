using System;

namespace IRISChatServer.Models
{
    public class ProfileModel
    {
        #region "Properties"
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public DateTime DateOfBirth { get; set; }
        #endregion

        #region "Constructors"
        public ProfileModel()
        {
            FirstName = "";
            LastName = "";
            Email = "";
            Username = "";
            DateOfBirth = DateTime.MinValue;
        }
        public ProfileModel(string FirstName, string LastName, string Email, string Username, DateTime DateOfBirth)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Username = Username;
            this.DateOfBirth = DateOfBirth;
        }
        #endregion
    }
}