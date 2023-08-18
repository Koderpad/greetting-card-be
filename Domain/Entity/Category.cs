namespace Domain.Entity;

public partial class Category : IEntity
{
    public string Id { get; set; } = null!;

    public string? ParentId { get; set; }

    public string? Name { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Category> InverseParent { get; set; } = new List<Category>();

    public virtual Category? Parent { get; set; }

    public virtual ICollection<SampleGreeting> SampleGreetings { get; set; } = new List<SampleGreeting>();

    public virtual ICollection<TemplateCard> TemplateCards { get; set; } = new List<TemplateCard>();
}
