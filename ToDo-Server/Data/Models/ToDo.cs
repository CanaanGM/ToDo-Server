using System.ComponentModel.DataAnnotations;

namespace ToDo_Server.Data.Models
{
    public class ToDo
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime MarkedDone { get; set; }
        public DateTime AddedToDatabase { get; } = DateTime.UtcNow;
        public bool status { get; set; } = false;

        // user goes here ~!

        public string UserId { get; set; } = string.Empty;
        public AppUser? User { get; set; } 

    }
}
