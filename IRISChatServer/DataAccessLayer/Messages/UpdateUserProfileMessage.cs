using IRISChatServer.Models;

namespace IRISChatServer.DataAccessLayer.Messages
{
    public class UpdateUserProfileMessage : IMessage
    {
        #region "Properties"
        public string OldUsername { get; set; }
        public ProfileModel UpdatedUserProfile { get; set; }
        #endregion

        #region "Constructors"
        public UpdateUserProfileMessage()
        {
            OldUsername = "";
            UpdatedUserProfile = null;
        }
        public UpdateUserProfileMessage(string OldUsername, ProfileModel UpdatedUserProfile)
        {
            this.OldUsername = OldUsername;
            this.UpdatedUserProfile = UpdatedUserProfile;
        }
        #endregion
    }
}