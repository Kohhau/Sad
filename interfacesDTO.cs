using OrderAlReady.Models;
using System.Collections.Generic;

namespace OrderAlReady
{
    // --- Enum for Specifying User Type ---
    public enum UserType
    {
        Student,
        Staff,
        PriorityStudent
    }

    // --- Data Transfer Objects (DTOs) and Result Objects ---
    public class UserCreationData
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int? AssignedStallId { get; set; } // Nullable, as it's only for Staff
    }

    public class CreateUserResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public User CreatedUser { get; set; }
    }

    public class DeleteUserResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class SuspendUserResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }

    // --- Interfaces ---
    public interface IAdminUserService
    {
        CreateUserResult CreateUser(UserType type, UserCreationData data);
        SuspendUserResult SuspendUser(int userId);
        DeleteUserResult DeleteUser(int userId);
    }

    public interface IUserRepository
    {
        User GetUserById(int id);
        User AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
        FoodStall GetStallById(int id);
        IEnumerable<User> GetAllUsers();
    }
}