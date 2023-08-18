namespace Domain.DTO;

public class SampleGreetingDTO
{
    public string? Id { get; set; }

    public string CategoryId { get; set; } = null!;

    public string? Data { get; set; }
    public virtual List<string> TagIdList { get; set; } = new List<string>();

}
