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
            var registerVehicelResult = await vehicleService.CreateVehicle(request, dbContext);
            Console.WriteLine(registerVehicelResult);
            return registerVehicelResult;

        });
    }
}