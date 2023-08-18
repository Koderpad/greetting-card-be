using Domain.DTO;
using Domain.Entity;

namespace Service.Abstract
{
    public interface IRefreshTokenService
    {
        List<RefreshTokenDTO> GetAll();

        RefreshTokenDTO GetByUserId(string id);

        bool IsActivate(RefreshTokenDTO dto);

        RefreshTokenDTO Create(RefreshTokenDTO dto);

        void Delete(string id);

        AuthResponse GenerateLoginTokens(UserInfo user);

        AuthResponse RefreshToken(string token);
        void DeleteRefreshTokenExpired();

        void DeleteTokenWhenLogout(string refreshTokenId);
    }
}
