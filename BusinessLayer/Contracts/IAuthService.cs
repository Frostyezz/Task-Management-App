using System.Security.Claims;

namespace BusinessLayer.Contracts
{
    public interface IAuthService
    {
        string CreateToken(Guid UserId);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        bool VerifyToken(string token, out Guid? userId);
    }
}
