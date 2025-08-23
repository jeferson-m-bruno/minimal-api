using MinimalAPI.Domain.Entities;

namespace MinimalAPI.Domain.ViewModels;

public class AdminViewModel
{
    public AdminViewModel()
    {
        
    }
    public AdminViewModel(Admin admin)
    {
        this.Id = admin.Id;
        this.Email = admin.Email;
        this.Perfil = admin.Perfil;
    }
    public int Id { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Perfil { get; set; } = default!;
}