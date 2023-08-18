using Newtonsoft.Json.Linq;

namespace Domain.DTO;

public class TemplateCardDTO
{
    public string? Id { get; set; }

    public string Name { get; set; } = null!;

    public double Width { get; set; }

    public double Height { get; set; }

    public string Image { get; set; } = null!;

    public JObject? JsonData { get; set; }

    public virtual List<string> CategoryIdList { get; set; } = new List<string>();
    public virtual List<string> TagIdList { get; set; } = new List<string>();

}
