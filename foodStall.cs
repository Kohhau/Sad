namespace OrderAlReady.Models
{
    /// <summary>
    /// Represents a food stall in the system.
    /// </summary>
    public class FoodStall
    {
        public int StallID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        public string Description { get; set; }

        public string OperationalHours { get; set; }

        public string Status { get; set; }

        public DateTime EstimatedPrepTime { get; set; }
        // A list of staff employed by this stall
        public List<FoodStallStaff> Staff { get; set; } = new List<FoodStallStaff>();
    }
}