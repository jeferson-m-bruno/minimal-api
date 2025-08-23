using MinimalAPI.Domain.DTOs;
using MinimalAPI.Domain.Entities;

namespace MinimalAPI.Domain.Interfaces;

public interface IAdminService
{
    Admin? Login(LoginDTO loginDTO);
    bool Insert(Admin admin);
    List<Admin> GetAll(int? page);
    Admin? GetById(int id);
}