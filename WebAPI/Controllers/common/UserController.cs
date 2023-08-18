using Domain.DTO;
using Domain.ExceptionModel;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Middlewares;

namespace WebAPI.Controllers.common
{
    [TypeFilter(typeof(AuthenticationFilter))]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserInfoService userInfoService;

        public UserController(IUserInfoService userInfoService)
        {
            this.userInfoService = userInfoService;
        }
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Xem thông tin cá nhân của người dùng")]
        public IActionResult GetById(string id)
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == "uid")!.Value;
            if (id != userId)
            {
                throw new AccessDeniedException();
            }
            return Ok(userInfoService.GetById(id));
        }

        [HttpGet("{id}/usercard")]
        [SwaggerOperation(Summary = "Xem danh sách thiệp mà người dùng đã tạo")]
        public IActionResult GetUserCardsByUserId(string id, int offset, int pagesize)
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == "uid")!.Value;
            if (id != userId)
            {
                throw new AccessDeniedException();
            }
            return Ok(userInfoService.GetUserCardsByUserId(id, offset, pagesize));
        }

        [HttpPatch("changeprofile")]
        [SwaggerOperation(Summary = "Cập nhật thông tin người dùng")]
        public IActionResult UpdateProfile(UserInfoDTO userInfoDTO)
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == "uid")!.Value;
            if (userInfoDTO.Id != userId)
            {
                throw new AccessDeniedException();
            }
            return Ok(userInfoService.Update(userInfoDTO));
        }

        [HttpPatch("changepassword")]
        [SwaggerOperation(Summary = "Đổi mật khẩu người dùng")]
        public IActionResult ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == "uid")!.Value;
            if (changePasswordDTO.Id != userId)
            {
                throw new AccessDeniedException();
            }
            userInfoService.ChangePassword(changePasswordDTO);
            return Ok();
        }

        //[HttpGet("{id}/user-upload")]
        //[SwaggerOperation(Summary = "Lấy tất cả user upload theo user id (có phân trang)")]
        //public IActionResult GetUserUploadByUserId(string id, int offset, int pagesize)
        //{
        //    if (User.IsInRole("Admin") || User.IsInRole("Common"))
        //    {
        //        return Ok(userInfoService.GetUserUploadByUserId(id, offset, pagesize));
        //    }
        //    throw new InvalidTokenException();
        //}
    }
}