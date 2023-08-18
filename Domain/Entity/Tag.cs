namespace Domain.Entity;

public partial class Tag : IEntity
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual ICollection<TemplateCard> TemplateCards { get; set; } = new List<TemplateCard>();
}
