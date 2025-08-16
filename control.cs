using OrderAlReady.Models;
using System.Linq;

namespace OrderAlReady.Control
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IUserRepository _userRepository;

        public AdminUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public CreateUserResult CreateUser(UserType type, UserCreationData data)
        {
            if (string.IsNullOrWhiteSpace(data.Name) || string.IsNullOrWhiteSpace(data.Email))
            {
                return new CreateUserResult { IsSuccess = false, ErrorMessage = "Name and email are required." };
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
                    if (data.AssignedStallId == null)
                    {
                        return new CreateUserResult { IsSuccess = false, ErrorMessage = "AssignedStallId is required for Staff." };
                    }
                    var stall = _userRepository.GetStallById(data.AssignedStallId.Value);
                    if (stall == null)
                    {
                        return new CreateUserResult { IsSuccess = false, ErrorMessage = "Assigned stall not found." };
                    }
                    newUser = new FoodStallStaff { AssignedStall = stall };
                    stall.Staff.Add((FoodStallStaff)newUser);
                    break;
            }

            if (newUser != null)
            {
                newUser.Name = data.Name;
                newUser.Email = data.Email;
                var createdUser = _userRepository.AddUser(newUser);
                return new CreateUserResult { IsSuccess = true, CreatedUser = createdUser };
            }
            return new CreateUserResult { IsSuccess = false, ErrorMessage = "Invalid user type specified." };
        }

        public SuspendUserResult SuspendUser(int userId)
        {
            var user = _userRepository.GetUserById(userId);
            if (user == null)
            {
                return new SuspendUserResult { IsSuccess = false, ErrorMessage = $"User with ID {userId} not found." };
            }
            if (user.Status == UserStatus.Suspended)
            {
                return new SuspendUserResult { IsSuccess = false, ErrorMessage = "User is already suspended." };
            }
            user.Suspend();
            _userRepository.UpdateUser(user);
            return new SuspendUserResult { IsSuccess = true };
        }

        public DeleteUserResult DeleteUser(int userId)
        {
            var userToDelete = _userRepository.GetUserById(userId);
            if (userToDelete == null)
            {
                return new DeleteUserResult { IsSuccess = false, ErrorMessage = "User not found." };
            }
            if (userToDelete is Admin)
            {
                return new DeleteUserResult { IsSuccess = false, ErrorMessage = "Cannot delete an admin account." };
            }

            if (userToDelete is FoodStallStaff staff)
            {
                if (staff.AssignedStall != null && staff.AssignedStall.Staff.Count == 1)
                {
                    return new DeleteUserResult { IsSuccess = false, ErrorMessage = "Cannot delete the only staff member at a stall." };
                }
            }

            _userRepository.DeleteUser(userId);
            return new DeleteUserResult { IsSuccess = true };
        }
    }
}