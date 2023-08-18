namespace Domain.Entity;

public partial class UserInfo : IEntity
{
    public string Id { get; set; } = null!;

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? FullName { get; set; }

    public bool? IsAdmin { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual ICollection<UserCard> UserCards { get; set; } = new List<UserCard>();

    public virtual ICollection<UserUpload> UserUploads { get; set; } = new List<UserUpload>();
}
