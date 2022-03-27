using System.Collections.Generic;

namespace StationTireInspection.Classes
{
    public class UserInformations
    {
        public UserNameAndID NameAndID { get; private set; }
        public string Password { get; private set; }
        public byte AskPasswordChanged { get; private set; }

        public UserInformations(UserNameAndID UserNameAndID, string UserPassword, byte askPasswordChanged)
        {
            NameAndID = UserNameAndID;
            Password = UserPassword;
            AskPasswordChanged = askPasswordChanged;
        }
    }
}
