namespace Domain.Entity;

public partial class TemplateCard : IEntity
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public double? Width { get; set; }

    public double? Height { get; set; }

    public string? ImageLink { get; set; }

    public string? Data { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual ICollection<UserCard> UserCards { get; set; } = new List<UserCard>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
