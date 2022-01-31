namespace IRISChatServer.Models
{
    public class User
    {
        public string RemoteEndPoint { get; set; }
        
        public string FullName { get; set; }

        public string Username { get; set; }

        public string LastUpdate { get; set; }

        public string LastLogin { get; set; }

        public bool IsVerified { get; set; }

        public bool IsOnline { get; set; }

        public User(string remoteEndPoint, string fullName, string username, string lastUpdate, string lastLogin, bool isVerified, bool isOnline)
        {
            RemoteEndPoint = remoteEndPoint;
            FullName = fullName;
            Username = username;
            LastUpdate = lastUpdate;
            LastLogin = lastLogin;
            IsVerified = isVerified;
            IsOnline = isOnline;
        }
    }
}