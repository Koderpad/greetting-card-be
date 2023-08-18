using Microsoft.AspNetCore.Http;

namespace Domain.DTO
{
    public class FileUploadModel
    {
        public IFormFile? FileDetails { get; set; }
        public string? ItemId { get; set; }
    }
}
