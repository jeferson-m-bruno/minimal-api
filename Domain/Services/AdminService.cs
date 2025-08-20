using System.Data.Common;
using MinimalAPI.Domain.DTOs;
using MinimalAPI.Domain.Entities;
using MinimalAPI.Domain.Interfaces;
using MinimalAPI.Infra.DB;

namespace MinimalAPI.Domain.Services;

public class AdminService : IAdminService
{
    private readonly MyContext _context;
    public AdminService(MyContext context)
    {
        this._context = context;
    }

    public Admin? Login(LoginDTO loginDTO)
    {
        return this._context.Admins.Where(admin =>
            admin.Email == loginDTO.Email
            && admin.Password == loginDTO.Password
        ).FirstOrDefault();
    }
}