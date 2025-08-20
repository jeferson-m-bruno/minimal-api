using MinimalAPI.Domain.DTOs;
using MinimalAPI.Domain.Entities;

namespace MinimalAPI.Domain.Interfaces;

public interface IAdminService
{
    Admin? Login(LoginDTO loginDTO);
}