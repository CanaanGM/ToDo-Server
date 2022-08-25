using Microsoft.AspNetCore.Identity;

namespace ToDo_Server.Data.Models
{
    public class AppUser : IdentityUser
    {
        public string? DisplayName { get; set; }
        public ICollection<ToDo> ToDos { get; set; } = new List<ToDo>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    }
}
