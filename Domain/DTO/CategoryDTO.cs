namespace Domain.DTO;

public class CategoryDTO
{
    public string? Id { get; set; }

    public string? ParentId { get; set; }

    public string Name { get; set; } = null!;
}
