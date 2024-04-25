using System.ComponentModel.DataAnnotations.Schema;

namespace JobWebsite.Models
{
    public class Job
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public decimal Salary { get; set; }
        public required string Location { get; set; }
    }
}
