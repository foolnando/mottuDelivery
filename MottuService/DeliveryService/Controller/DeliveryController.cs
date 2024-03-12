using MottuService.DataBase;

namespace MottuService.RentVehicleService;
public static class DeliveyController
{

    public static void AddDeliveryRoute(this WebApplication app)
    {
        var deliveryService = new DeliveryService();
        var deliveryRouteGroup = app.MapGroup("delivery");
        deliveryRouteGroup.MapPost("", async (ICreateDeliveryOrderRequest request, MottuDataBaseContext dbContext) =>
        {
    
            try {
                await deliveryService.CreateDeliveryOrder(request, dbContext);
                return Results.Ok();
            } catch(ConflictException) {
                return Results.Conflict();
            }
    
        });
        deliveryRouteGroup.MapPost("/accept", async (IConfirmOrderRequest request, MottuDataBaseContext dbContext) =>
        {
    
            try {
                await deliveryService.isDriverNotified(request.rentId, request.orderId, dbContext);
                await deliveryService.HandleOrderDelivery(request, dbContext, "aceito", "unavailable");
                return Results.Ok();
            } catch(NotFoundException) {
                return Results.Conflict("Driver was not notified");
            }
        });

        deliveryRouteGroup.MapPost("/confirm", async (IConfirmOrderRequest request, MottuDataBaseContext dbContext) =>
        {
    
            try {
                await deliveryService.isDriverNotified(request.rentId, request.orderId, dbContext);
                await deliveryService.HandleOrderDelivery(request, dbContext, "entregue", "available");
                await deliveryService.UpdateOrderDeliveryDate(request.orderId, dbContext);
                return Results.Ok();
            } catch(NotFoundException) {
                return Results.Conflict("Driver was not notified");
            }
        });
    }
}