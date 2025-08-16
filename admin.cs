using OrderAlReady.Models;
using System.Collections.Generic;
using System; // For Console.WriteLine

// New enum to specify which type of user to create
public enum UserType
{
    Student,
    Staff,
    PriorityStudent
}

// A simple object to pass user creation data
public class UserCreationData
{
    public string Name { get; set; }
    public string Email { get; set; }
    public FoodStall AssignedStall { get; set; } // Only used for Staff
}

namespace OrderAlReady.Models
{
    public class Admin : User
    {
        private List<User> _userDatabase;

        public Admin(List<User> userDatabase)
        {
            _userDatabase = userDatabase;
        }

        /// <summary>
        /// GENERIC: Creates any type of user based on the UserType enum.
        /// </summary>
        public User CreateUser(UserType type, UserCreationData data)
        {
            if (string.IsNullOrWhiteSpace(data.Name) || string.IsNullOrWhiteSpace(data.Email))
            {
                Console.WriteLine("ERROR: Cannot create user. Name and email are required.");
                return null;
            }

            User newUser = null;
            switch (type)
            {
                case UserType.Student:
                    newUser = new Student();
                    break;
                case UserType.PriorityStudent:
                    newUser = new PriorityStudent();
                    break;
                case UserType.Staff:
                    if (data.AssignedStall == null)
                    {
                        Console.WriteLine("ERROR: Cannot create staff. An assigned stall is required.");
                        return null;
                    }
                    newUser = new FoodStallStaff { AssignedStall = data.AssignedStall };
                    data.AssignedStall.Staff.Add((FoodStallStaff)newUser); // Add to stall's list
                    break;
            }

            if (newUser != null)
            {
                newUser.UserID = _userDatabase.Count + 1;
                newUser.Name = data.Name;
                newUser.Email = data.Email;
                _userDatabase.Add(newUser);
                Console.WriteLine($"SUCCESS: {type} user '{data.Name}' was created.");
            }
            return newUser;
        }

        /// <summary>
        /// GENERIC: Deletes any type of user.
        /// Includes a special check for the FoodStallStaff dependency rule.
        /// </summary>
        public bool DeleteUser(User userToDelete)
        {
            if (userToDelete == null || userToDelete is Admin)
            {
                Console.WriteLine("ERROR: User not found or cannot delete an admin.");
                return false;
            }

            // Special business rule check for staff members
            if (userToDelete is FoodStallStaff staff)
            {
                if (staff.AssignedStall != null && staff.AssignedStall.Staff.Count == 1)
                {
                    Console.WriteLine($"ERROR: Cannot delete '{staff.Name}'. They are the only staff at '{staff.AssignedStall.Name}'.");
                    return false;
                }
                if (staff.AssignedStall != null)
                {
                    staff.AssignedStall.Staff.Remove(staff);
                }
            }

            _userDatabase.Remove(userToDelete);
            Console.WriteLine($"SUCCESS: User '{userToDelete.Name}' has been deleted.");
            return true;
        }

        // --- SuspendUser method remains the same ---
        public bool SuspendUser(User userToSuspend)
        {
            if (userToSuspend == null || userToSuspend.Status == UserStatus.Suspended)
            {
                return false;
            }
            userToSuspend.Suspend();
            Console.WriteLine($"SUCCESS: User '{userToSuspend.Name}' has been suspended.");
            return true;
        }
    }
}