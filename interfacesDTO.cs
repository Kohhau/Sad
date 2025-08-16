using OrderAlReady.Models;

namespace OrderAlReady
{
    // --- Data Transfer Object (DTO) ---
    // A simple class to carry data for the create request.
    public class CreateStaffRequestDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int AssignedStallId { get; set; }
    }

    // --- Result Objects ---
    // Used to return complex results from the service.
    public class CreateStaffResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public FoodStallStaff CreatedStaff { get; set; }
    }
    
    public class DeleteStaffResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }


    // --- Interfaces for Dependency Injection ---
    public interface IAdminUserService
    {
        CreateStaffResult CreateStaffAccount(CreateStaffRequestDto request);
        bool SuspendUser(int userId);
        DeleteStaffResult DeleteStaffAccount(int staffId);
    }
    
    // Represents the contract for a data access layer.
    public interface IUserRepository
    {
        User GetUserById(int id);
        User AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
        FoodStall GetStallById(int id);
    }
}