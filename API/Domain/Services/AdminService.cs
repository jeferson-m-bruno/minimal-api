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

    public List<Admin> GetAll(int? page)
    {
        int byPage = 10;
        int validPage = page ?? 0;
        var query = this._context.Admins.AsQueryable();
        query = query.Skip(validPage * byPage).Take(byPage);
        return query.ToList();
    }

    public Admin? GetById(int id)
    {
        return this._context.Admins.FirstOrDefault(admin => admin.Id == id);
    }

    public bool Insert(Admin admin)
    {
        this._context.Admins.Add(admin);
        return this._context.SaveChanges() > 0;        
    }

    public Admin? Login(LoginDTO loginDTO)
    {
        return this._context.Admins.Where(admin =>
            admin.Email == loginDTO.Email
            && admin.Password == loginDTO.Password
        ).FirstOrDefault();
    }
}