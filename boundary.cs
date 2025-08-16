namespace OrderAlReady.Boundary
{
    public class AdminUsersController
    {
        private readonly IAdminUserService _userService;

        public AdminUsersController(IAdminUserService userService)
        {
            _userService = userService;
        }

        public object CreateUser(UserType type, UserCreationData request)
        {
            var result = _userService.CreateUser(type, request);
            if (!result.IsSuccess)
            {
                return $"Error: {result.ErrorMessage}";
            }
            return result.CreatedUser;
        }

        public object SuspendUser(int id)
        {
             var result = _userService.SuspendUser(id);
             if (!result.IsSuccess)
             {
                return $"Error: {result.ErrorMessage}";
             }
             return "User suspended successfully.";
        }

        public object DeleteUser(int id)
        {
            var result = _userService.DeleteUser(id);
            if (!result.IsSuccess)
            {
                return $"Error: {result.ErrorMessage}";
            }
            return "User account deleted successfully.";
        }
    }
}