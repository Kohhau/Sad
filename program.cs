using OrderAlReady;
using OrderAlReady.Control;
using OrderAlReady.Data;
using OrderAlReady.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        // --- SETUP: The service and repository pattern is now used ---
        var userDatabase = new List<User>();
        var stallDatabase = new List<FoodStall>();
        
        IUserRepository userRepository = new InMemoryUserRepository(userDatabase, stallDatabase);
        IAdminUserService adminService = new AdminUserService(userRepository);

        // Add initial data
        var admin = new Admin { Name = "SysAdmin" };
        userRepository.AddUser(admin);
        var studentToSuspend = new Student { Name = "John Doe" };
        userRepository.AddUser(studentToSuspend);

        var chickenStall = new FoodStall { StallID = 101, Name = "Chicken Rice Stall" };
        stallDatabase.Add(chickenStall);
        var noodleStall = new FoodStall { StallID = 102, Name = "Noodle Stall" };
        stallDatabase.Add(noodleStall);
        
        // --- TEST EXECUTION ---
        Console.WriteLine("--- Testing Use Case: Admin Manages User Accounts ---");

        // Test Case: Suspend a User
        Console.WriteLine("\n[Test Case: Suspending an active user...]");
        adminService.SuspendUser(studentToSuspend.UserID);
        Console.WriteLine($"Verification: John Doe's status is now '{studentToSuspend.Status}'.");

        // Test Case: Attempt to suspend a non-existent user
        Console.WriteLine("\n[Test Case: Attempting to suspend a non-existent user...]");
        int nonExistentUserId = 999;
        var suspendResult = adminService.SuspendUser(nonExistentUserId);
        if (!suspendResult.IsSuccess)
        {
            Console.WriteLine($"SUCCESS: System correctly handled the operation: {suspendResult.ErrorMessage}");
        }
        else
        {
            Console.WriteLine($"FAILURE: System incorrectly processed a non-existent user ID {nonExistentUserId}.");
        }

        Console.WriteLine("\n--------------------------------------------------\n");
        
        // NEW Test Case: Create a valid student account
        Console.WriteLine("[NEW Test Case: Creating a valid student account...]");
        var studentData = new UserCreationData { Name = "Alice Tan", Email = "alice.t@example.com" };
        var createStudentResult = adminService.CreateUser(UserType.Student, studentData);
        var student1 = createStudentResult.CreatedUser;
        if (createStudentResult.IsSuccess) Console.WriteLine($"SUCCESS: Created student '{student1.Name}' with ID {student1.UserID}.");

        // ADAPTED Test Case (was A2): Create a valid staff account
        Console.WriteLine("\n[Test Case: Creating a valid staff account...]");
        var staffData = new UserCreationData { Name = "Maria Lim", Email = "maria.lim@example.com", AssignedStallId = chickenStall.StallID };
        var createStaffResult = adminService.CreateUser(UserType.Staff, staffData);
        var staff1 = createStaffResult.CreatedUser;
        if (createStaffResult.IsSuccess) Console.WriteLine($"SUCCESS: Created staff '{staff1.Name}' with ID {staff1.UserID}.");
        
        // ADAPTED Test Case (was E2): Attempt to create staff with invalid data (missing name)
        Console.WriteLine("\n[Test Case: Attempting to create user with missing name...]");
        var invalidData = new UserCreationData { Name = "", Email = "ken@example.com", AssignedStallId = noodleStall.StallID };
        var invalidResult = adminService.CreateUser(UserType.Staff, invalidData);
        if (!invalidResult.IsSuccess) Console.WriteLine($"SUCCESS: Operation failed as expected: {invalidResult.ErrorMessage}");

        // NEW Test Case: Attempt to create staff with missing stall assignment
        Console.WriteLine("\n[NEW Test Case: Attempting to create staff with non-existent stall...]");
        var noStallData = new UserCreationData { Name = "Ken Goh", Email = "ken@example.com", AssignedStallId = 999 };
        var noStallResult = adminService.CreateUser(UserType.Staff, noStallData);
        if (!noStallResult.IsSuccess) Console.WriteLine($"SUCCESS: Operation failed as expected: {noStallResult.ErrorMessage}");

        Console.WriteLine("\n--------------------------------------------------\n");

        // NEW Test Case: Delete a student account successfully
        Console.WriteLine("[NEW Test Case: Deleting a student account...]");
        var deleteStudentResult = adminService.DeleteUser(student1.UserID);
        if (deleteStudentResult.IsSuccess) Console.WriteLine($"SUCCESS: Student '{student1.Name}' was deleted.");
        
        // ADAPTED Test Case (was E1): Attempt to delete the only staff member at a stall
        Console.WriteLine("\n[Test Case: Attempting to delete the only staff member...]");
        var deleteResult1 = adminService.DeleteUser(staff1.UserID);
        if(!deleteResult1.IsSuccess) Console.WriteLine($"SUCCESS: Operation failed as expected: {deleteResult1.ErrorMessage}");
        
        // ADAPTED Setup: Add a second staff member to the stall to allow for successful deletion
        Console.WriteLine("\n[Setup: Adding a second staff member to Chicken Rice Stall...]");
        var staffData2 = new UserCreationData { Name = "Bob", Email = "bob@example.com", AssignedStallId = chickenStall.StallID };
        var createStaffResult2 = adminService.CreateUser(UserType.Staff, staffData2);
        var staff2 = createStaffResult2.CreatedUser;
        if (createStaffResult2.IsSuccess) Console.WriteLine($"SUCCESS: Created staff '{staff2.Name}' with ID {staff2.UserID}.");

        // ADAPTED Test Case: Attempt to delete the first staff member again (should succeed)
        Console.WriteLine("\n[Test Case: Attempting to delete the first staff member again...]");
        var deleteStaffResult = adminService.DeleteUser(staff1.UserID);
        if (deleteStaffResult.IsSuccess) Console.WriteLine($"SUCCESS: Staff member '{staff1.Name}' was deleted.");
        Console.WriteLine($"Verification: Chicken Stall now has {chickenStall.Staff.Count} staff member(s).");
        Console.WriteLine($"Verification: The database now has {userRepository.GetAllUsers().Count()} total users.");
    }
}