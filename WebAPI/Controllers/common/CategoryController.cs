using Domain.ExceptionModel;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Middlewares;

namespace WebAPI.Controllers.common
{
    [TypeFilter(typeof(AuthenticationFilter))]
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        //[HttpGet]
        //[SwaggerOperation(Summary = "Lấy tất cả Category")]
        //public IActionResult GetAll()
        //{
        //    return Ok(categoryService.GetAll());
        //}

        [HttpGet("theme")]
        [SwaggerOperation(Summary = "Lấy tất cả danh mục có danh mục cha là chủ đề")]
        public IActionResult GetAllThemeCategory()
        {
            return Ok(categoryService.GetAllThemeCategory());
        }

        //[HttpGet("{id}")]
        //[SwaggerOperation(Summary = "Lấy Category theo id")]
        //public IActionResult GetById(string id)
        //{
        //    return Ok(categoryService.GetById(id));
        //}

        //[HttpGet("search")]
        //[SwaggerOperation(Summary = "Tìm kiếm Category theo tên")]
        //public IActionResult GetByName(string name)
        //{
        //    if (User.IsInRole("Admin") || User.IsInRole("Common"))
        //    {
        //        return Ok(categoryService.GetByName(name));
        //    }
        //    throw new InvalidTokenException();
        //}

        //[HttpGet("{id}/template-card")]
        //[SwaggerOperation(Summary = "Lấy tất cả template card theo category id (có phân trang)")]
        //public IActionResult GetTemplateCardByCategoryId(string id, int offset, int pagesize)
        //{
        //    if (User.IsInRole("Admin") || User.IsInRole("Common"))
        //    {
        //        return Ok(categoryService.GetTemplateCardByCategoryId(id, offset, pagesize));
        //    }
        //    throw new InvalidTokenException();
        //}
        [HttpGet("{id}/sample-greeting")]
        [SwaggerOperation(Summary = "Lấy tất cả lời chúc mẫu theo danh mục")]
        public IActionResult GetSampleGreetingsByCategoryId(string id, int offset, int pagesize)
        {
            if (User.IsInRole("Admin") || User.IsInRole("Common"))
            {
                return Ok(categoryService.GetSampleGreetingsByCategoryId(id, offset, pagesize));
            }
            throw new InvalidTokenException();
        }
    }
}
