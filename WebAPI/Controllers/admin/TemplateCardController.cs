using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using WebAPI.Middlewares;

namespace WebAPI.Controllers.admin
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    [Route("api/admin/templatecard")]
    [ApiController]
    public class TemplateCardController : ControllerBase
    {
        private readonly ITemplateCardService templateCardService;

        public TemplateCardController(ITemplateCardService templateCardService)
        {
            this.templateCardService = templateCardService;
        }

        //[HttpPost]
        //[SwaggerOperation(Summary = "Tạo template card [Admin]")]
        //public IActionResult Create(TemplateCardDTO dto)
        //{
        //    return StatusCode(StatusCodes.Status201Created, templateCardService.Create(dto));
        //}

        //[HttpPut]
        //[SwaggerOperation(Summary = "Chỉnh sửa template card [Admin]")]
        //public IActionResult Update(TemplateCardDTO dto)
        //{
        //    return Ok(templateCardService.Update(dto));
        //}

        //[HttpDelete]
        //[SwaggerOperation(Summary = "Xóa template card (chuyển trạng thái từ false thành true) [Admin]")]
        //public IActionResult Delete(string id)
        //{
        //    templateCardService.Delete(id);
        //    return Ok(new { Deleted = id });
        //}

        //[HttpDelete("permanently-delete")]
        //[SwaggerOperation(Summary = "Xóa hoàn toàn template card ra khỏi database [Admin]")]
        //public IActionResult PermanentlyDelete(string id)
        //{
        //    templateCardService.PermanentlyDelete(id);
        //    return Ok(new { Deleted = id });
        //}

        //[HttpGet("deleted")]
        //[SwaggerOperation(Summary = "Lấy các template card bị xóa (có trạng thái isDeleted = true) [Admin]")]
        //public IActionResult GetAllDeleted()
        //{
        //    return Ok(templateCardService.GetAllDeleted());
        //}

        //[HttpPost("restore")]
        //[SwaggerOperation(Summary = "Phục hồi template card đã xóa (chuyển trạng thái từ true thành false) [Admin]")]
        //public IActionResult restore(string id)
        //{
        //    return Ok(templateCardService.Restore(id));
        //}
    }
}
