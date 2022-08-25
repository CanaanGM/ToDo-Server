using ToDo_Server.Data.Models;

namespace ToDo_Server.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(string UserName, string userId, string userEmail);
        RefreshToken GenerateRefreshToken();
    }
}
