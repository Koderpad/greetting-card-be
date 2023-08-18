namespace Domain.DTO;

public class RefreshTokenDTO
{
    public string? Id { get; set; }

    public string UserId { get; set; } = null!;

    public string? Value { get; set; }

    public DateTime ExpirationDate { get; set; }
}
