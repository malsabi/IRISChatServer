using IRISChatServer.Models;

namespace IRISChatServer.DataAccessLayer.Messages
{
    public class UpdateUserProfileResultMessage : IMessage
    {
        #region "Properties"
        public ProfileModel UpdatedUserProfile { get; set; }
        public string ResultMessage { get; set; }
        public bool IsResultSuccess { get; set; }
        #endregion

        #region "Constructors"
        public UpdateUserProfileResultMessage()
        {
            UpdatedUserProfile = null;
            ResultMessage = "";
            IsResultSuccess = false;
        }
        public UpdateUserProfileResultMessage(ProfileModel UpdatedUserProfile, string ResultMessage, bool IsResultSuccess)
        {
            this.UpdatedUserProfile = UpdatedUserProfile;
            this.ResultMessage = ResultMessage;
            this.IsResultSuccess = IsResultSuccess;
        }
        #endregion
    }
}