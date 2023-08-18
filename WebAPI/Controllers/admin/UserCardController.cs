using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using WebAPI.Middlewares;

namespace WebAPI.Controllers.admin
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    [Route("api/admin/usercard")]
    [ApiController]
    public class UserCardController : ControllerBase
    {
        private readonly IUserCardService userCardService;

        public UserCardController(IUserCardService userCardService)
        {
            this.userCardService = userCardService;
        }

        //[HttpPost]
        //[SwaggerOperation(Summary = "Tạo user card [Admin]")]
        //public IActionResult Create([FromForm(Name ="user-card")] string userCard, [FromForm] IFormFile image, [FromForm] List<FileUploadModel> item)
        //{
        //    UserCardDTO userCardDTO = JsonConvert.DeserializeObject<UserCardDTO>(userCard)!;
        //    return StatusCode(StatusCodes.Status201Created, userCardService.Create(userCardDTO, image, item));
        //}

        //[HttpPut]
        //[SwaggerOperation(Summary = "Chỉnh sửa user card [Admin]")]
        //public IActionResult Update(UserCardDTO dto)
        //{

        //    return Ok(userCardService.Update(dto));
        //}

        //[HttpDelete]
        //[SwaggerOperation(Summary = "Xóa user card (chuyển trạng thái từ false thành true) [Admin]")]
        //public IActionResult Delete(string id)
        //{
        //    userCardService.Delete(id);
        //    return Ok(new { Deleted = id });
        //}

        //[HttpDelete("permanently-delete")]
        //[SwaggerOperation(Summary = "Xóa hoàn toàn user card ra khỏi database [Admin]")]
        //public IActionResult PermanentlyDelete(string id)
        //{
        //    userCardService.PermanentlyDelete(id);
        //    return Ok(new { Deleted = id });
        //}
    }
}
