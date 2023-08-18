using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Service.Abstract;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserInfoService userInfoService;

        private readonly IRefreshTokenService refreshTokenService;

        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthenticationController(IUserInfoService userInfoService,
            IRefreshTokenService refreshTokenService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.userInfoService = userInfoService;
            this.refreshTokenService = refreshTokenService;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO loginDTO)
        {
            var user = userInfoService.ValidateUser(loginDTO);
            var authResponse = refreshTokenService.GenerateLoginTokens(user);
            Response.Cookies.Append("X-Refresh-Token", authResponse.Token?.RefreshToken!, new CookieOptions()
            { HttpOnly = true, SameSite = SameSiteMode.None, Expires = DateTime.UtcNow.AddDays(7), Secure = true });

            return Ok(new { accessToken = authResponse.Token?.AccessToken, user = authResponse.UserInfo });
        }

        [HttpPost("refreshtoken")]
        [SwaggerOperation(Summary = "Làm mới token")]
        public IActionResult RenewToken(string? token)
        {
            string xRefreshToken = httpContextAccessor.HttpContext!.Request.Cookies["X-Refresh-Token"]!;
            if (xRefreshToken != null && token == null)
            {
                token = xRefreshToken;
            }

            var authResponse = refreshTokenService.RefreshToken(token!);

            Response.Cookies.Append("X-Refresh-Token", authResponse.Token?.RefreshToken!, new CookieOptions()
            { HttpOnly = true, SameSite = SameSiteMode.None, Expires = DateTime.UtcNow.AddDays(7), Secure = true });

            return Ok(new { accessToken = authResponse.Token?.AccessToken, user = authResponse.UserInfo });
        }

        [HttpPost("register")]
        public IActionResult Register(UserInfoDTO userInfoDTO)
        {
            return Ok(userInfoService.RegisterUser(userInfoDTO));
        }

        [HttpPost("logout")]
        [SwaggerOperation(Summary = "Đăng xuất")]
        public IActionResult Logout(string? refreshToken)
        {
            string xRefreshToken = httpContextAccessor.HttpContext!.Request.Cookies["X-Refresh-Token"]!;
            if (xRefreshToken != null && refreshToken == null)
            {
                refreshToken = xRefreshToken;
            }

            refreshTokenService.DeleteTokenWhenLogout(refreshToken!);

            return Ok();
        }

        [HttpPost("resetpassword")]
        [SwaggerOperation(Summary = "Quên mật khẩu")]
        public IActionResult ResetPassword([FromBody] JObject requestBody)
        {
            string email = requestBody.GetValue("email")!.ToString();

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email is required.");
            }

            var updatedUser = userInfoService.ResetPassword(email);
            return Ok(updatedUser);
        }
    }
}
