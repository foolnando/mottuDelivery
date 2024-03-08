using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuService.DataBase;

public class VehicleService
{

    public async Task<bool> IsPlateRegistered(string plate, MottuDataBaseContext dbContext)
    {
        return await dbContext.Vehicles.AnyAsync(vehicle => vehicle.Plate == plate);
    }
    public async Task<IResult> CreateVehicle(ICreateVehicleRequest request, MottuDataBaseContext dbContext)
    {
        var plateAlreadyExists = await IsPlateRegistered(request.plate, dbContext);
        if (plateAlreadyExists)
        {
            return Results.Conflict("Can not register vehicle, this plate it is already registered");
        }

        var vehicle = new Vehicle(request.plate, request.model, request.year);
        await dbContext.Vehicles.AddAsync(vehicle);
        await dbContext.SaveChangesAsync();
        return Results.Ok("Motocycle registered");
    }
}
