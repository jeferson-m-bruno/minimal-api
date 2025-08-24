using MinimalAPI.Domain.DTOs;
using MinimalAPI.Domain.Entities;
using MinimalAPI.Domain.Interfaces;

namespace TestMinimalApi.Mocks;

public class AdminServiceMock :  IAdminService
{
    private static List<Admin> dbAdmins = new List<Admin>(){
        new Admin{
            Id = 1,
            Email = "adm@teste.com",
            Password = "123456",
            Perfil = "Adm"
        },
        new Admin{
            Id = 2,
            Email = "editor@teste.com",
            Password = "123456",
            Perfil = "Editor"
        }
    };
    public Admin? Login(LoginDTO loginDTO)
    {
        return dbAdmins.Find(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password);
    }

    public bool Insert(Admin admin)
    {
        admin.Id = dbAdmins.Count() + 1;
        dbAdmins.Add(admin);
        return true;
    }

    public List<Admin> GetAll(int? page)
    {
        return dbAdmins;
    }

    public Admin? GetById(int id)
    {
        return dbAdmins.Find(a => a.Id == id);
    }
}