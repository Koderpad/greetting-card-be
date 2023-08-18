namespace Domain.DTO;

public class UserInfoDTO
{
    public string? Id { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? FullName { get; set; }

    public bool? IsAdmin { get; set; }
}
