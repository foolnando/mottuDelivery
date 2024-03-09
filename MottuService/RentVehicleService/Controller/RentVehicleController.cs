using MottuService.DataBase;

namespace MottuService.RentVehicleService;
public static class RentVehicleController
{

    public static void AddRentVehicleRouter(this WebApplication app)
    {
        var vehicleService = new VehicleService();
        var rentVehicleServiceValidator = new VehicleValidator();
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

        vehicleRouteGroup.MapPatch("/{id}", async (Guid id, IUpdateVehicleRequest request, MottuDataBaseContext dbContext) => {
            try {
                await vehicleService.UpdatePlateVehicle(id, request.plate, dbContext);
                return Results.Ok();
            } catch (NotFoundException) {
                return Results.NotFound();

            }
        }
        );

        vehicleRouteGroup.MapPost("/rent",async (ICreateRentVehicleRequest request, MottuDataBaseContext dbContext) => {
            try {
                rentVehicleServiceValidator.ValidadeRentVehicleRequest(request);
                await vehicleService.RentVehicle(request, dbContext);
                return Results.Ok();
            } catch(InvalidRequestExeception) {
                return Results.BadRequest();
            }
        });

        vehicleRouteGroup.MapPost("/rent/excharge",async (IGetExpectedValueChargeRequest request, MottuDataBaseContext dbContext) => {
            await vehicleService.GetRentVehicleExcharge(request.rentId, request.endDate, dbContext);
        });
    }
}