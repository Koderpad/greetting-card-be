namespace Domain.Entity;

public partial class UserUpload : IEntity
{
    public string Id { get; set; } = null!;

    public string? UserId { get; set; }

    public string? FileLink { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual UserInfo? User { get; set; }
}
