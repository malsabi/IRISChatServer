using System;
using System.IO;
using System.Data.SQLite;
using IRISChatServer.Models;
using IRISChatServer.Configs;
using IRISChatServer.DataAccessLayer.Messages;

namespace IRISChatServer.DataBase
{
    public class DataBaseManager
    {
        #region "Fields"
        private bool isDBConnectionOpened;
        private SQLiteConnection dBConnection;
        #endregion

        #region "Properties"
        /// <summary>
        /// It will return true, in case if the database is opened, otherwise false.
        /// </summary>
        public bool IsDBConnectionOpened
        {
            get
            {
                return isDBConnectionOpened;
            }
        }
        /// <summary>
        /// Returns the SQLite Connection.
        /// </summary>
        public SQLiteConnection DBConnection
        {
            get
            {
                return dBConnection;
            }
        }
        #endregion

        #region "Constructors"
        public DataBaseManager()
        {
            isDBConnectionOpened = false;
        }
        #endregion

        #region "Private Methods"
        /// <summary>
        /// Creates an entity called "Users" that will contain all of the registered users.
        /// </summary>
        private void CreateUsersEntity()
        {
            string Query = string.Format("CREATE TABLE IF NOT EXISTS {0} (FirstName TEXT, LastName TEXT, Email TEXT, Username TEXT PRIMARY KEY, Password TEXT, Gender TEXT, DateOfBirth TEXT, IsActive INTEGER, IsStaySignedIn INTEGER)", DataBaseConfig.UsersEntity);
            SQLiteCommand Command = new SQLiteCommand(Query, DBConnection);
            Command.ExecuteNonQuery();
        }
        /// <summary>
        /// Creates an entity called "GlobalChat" that will contain each user chat.
        /// </summary>
        private void CreateGlobalChatEntity()
        {
            string Query = string.Format("CREATE TABLE IF NOT EXISTS {0} (Username TEXT PRIMARY KEY, UserChat TEXT, TimeStamp TEXT)", DataBaseConfig.UsersEntity);
            SQLiteCommand Command = new SQLiteCommand(Query, DBConnection);
            Command.ExecuteNonQuery();
        }
        #endregion

        #region "Public Methods"
        /// <summary>
        /// This method is used for creating a database file.
        /// </summary>
        public void CreateDataBase()
        {
            if (File.Exists(DataBaseConfig.DBPath) == false)
            {
                if (Directory.Exists(DataBaseConfig.DBDirectory) == false)
                {
                    Directory.CreateDirectory(DataBaseConfig.DBDirectory);
                }
                SQLiteConnection.CreateFile(DataBaseConfig.DBPath);
            }
            dBConnection = new SQLiteConnection(string.Format("Data Source={0}", DataBaseConfig.DBPath));
        }
        /// <summary>
        /// This starts the data base but make sure to call "CreateDataBase" method first.
        /// </summary>
        public void StartDataBase()
        {
            if (isDBConnectionOpened == false)
            {
                DBConnection.Open();
                CreateUsersEntity();
                isDBConnectionOpened = true;
            }
        }
        /// <summary>
        /// This stops the data base but make sure to start before closing.
        /// </summary>
        public void StopDataBase()
        {
            if (isDBConnectionOpened == true)
            {
                DBConnection.Close();
                isDBConnectionOpened = false;
            }
        }
        /// <summary>
        /// This removes the content and deletes the data base file.
        /// </summary>
        public void DeleteDataBase()
        {
            if (File.Exists(DataBaseConfig.DBPath) == false)
            {
                if (isDBConnectionOpened)
                {
                    StopDataBase();
                }
                File.Delete(DataBaseConfig.DBPath);
            }
        }
        #endregion

