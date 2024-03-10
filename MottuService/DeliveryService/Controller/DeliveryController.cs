using MottuService.DataBase;

namespace MottuService.RentVehicleService;
public static class DeliveyController
{

    public static void AddDeliveyRoute(this WebApplication app)
    {
        var deliveryervice = new DriverService();
        var deliveryRouteGroup = app.MapGroup("delivery");

    }
}