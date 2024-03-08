using MottuService.DataBase;

namespace MottuService.RentVehicleService;
public static class RentVehicleController
{

    public static void AddRentVehicleRouter(this WebApplication app)
    {
        var vehicleService = new VehicleService();
        var vehicleRouteGroup = app.MapGroup("vehicle");
        
        vehicleRouteGroup.MapPost("", async (ICreateVehicleRequest request, MottuDataBaseContext dbContext) =>
        {
            try {
                await vehicleService.CreateVehicle(request, dbContext);
                return Results.Ok();
            } catch(ConflictException) {
                return Results.Conflict();
            }
            

        });

        vehicleRouteGroup.MapGet("/{plate}", async (string plate, MottuDataBaseContext dbContext) =>
        {
           try {
            var vehicles = await vehicleService.GetVehicle(plate, dbContext);
            return Results.Ok(vehicles);
            }
            catch (NotFoundException)
            {
                return Results.NotFound();
            }
           });
    }
}