        #region "User Methods"
        /// <summary>
        /// Checks if the username is found in the users entity.
        /// </summary>
        /// <param name="Username">Represents the username that is registered.</param>
        /// <returns>Returns true if the username is found otherwise false.</returns>
        public bool IsUserRegistered(string Username)
        {
            string Query = string.Format("SELECT Username FROM {0}", DataBaseConfig.UsersEntity);
            SQLiteCommand Command = new SQLiteCommand(Query, DBConnection);
            using (SQLiteDataReader DataReader = Command.ExecuteReader())
            {
                while (DataReader.Read())
                {
                    if (DataReader.GetString(0).Equals(Username))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Registers the user to the users entity
        /// </summary>
        /// <param name="User">Represents all of the information related to the user such as username, firstname, lastname, etc..</param>
        /// <returns>Returns true if the user registered otherwise false.</returns>
        public bool RegisterUser(RegisterUserMessage User)
        {
            if (IsUserRegistered(User.Username))
            {
                return false;
            }
            else
            {
                try
                {
                    SQLiteCommand InsertQuery = new SQLiteCommand(string.Format("INSERT INTO {0} (FirstName, LastName, Email, Username, Password, Gender, DateOfBirth, IsActive, IsStaySignedIn) VALUES (@FirstNameParm, @LastNameParm, @EmailParm, @UsernameParm, @PasswordParm, @GenderParm, @DateOfBirthParm, @IsActiveParm, @IsStaySignedInParm)", DataBaseConfig.UsersEntity), DBConnection);
                    InsertQuery.Parameters.AddWithValue("@FirstNameParm", User.FirstName);
                    InsertQuery.Parameters.AddWithValue("@LastNameParm", User.LastName);
                    InsertQuery.Parameters.AddWithValue("@EmailParm", User.Email);
                    InsertQuery.Parameters.AddWithValue("@UsernameParm", User.Username);
                    InsertQuery.Parameters.AddWithValue("@PasswordParm", User.Password);
                    InsertQuery.Parameters.AddWithValue("@GenderParm", User.Gender);
                    InsertQuery.Parameters.AddWithValue("@DateOfBirthParm", User.DateOfBirth);
                    InsertQuery.Parameters.AddWithValue("@IsActiveParm", 0);
                    InsertQuery.Parameters.AddWithValue("@IsStaySignedInParm", 0);
                    InsertQuery.ExecuteNonQuery();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Sets the user to be active and also to stay signed in if checked.
        /// </summary>
        /// <param name="Username">Represents the user name,</param>
        /// <param name="IsStaySignedIn">Represents if user is goingto stay active until the user signs out.</param>
        /// <returns>Returns true if signing in user succeeded otherwise false.</returns>
        public bool SignInUser(string Username, bool IsStaySignedIn)
        {
            try
            {
                if (!IsUserRegistered(Username))
                {
                    return false;
                }
                else
                {
                    string Query = string.Format("UPDATE {0} SET IsActive = @IsActiveParm, IsStaySignedIn = @IsStaySignedInParm WHERE Username = @UsernameParm", DataBaseConfig.UsersEntity);
                    SQLiteCommand UpdateQuery = new SQLiteCommand(Query, DBConnection);
                    UpdateQuery.Parameters.AddWithValue("@IsActiveParm", Convert.ToInt32(1));
                    UpdateQuery.Parameters.AddWithValue("@IsStaySignedInParm", Convert.ToInt32(IsStaySignedIn));
                    UpdateQuery.Parameters.AddWithValue("@UsernameParm", Username);
                    UpdateQuery.ExecuteNonQuery();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Sets the user to be offline, ie: not active.
        /// </summary>
        /// <param name="Username">Represents the user name.</param>
        /// <returns>Returns true if signing out the user succeeded otherwise false.</returns>
        public bool SignOutUser(string Username)
        {
            try
            {
                if (IsUserRegistered(Username) == false)
                {
                    Console.WriteLine("SignOutUser:: {0} not registered", Username);
                    return false;
                }
                else
                {
                    string Query = string.Format("UPDATE {0} SET IsStaySignedIn = @IsStaySignedInParm, IsActive = @IsActiveParm WHERE Username = @UsernameParm", DataBaseConfig.UsersEntity);
                    SQLiteCommand UpdateQuery = new SQLiteCommand(Query, DBConnection);
                    UpdateQuery.Parameters.AddWithValue("@IsStaySignedInParm", 0);
                    UpdateQuery.Parameters.AddWithValue("@IsActiveParm", 0);
                    UpdateQuery.Parameters.AddWithValue("@UsernameParm", Username);
                    UpdateQuery.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SignOutUser:: Exception: {0}", ex.Message);
                return false;
            }
        }
        /// <summary>
        /// Gets the user password if the user is registered.
        /// </summary>
        /// <param name="Username">Represents the user name.</param>
        /// <returns>Returns the password string if the user is registered otherwise null.</returns>
        public string GetUserPassword(string Username)
        {
            try
            {
                if (!IsUserRegistered(Username))
                {
                    return null;
                }
                else
                {
                    string UserPassword = null;
                    string Query = string.Format("SELECT Username, Password FROM {0}", DataBaseConfig.UsersEntity);
                    SQLiteCommand Command = new SQLiteCommand(Query, DBConnection);
                    using (SQLiteDataReader DataReader = Command.ExecuteReader())
                    {
                        while (DataReader.Read())
                        {
                            if (DataReader.GetString(0).Equals(Username))
                            {
                                UserPassword = DataReader.GetString(1);
                                break;
                            }
                        }
                    }
                    return UserPassword;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Updates the user password if the user is registered.
        /// </summary>
        /// <param name="Username">Represents the user name.</param>
        /// <param name="NewPassword">Represents the new password to be updated.</param>
        /// <returns>Returns true if the update succeeded otherwise false.</returns>
        public bool UpdateUserPassword(string Username, string NewPassword)
        {
            try
            {
                if (!IsUserRegistered(Username))
                {
                    return false;
                }
                else
                {
                    string Query = string.Format("UPDATE {0} SET Password = @PasswordParm WHERE Username = @UsernameParm", DataBaseConfig.UsersEntity);
                    SQLiteCommand UpdateQuery = new SQLiteCommand(Query, DBConnection);
                    UpdateQuery.Parameters.AddWithValue("@PasswordParm", NewPassword);
                    UpdateQuery.Parameters.AddWithValue("@UsernameParm", Username);
                    UpdateQuery.ExecuteNonQuery();
                    return true;

                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Updates the user information if the user is registered.
        /// </summary>
        /// <param name="OldUsername">Represents the old username before it gets updated.</param>
        /// <param name="UpdateUser">Represents all of the information related to the user to be updated.</param>
        /// <returns>Returns true if the update succeeded otherwise false.</returns>
        public bool UpdateUserInformation(string OldUsername, ProfileModel UpdatedUserProfile)
        {
            try
            {
                if (IsUserRegistered(OldUsername) == false)
                {
                    return false;
                }
                else
                {
                    if (OldUsername.Equals(UpdatedUserProfile.Username) == false)
                    {
                        //Check if the new username is already registered.
                        if (IsUserRegistered(UpdatedUserProfile.Username))
                        {
                            return false;
                        }
                    }
                    string Query = string.Format("UPDATE {0} SET FirstName = @FirstNameParm, LastName = @LastNameParm, Email = @EmailParm, Username = @NewUsernameParm, DateOfBirth = @DateOfBirthParm WHERE Username = @OldUsernameParm", DataBaseConfig.UsersEntity);
                    SQLiteCommand UpdateQuery = new SQLiteCommand(Query, DBConnection);
                    UpdateQuery.Parameters.AddWithValue("@FirstNameParm", UpdatedUserProfile.FirstName);
                    UpdateQuery.Parameters.AddWithValue("@LastNameParm", UpdatedUserProfile.LastName);
                    UpdateQuery.Parameters.AddWithValue("@EmailParm", UpdatedUserProfile.Email);
                    UpdateQuery.Parameters.AddWithValue("@NewUsernameParm", UpdatedUserProfile.Username);
                    UpdateQuery.Parameters.AddWithValue("@DateOfBirthParm", UpdatedUserProfile.DateOfBirth);
                    UpdateQuery.Parameters.AddWithValue("@OldUsernameParm", OldUsername);
                    UpdateQuery.ExecuteNonQuery();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Gets the user information and stores them in the user model, only if the user is registered
        /// </summary>
        /// <param name="Username">Represents the user name.</param>
        /// <returns>Returns the UserModel if the user is registered otherwise null.</returns>
        public ProfileModel GetUserInformation(string Username)
        {
            try
            {
                if (!IsUserRegistered(Username))
                {
                    return null;
                }
                else
                {
                    ProfileModel UserInformation = null;
                    string Query = string.Format("SELECT FirstName, LastName, Email, Username, DateOfBirth FROM {0}", DataBaseConfig.UsersEntity);
                    SQLiteCommand Command = new SQLiteCommand(Query, DBConnection);
                    using (SQLiteDataReader DataReader = Command.ExecuteReader())
                    {
                        while (DataReader.Read())
                        {
                            if (DataReader.GetString(3).Equals(Username))
                            {
                                UserInformation = new ProfileModel(DataReader.GetString(0), DataReader.GetString(1), DataReader.GetString(2), DataReader.GetString(3), DateTime.Parse(DataReader.GetString(4)));
                                break;
                            }
                        }
                    }
                    return UserInformation;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Deletes the user if registered in the database
        /// </summary>
        /// <param name="Username">Represents the username</param>
        /// <returns>Returns true if the user is deleted otherwise false</returns>
        public bool DeleteUser(string Username)
        {
            try
            {
                if (IsUserRegistered(Username) == false)
                {
                    Console.WriteLine("DeleteUser: {0} is not registered.", Username);
                    return false;
                }
                else
                {
                    string Query = string.Format("DELETE FROM {0} WHERE Username = @UsernameParm", DataBaseConfig.UsersEntity);
                    SQLiteCommand DeleteRowQuery = new SQLiteCommand(Query, DBConnection);
                    DeleteRowQuery.Parameters.AddWithValue("@UsernameParm", Username);
                    DeleteRowQuery.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DeleteUser: Exception: {0}", ex.Message);
                return false;
            }
        }
        #endregion
    }
}