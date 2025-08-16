using OrderAlReady.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        // --- SETUP: Create a mock database and initial data ---
        var userDatabase = new List<User>();
        var admin = new Admin(userDatabase) { UserID = 1, Name = "SysAdmin" };
        userDatabase.Add(admin);

        var studentToSuspend = new Student { UserID = 2, Name = "John Doe" };
        userDatabase.Add(studentToSuspend);
        
        var chickenStall = new FoodStall { StallID = 101, Name = "Chicken Rice Stall" };
        var noodleStall = new FoodStall { StallID = 102, Name = "Noodle Stall" };
        
        // --- TEST EXECUTION ---
        Console.WriteLine("--- Testing Use Case: Admin Manages User Accounts ---");

        // Test Case: Suspend a User
        Console.WriteLine("\n[Test Case: Suspending an active user...]");
        admin.SuspendUser(studentToSuspend);
        Console.WriteLine($"Verification: John Doe's status is now '{studentToSuspend.Status}'.");

        // Test Case: Attempt to suspend a non-existent user
        Console.WriteLine("\n[Test Case: Attempting to suspend a non-existent user...]");
        int nonExistentUserId = 999;
        var nonExistentUser = userDatabase.FirstOrDefault(u => u.UserID == nonExistentUserId);
        bool suspendResult = admin.SuspendUser(nonExistentUser);
        if (!suspendResult)
        {
            Console.WriteLine($"SUCCESS: System correctly handled the operation for non-existent user ID {nonExistentUserId}.");
        }
        else
        {
            Console.WriteLine($"FAILURE: System incorrectly processed a non-existent user ID {nonExistentUserId}.");
        }

        Console.WriteLine("\n--------------------------------------------------\n");

        // Test Case: Create a valid student account
        Console.WriteLine("[NEW Test Case: Creating a valid student account...]");
        var studentData = new UserCreationData { Name = "Alice Tan", Email = "alice.t@example.com" };
        var student1 = admin.CreateUser(UserType.Student, studentData);

        // Test Case (was A2): Create a valid staff account
        Console.WriteLine("\n[Test Case: Creating a valid staff account...]");
        var staffData = new UserCreationData { Name = "Maria Lim", Email = "maria.lim@example.com", AssignedStall = chickenStall };
        var staff1 = admin.CreateUser(UserType.Staff, staffData);
        
        // Test Case (was E2): Attempt to create staff with invalid data (missing name)
        Console.WriteLine("\n[Test Case: Attempting to create user with missing name...]");
        var invalidData = new UserCreationData { Name = "", Email = "ken@example.com", AssignedStall = noodleStall };
        admin.CreateUser(UserType.Staff, invalidData);

        // Test Case: Attempt to create staff with missing stall assignment
        Console.WriteLine("\n[NEW Test Case: Attempting to create staff with missing stall...]");
        var noStallData = new UserCreationData { Name = "Ken Goh", Email = "ken@example.com", AssignedStall = null };
        admin.CreateUser(UserType.Staff, noStallData);

        Console.WriteLine("\n--------------------------------------------------\n");

        // Test Case: Delete a student account successfully
        Console.WriteLine("[NEW Test Case: Deleting a student account...]");
        admin.DeleteUser(student1);
        
        // Test Case (was E1): Attempt to delete the only staff member at a stall
        Console.WriteLine("\n[Test Case: Attempting to delete the only staff member...]");
        admin.DeleteUser(staff1);

        // Setup: Add a second staff member to the stall to allow for successful deletion
        Console.WriteLine("\n[Setup: Adding a second staff member to Chicken Rice Stall...]");
        var staffData2 = new UserCreationData { Name = "Bob", Email = "bob@example.com", AssignedStall = chickenStall };
        var staff2 = admin.CreateUser(UserType.Staff, staffData2);
        
        // Test Case: Attempt to delete the first staff member again (should succeed)
        Console.WriteLine("\n[Test Case: Attempting to delete the first staff member again...]");
        admin.DeleteUser(staff1);
        Console.WriteLine($"Verification: Chicken Stall now has {chickenStall.Staff.Count} staff member(s).");
        Console.WriteLine($"Verification: The database now has {userDatabase.Count} total users.");
    }
}