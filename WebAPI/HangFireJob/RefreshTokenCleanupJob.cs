using Service.Abstract;

namespace WebAPI.HangFireJob
{
    public class RefreshTokenCleanupJob
    {
        private readonly IRefreshTokenService refreshTokenService;

        public RefreshTokenCleanupJob(IRefreshTokenService refreshTokenService)
        {
            this.refreshTokenService = refreshTokenService;
        }
        public void CleanupExpriredRefreshToken()
        {
            refreshTokenService.DeleteRefreshTokenExpired();
            Console.WriteLine("cleanup refresh token");
        }
    }
}
