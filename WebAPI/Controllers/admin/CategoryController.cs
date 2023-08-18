using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using WebAPI.Middlewares;

namespace WebAPI.Controllers.admin
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    [Route("api/admin/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        //[HttpPost]
        //[SwaggerOperation(Summary = "Tạo Category [Admin]")]
        //public IActionResult Create(CategoryDTO dto)
        //{
        //    return StatusCode(StatusCodes.Status201Created, categoryService.Create(dto));
        //}

        //[HttpPut]
        //[SwaggerOperation(Summary = "Chỉnh sửa Category [Admin]")]
        //public IActionResult Update(CategoryDTO dto)
        //{
        //    return Ok(categoryService.Update(dto));
        //}

        //[HttpDelete]
        //[SwaggerOperation(Summary = "Xóa Category (chuyển trạng thái từ false thành true) [Admin]")]
        //public IActionResult Delete(string id)
        //{
        //    categoryService.Delete(id);
        //    return Ok(new { deleted = id });
        //}

        //[HttpDelete("permanently-delete")]
        //[SwaggerOperation(Summary = "Xóa hoàn toàn Category ra khỏi database [Admin]")]
        //public IActionResult PermanentlyDelete(string id)
        //{
        //    categoryService.PermanentlyDelete(id);
        //    return Ok(new { deleted = id });
        //}

        //[HttpGet("deleted")]
        //[SwaggerOperation(Summary = "Lấy các category bị xóa (có trạng thái isDeleted = true) [Admin]")]
        //public IActionResult GetAllDeleted()
        //{
        //    return Ok(categoryService.GetAllDeleted());
        //}

        //[HttpPost("restore")]
        //[SwaggerOperation(Summary = "Phục hồi Category đã xóa (chuyển trạng thái từ true thành false) [Admin]")]
        //public IActionResult restore(string id)
        //{
        //    return Ok(categoryService.Restore(id));
        //}

        //[HttpGet("{id}/TemplateCard")]
        //public IActionResult GetAllTemplateCardByCategoryId(string id)
        //{
        //    return Ok(categoryService.GetAllTemplateCardByCategoryId(id));
        //}
    }
}
