using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using WebAPI.Middlewares;

namespace WebAPI.Controllers.admin
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    [Route("api/admin/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserInfoService userInfoService;

        public UserController(IUserInfoService userInfoService)
        {
            this.userInfoService = userInfoService;
        }

        //[HttpGet]
        //[SwaggerOperation(Summary = "Lấy User có trong database (có phân trang) [Admin]")]
        //public IActionResult Get(int offset, int pagesize)
        //{
        //    return Ok(userInfoService.Get(offset, pagesize));
        //}

        //[HttpDelete]
        //[SwaggerOperation(Summary = "Xóa User (chuyển trạng thái từ false thành true) [Admin]")]
        //public IActionResult Delete(string id)
        //{
        //    userInfoService.Delete(id);
        //    return Ok(new { delete = id });
        //}
    }
}
