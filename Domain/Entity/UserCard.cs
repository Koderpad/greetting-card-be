namespace Domain.Entity;

public partial class UserCard : IEntity
{
    public string Id { get; set; } = null!;

    public string? UserId { get; set; }

    public string? TemplateCardId { get; set; }

    public string? ImageLink { get; set; }

    public string? Data { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual TemplateCard? TemplateCard { get; set; }

    public virtual UserInfo? User { get; set; }
}
