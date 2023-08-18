using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using WebAPI.Middlewares;

namespace WebAPI.Controllers.admin
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    [Route("api/admin/samplegreeting")]
    [ApiController]
    public class SampleGreetingController : ControllerBase
    {
        private readonly ISampleGreetingService sampleGreetingService;

        public SampleGreetingController(ISampleGreetingService sampleGreetingService)
        {
            this.sampleGreetingService = sampleGreetingService;
        }

        //[HttpPost]
        //[SwaggerOperation(Summary = "Tạo sample greeting [Admin]")]
        //public IActionResult Create(SampleGreetingDTO dto)
        //{
        //    return StatusCode(StatusCodes.Status201Created, sampleGreetingService.Create(dto));
        //}

        //[HttpPut]
        //[SwaggerOperation(Summary = "Chỉnh sửa sample greeting [Admin]")]
        //public IActionResult Update(SampleGreetingDTO dto)
        //{
        //    return Ok(sampleGreetingService.Update(dto));
        //}

        //[HttpDelete]
        //[SwaggerOperation(Summary = "Xóa sample greeting (chuyển trạng thái từ false thành true) [Admin]")]
        //public IActionResult Delete(string id)
        //{
        //    sampleGreetingService.Delete(id);
        //    return Ok(new { Deleted = id });
        //}

        //[HttpDelete("permanently-delete")]
        //[SwaggerOperation(Summary = "Xóa hoàn toàn sample greeting khỏi database [Admin]")]
        //public IActionResult PermanentlyDelete(string id)
        //{
        //    sampleGreetingService.PermanentlyDelete(id);
        //    return Ok(new { Deleted = id });
        //}

        //[HttpGet("deleted")]
        //[SwaggerOperation(Summary = "Lấy các sample greeting bị xóa (có trạng thái isDeleted = true) [Admin]")]
        //public IActionResult GetAllDeleted()
        //{
        //    return Ok(sampleGreetingService.GetAllDeleted());
        //}

        //[HttpPost("restore")]
        //[SwaggerOperation(Summary = "Phục hồi sample greeting đã xóa (chuyển trạng thái từ true thành false) [Admin]")]
        //public IActionResult restore(string id)
        //{
        //    return Ok(sampleGreetingService.Restore(id));
        //}
    }
}
