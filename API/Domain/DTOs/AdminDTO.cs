using MinimalAPI.Domain.Enums;

namespace MinimalAPI.Domain.DTOs;

public class AdminDTO
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;

    public PerfilEnum? Perfil { get; set; } = default!;

    public List<String> Validation()
    {
        List<string> messages = new List<string>();
        if (String.IsNullOrEmpty(this.Email))
        {
            messages.Add("E-mail inválido!");
        }
        if (String.IsNullOrEmpty(this.Password))
        {
            messages.Add("Password inválido");
        }
        if (Perfil == null)
        {
            messages.Add("Perfil inválido");
        }

        return messages;
    }
}