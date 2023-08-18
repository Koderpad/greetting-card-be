namespace Domain.Entity;

public partial class SampleGreeting : IEntity
{
    public string Id { get; set; } = null!;

    public string? CategoryId { get; set; }

    public string? Data { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Category? Category { get; set; }
}
