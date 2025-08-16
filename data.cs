using OrderAlReady.Models;
using System.Collections.Generic;
using System.Linq;

namespace OrderAlReady.Data
{
    /// <summary>
    /// A mock database implementation for testing purposes.
    /// It implements the IUserRepository interface to provide data access methods
    /// using simple in-memory lists.
    /// </summary>
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users;
        private readonly List<FoodStall> _stalls;
        private int _nextUserId = 1;

        public InMemoryUserRepository(List<User> users, List<FoodStall> stalls)
        {
            _users = users;
            _stalls = stalls;
            if (_users.Any())
            {
                // Ensures new users get a unique ID
                _nextUserId = _users.Max(u => u.UserID) + 1;
            }
        }

        public User GetUserById(int id) => _users.FirstOrDefault(u => u.UserID == id);

        public FoodStall GetStallById(int id) => _stalls.FirstOrDefault(s => s.StallID == id);

        public IEnumerable<User> GetAllUsers() => _users;
        
        public User AddUser(User user)
        {
            user.UserID = _nextUserId++;
            _users.Add(user);
            return user;
        }

        public void DeleteUser(int id)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                // If the user is a staff member, also remove them from their stall's staff list
                if (user is FoodStallStaff staff && staff.AssignedStall != null)
                {
                    staff.AssignedStall.Staff.Remove(staff);
                }
                _users.Remove(user);
            }
        }

        public void UpdateUser(User user)
        {
            // For in-memory lists, the object reference is updated directly when its
            // properties are changed (e.g., when Suspend() is called).
            // A real database implementation would have logic here to save the changes.
        }
    }
}