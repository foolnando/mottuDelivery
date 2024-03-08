using MottuService.DataBase;

namespace MottuService.RentVehicleService;
public static class DriverController
{

    public static void AddDriverRoute(this WebApplication app)
    {
        var driverService = new DriverService();
        var driverRouteGroup = app.MapGroup("driver");
        driverRouteGroup.MapPost("", async (ICreateDriverRequest request, MottuDataBaseContext dbContext) =>
        {
            var registerDriverResult = await driverService.CreateDriver(request, dbContext);
            Console.WriteLine(request);
            return registerDriverResult;

        });
    }
}