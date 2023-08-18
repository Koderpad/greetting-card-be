using Domain.DTO;
using Domain.ExceptionModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Abstract;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Middlewares;

namespace WebAPI.Controllers.common
{
    [TypeFilter(typeof(AuthenticationFilter))]
    [Route("api/usercard")]
    [ApiController]
    public class UserCardController : ControllerBase
    {
        private readonly IUserCardService userCardService;

        public UserCardController(IUserCardService userCardService)
        {
            this.userCardService = userCardService;
        }

        //[HttpGet]
        //[SwaggerOperation(Summary = "Lấy tất cả user card (có phân trang)")]
        //public IActionResult Get(int offset, int pagesize)
        //{
        //    return Ok(userCardService.Get(offset, pagesize));
        //}

        [HttpGet("{userCardId}")]
        [SwaggerOperation(Summary = "Lấy user card theo id")]
        public IActionResult GetById(string userId, string userCardId)
        {
            var uId = User.Claims.FirstOrDefault(u => u.Type == "uid")!.Value;
            if (uId != userId)
            {
                throw new AccessDeniedException();
            }
            return Ok(userCardService.GetById(userCardId));
        }

        [HttpPost("create")]
        [SwaggerOperation(Summary = "Tạo thiệp")]
        public IActionResult Create()
        {
            var formData = Request.Form;
            UserCardDTO userCardDTO = JsonConvert.DeserializeObject<UserCardDTO>(formData["user-card"]!)!;
            IFormFile image = formData.Files["image"]!;
            List<FileUploadModel> items = new List<FileUploadModel>();
            foreach (var file in formData.Files)
            {
                FileUploadModel item = new FileUploadModel
                {
                    ItemId = file.Name,
                    FileDetails = file
                };
                items.Add(item);
            }
            return StatusCode(StatusCodes.Status201Created, userCardService.Create(userCardDTO, image, items));
        }

        [HttpPost("update")]
        [SwaggerOperation(Summary = "Cập nhật thiệp (Cập nhật thiệp r nhưng mà chưa xóa được ảnh cũ :v)")]
        public IActionResult Update()
        {
            var formData = Request.Form;
            UserCardDTO userCardDTO = JsonConvert.DeserializeObject<UserCardDTO>(formData["user-card"]!)!;
            IFormFile image = formData.Files["image"]!;
            List<FileUploadModel> items = new List<FileUploadModel>();
            foreach (var file in formData.Files)
            {
                FileUploadModel item = new FileUploadModel
                {
                    ItemId = file.Name,
                    FileDetails = file
                };
                items.Add(item);
            }
            return StatusCode(StatusCodes.Status201Created, userCardService.Update(userCardDTO, image, items));
        }

        //[HttpGet("search")]
        //public IActionResult GetByName(string userId)
        //{
        //    if (User.IsInRole("Admin") || User.IsInRole("Common"))
        //    {
        //        return Ok(userCardService.GetByUserInfoId(userId));
        //    }
        //    throw new InvalidTokenException();
        //}
    }
}
