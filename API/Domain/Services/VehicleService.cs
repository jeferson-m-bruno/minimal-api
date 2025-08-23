using Microsoft.EntityFrameworkCore;
using MinimalAPI.Domain.Entities;
using MinimalAPI.Domain.Interfaces;
using MinimalAPI.Infra.DB;

namespace MinimalAPI.Domain.Services;

public class VehicleService : IVehicleService
{
    private readonly MyContext _context;

    public VehicleService(MyContext context)
    {
        this._context = context;
    }

    public bool Delete(int id)
    {
        var vehicle = this._context.Vehicles.FirstOrDefault(obj => obj.Id == id);
        if (vehicle == null)
            return false;

        this._context.Vehicles.Remove(vehicle);
        return this._context.SaveChanges() > 0;
    }

    public bool Insert(Vehicle vehicle)
    {
        this._context.Vehicles.Add(vehicle);
        return this._context.SaveChanges() > 0;
    }

    public bool Update(Vehicle vehicle)
    {
        this._context.Vehicles.Update(vehicle);
        return this._context.SaveChanges() > 0 ;

    }

    public List<Vehicle> GetAll(int page = 0, string? searchName = null, string? searchBrand = null)
    {
        int byPage = 10;
        var query = this._context.Vehicles.AsQueryable();
        if (!string.IsNullOrEmpty(searchName))
        {
            query = query.Where(search => EF.Functions.Like(search.Name.ToLower(), $"%{searchName}%"));
        }
        if (!string.IsNullOrEmpty(searchBrand))
        {
            query = query.Where(search => EF.Functions.Like(search.Brand.ToLower(), $"%{searchBrand}%"));
        }

        query = query.Skip(page * byPage).Take(byPage);
        return query.ToList();
    }

    public Vehicle? GetById(int id)
    {
        return this._context.Vehicles.FirstOrDefault(vehicle => vehicle.Id == id);
    }


}