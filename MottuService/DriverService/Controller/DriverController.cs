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
            Console.WriteLine(request);
            var registerDriverResult = await driverService.CreateDriver(request, dbContext);     
            return registerDriverResult;

        }).DisableAntiforgery();

        driverRouteGroup.MapPatch("/cnhImage/{id}", async (Guid id, [Microsoft.AspNetCore.Mvc.FromForm] IFormFile cnhImage, MottuDataBaseContext dbContext) =>
        {
            if (cnhImage.Length == 0)
            {
                return Results.BadRequest("Image file must be sent.");
            }
            if (cnhImage.ContentType != "image/png" && cnhImage.ContentType != "image/bmp")
            {
                return Results.BadRequest("The file must be in PNG or BMP format");
            }
            await driverService.UpdateDriverCnhPicture(cnhImage, id, dbContext);
            return Results.Ok();
        }).DisableAntiforgery();


        driverRouteGroup.MapGet("/{cnpj}", async (string cnpj, MottuDataBaseContext dbContext) =>
        {   
           try {
            var driver = await driverService.GetDriver(cnpj, dbContext);
            return Results.Ok(driver);
            }
            catch (NotFoundException)
            {
                return Results.NotFound();
            }
           });
    }
}