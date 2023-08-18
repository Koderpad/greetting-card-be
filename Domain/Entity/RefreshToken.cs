namespace Domain.Entity;

public partial class RefreshToken
{
    public string Id { get; set; } = null!;

    public string? FamilyId { get; set; }

    public string? UserId { get; set; }

    public bool? IsActivated { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public virtual UserInfo? User { get; set; }
}
