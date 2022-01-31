namespace IRISChatServer.Configs
{
    public class DataBaseConfig
    {
        #region "DATABASE CONSTANTS"
        /// <summary>
        /// This is where the directory of the databse will be created that contains
        /// the database file and any other important data.
        /// </summary>
        public static string DBDirectory = "./DataBase";
        /// <summary>
        /// This is the database file name that will be saved in the disk.
        /// </summary>
        public static string DBName = "IRIS.sqlite";
        /// <summary>
        /// This is the path for the databse sqlite.
        /// </summary>
        public static string DBPath = string.Format("{0}/{1}", DBDirectory, DBName);
        /// <summary>
        /// Represents the users entity name.
        /// </summary>
        public static string UsersEntity = "USERS";
        /// <summary>
        /// Represents the global chat entity name.
        /// </summary>
        public static string GlobalChatEntity = "GLOBALCHAT";
        #endregion
    }
}