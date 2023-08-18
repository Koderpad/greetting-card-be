using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Middlewares;

namespace WebAPI.Controllers.common
{
    [TypeFilter(typeof(AuthenticationFilter))]
    [Route("api/templatecard")]
    [ApiController]
    public class TemplateCardController : ControllerBase
    {
        private readonly ITemplateCardService templateCardService;

        public TemplateCardController(ITemplateCardService templateCardService)
        {
            this.templateCardService = templateCardService;
        }

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    return Ok(templateCardService.GetAll());
        //}

        //[HttpGet("search")]
        //public IActionResult GetByName(string name)
        //{
        //    if (User.IsInRole("Admin") || User.IsInRole("Common"))
        //    {
        //        return Ok(templateCardService.GetByName(name));
        //    }
        //    throw new InvalidTokenException();
        //}

        [HttpGet]
        public IActionResult Get(int offset, int pagesize)
        {
            return Ok(templateCardService.Get(offset, pagesize));
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Lấy tất cả template card theo id")]
        public IActionResult GetById(string id)
        {
            return Ok(templateCardService.GetById(id));
        }
        //[HttpGet("search")]
        //public IActionResult GetByTag([FromQuery] string tag)
        //{
        //    var templateCards = templateCardService.GetByTag(tag);
        //    return Ok(templateCards);
        //}
    }
}
