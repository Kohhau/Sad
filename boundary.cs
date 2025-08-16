// using Microsoft.AspNetCore.Mvc; // Imports for a real ASP.NET Core project

namespace OrderAlReady.Boundary
{
    /// <summary>
    /// <<boundary>>
    /// Handles incoming HTTP requests for managing user accounts.
    /// Delegates all business logic to the IAdminUserService.
    /// </summary>
    // [ApiController]
    // [Route("api/admin/users")]
    public class AdminUsersController // : ControllerBase
    {
        private readonly IAdminUserService _userService;

        // The service is "injected" into the controller's constructor.
        public AdminUsersController(IAdminUserService userService)
        {
            _userService = userService;
        }

        // POST /api/admin/users/staff
        // [HttpPost("staff")]
        public object CreateStaff(CreateStaffRequestDto request)
        {
            var result = _userService.CreateStaffAccount(request);

            if (!result.IsSuccess)
            {
                // return BadRequest(result.ErrorMessage);
                return $"Error: {result.ErrorMessage}"; // Simplified for console
            }
            // return Ok(result.CreatedStaff);
            return result.CreatedStaff; // Simplified for console
        }
        
        // POST /api/admin/users/{id}/suspend
        // [HttpPost("{id}/suspend")]
        public object SuspendUser(int id)
        {
             var result = _userService.SuspendUser(id);
             if (!result)
             {
                // return NotFound($"User with ID {id} not found or already suspended.");
                return $"Error: User with ID {id} not found or already suspended.";
             }
             // return Ok("User suspended successfully.");
             return "User suspended successfully.";
        }

        // DELETE /api/admin/users/staff/{id}
        // [HttpDelete("staff/{id}")]
        public object DeleteStaff(int id)
        {
            var result = _userService.DeleteStaffAccount(id);
            if (!result.IsSuccess)
            {
                // return BadRequest(result.ErrorMessage);
                return $"Error: {result.ErrorMessage}";
            }
            // return Ok("Staff account deleted successfully.");
            return "Staff account deleted successfully.";
        }
    }
}