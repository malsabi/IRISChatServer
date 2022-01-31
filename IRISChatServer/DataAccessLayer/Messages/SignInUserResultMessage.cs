using IRISChatServer.Models;

namespace IRISChatServer.DataAccessLayer.Messages
{
    public class SignInUserResultMessage : IMessage
    {
        #region "Properties"
        public bool IsResultSuccess { get; set; }
        public string ResultMessage { get; set; }
        public ProfileModel UserProfile { get; set; }
        #endregion

        #region "Constructors"
        public SignInUserResultMessage()
        {
            IsResultSuccess = false;
            ResultMessage = "";
            UserProfile = null;
        }
        public SignInUserResultMessage(bool IsResultSuccess, string ResultMessage, ProfileModel UserProfile)
        {
            this.IsResultSuccess = IsResultSuccess;
            this.ResultMessage = ResultMessage;
            this.UserProfile = UserProfile;
        }
        #endregion
    }
}