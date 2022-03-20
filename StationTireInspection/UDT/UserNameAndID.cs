namespace StationTireInspection.Classes
{
    public class UserNameAndID
    {
        public int ID { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public UserNameAndID(int UserID, string UserFirstName, string UserLastName)
        {
            ID = UserID;
            FirstName = UserFirstName;
            LastName = UserLastName;
        }
    }
}
