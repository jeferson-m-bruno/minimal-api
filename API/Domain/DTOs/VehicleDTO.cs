
namespace MinimalAPI.Domain.DTOs;

public record VehicleDTO
{    
    public string Name { get; set; } = default!;    
    public string Brand { get; set; } = default!;       
    public int Year { get; set; } = default!;

    public List<String> Validation()
    {
        List<string> messages = new List<string>();
        if (String.IsNullOrEmpty(this.Name))
        {
            messages.Add("Nome inválido!");
        }
        if (String.IsNullOrEmpty(this.Brand))
        {
            messages.Add("Marca inválido!");
        }
        if (this.Year < 1900)
        {
            messages.Add("Ano inválido!");
        }

        return messages;
    }
}