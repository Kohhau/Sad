namespace OrderAlReady.Models
{
    public enum UserStatus
    {
        Active,
        Suspended
    }

    public abstract class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        // The Status property now has a private setter to protect it.
        public UserStatus Status { get; private set; }

        // Constructor to initialize status
        public User()
        {
            Status = UserStatus.Active;
        }

        public void Suspend()
        {
            if (this.Status == UserStatus.Active)
            {
                this.Status = UserStatus.Suspended;
            }
        }
    }
}