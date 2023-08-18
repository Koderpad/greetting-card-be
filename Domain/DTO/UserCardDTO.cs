using Newtonsoft.Json.Linq;

namespace Domain.DTO;

public class UserCardDTO
{
    public string? Id { get; set; }

    public string UserId { get; set; } = null!;

    public string? TemplateCardId { get; set; }

    public string? ImageLink { get; set; }

    public JObject? JsonData { get; set; }
}
