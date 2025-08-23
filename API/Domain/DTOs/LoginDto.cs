namespace MinimalAPI.Domain.DTOs;

public class LoginDTO
{
    public String Email { get; set; } = default!;
    public String Password { get; set; } = default!;
}
