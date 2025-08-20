using MinimalAPI.Domain.Entities;

namespace MinimalAPI.Domain.Interfaces;

public interface IVehicleService
{
    List<Vehicle> GetAll(int page = 0, string? searchName = null, string? searchBrand = null);
    Vehicle? GetById(int id);
    bool Insert(Vehicle vehicle);
    bool Update(Vehicle vehicle);
    bool Delete(int id);

}