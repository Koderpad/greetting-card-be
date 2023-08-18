using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using WebAPI.Middlewares;

namespace WebAPI.Controllers.admin
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    [Route("api/admin/userupload")]
    [ApiController]
    public class UserUploadController : Controller
    {
        private readonly IUserUploadService userUploadService;

        public UserUploadController(IUserUploadService userUploadService)
        {
            this.userUploadService = userUploadService;
        }
        //[HttpGet]
        //[SwaggerOperation(Summary = "Lấy tất cả User Upload [Admin]")]
        //public IActionResult GetAll()
        //{
        //    if (User.IsInRole("Admin") || User.IsInRole("Common"))
        //    {
        //        return Ok(userUploadService.GetAll());
        //    }
        //    throw new InvalidTokenException();
        //}

        //[HttpGet("{id}")]
        //[SwaggerOperation(Summary = "Lấy User Upload theo id [Admin]")]
        //public IActionResult GetById(string id)
        //{
        //    if (User.IsInRole("Admin") || User.IsInRole("Common"))
        //    {
        //        return Ok(userUploadService.GetById(id));
        //    }
        //    throw new InvalidTokenException();
        //}

        //[HttpPost]
        //[SwaggerOperation(Summary = "Tạo User Upload [Admin]")]
        //public IActionResult Create(UserUploadDTO dto)
        //{
        //    if (User.IsInRole("Admin"))
        //    {
        //        return StatusCode(
        //                        StatusCodes.Status201Created,
        //                        userUploadService.Create(dto));
        //    }
        //    else if (User.IsInRole("Common"))
        //    {
        //        throw new AccessDeniedException();
        //    }
        //    else
        //    {
        //        throw new InvalidTokenException();
        //    }
        //}

        //[HttpDelete]
        //[SwaggerOperation(Summary = "Xóa User Upload [Admin]")]
        //public IActionResult Delete(string id) { 
        //    if (User.IsInRole("Admin"))
        //    {
        //        userUploadService.Delete(id);
        //        return Ok(new { delete = id });
        //    }
        //    else if (User.IsInRole("Common"))
        //    {
        //        throw new AccessDeniedException();
        //    }
        //    else
        //    {
        //        throw new InvalidTokenException();
        //    }
        //}
    }
}
