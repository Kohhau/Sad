using OrderAlReady.Models; // Assumes access to the entity classes from the previous answer

namespace OrderAlReady.Control
{
    /// <summary>
    /// <<control>>
    /// Implements the business logic for managing user accounts.
    /// This is where the rules from the use case specification are enforced.
    /// </summary>
    public class AdminUserService : IAdminUserService
    {
        // A reference to a repository to handle data access.
        private readonly IUserRepository _userRepository;

        public AdminUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public CreateStaffResult CreateStaffAccount(CreateStaffRequestDto request)
        {
            // E2: Invalid Data check
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
            {
                return new CreateStaffResult { IsSuccess = false, ErrorMessage = "Name and email are required." };
            }

            var stall = _userRepository.GetStallById(request.AssignedStallId);
            if (stall == null)
            {
                 return new CreateStaffResult { IsSuccess = false, ErrorMessage = "Assigned stall not found." };
            }
            
            // A2: Logic to create the new staff member
            var newStaff = new FoodStallStaff
            {
                Name = request.Name,
                Email = request.Email,
                AssignedStall = stall
            };

            var createdStaff = _userRepository.AddUser(newStaff);
            return new CreateStaffResult { IsSuccess = true, CreatedStaff = (FoodStallStaff)createdStaff };
        }
        
        public bool SuspendUser(int userId)
        {
            var user = _userRepository.GetUserById(userId);
            if (user == null || user.Status == UserStatus.Suspended)
            {
                return false;
            }
            user.Suspend();
            _userRepository.UpdateUser(user);
            return true;
        }

        public DeleteStaffResult DeleteStaffAccount(int staffId)
        {
            var staffToDelete = _userRepository.GetUserById(staffId) as FoodStallStaff;

            if (staffToDelete == null)
            {
                return new DeleteStaffResult { IsSuccess = false, ErrorMessage = "Staff member not found." };
            }

            // E1: Business rule check
            if (staffToDelete.AssignedStall != null && staffToDelete.AssignedStall.Staff.Count == 1)
            {
                return new DeleteStaffResult { IsSuccess = false, ErrorMessage = "Cannot delete the only staff member at a stall." };
            }

            _userRepository.DeleteUser(staffId);
            return new DeleteStaffResult { IsSuccess = true };
        }
    }
}