namespace OrderAlReady.Models
{
    /// <summary>
    /// Represents a food stall staff member.
    /// </summary>
    public class FoodStallStaff : User
    {
        // Reference to the stall they work at
        public FoodStall AssignedStall { get; set; }
    }
}