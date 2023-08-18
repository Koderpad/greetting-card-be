using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Middlewares;

namespace WebAPI.Controllers.common
{
    [TypeFilter(typeof(AuthenticationFilter))]
    [Route("api/samplegreeting")]
    [ApiController]
    public class SampleGreetingController : ControllerBase
    {
        private readonly ISampleGreetingService sampleGreetingService;

        public SampleGreetingController(ISampleGreetingService sampleGreetingService)
        {
            this.sampleGreetingService = sampleGreetingService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Lấy tất cả sample greeting (có phân trang)")]
        public IActionResult GetAll(int offset, int pagesize)
        {
            return Ok(sampleGreetingService.Get(offset, pagesize));
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Lấy sample greeting theo id")]
        public IActionResult GetById(string id)
        {
            return Ok(sampleGreetingService.GetById(id));
        }
    }
}
