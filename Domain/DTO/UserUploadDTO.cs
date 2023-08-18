namespace Domain.DTO;

public class UserUploadDTO
{
    public string? Id { get; set; }

    public string UserId { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public string FileLink { get; set; } = null!;
}
