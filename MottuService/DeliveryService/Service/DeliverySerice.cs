using MottuService.DataBase;

public class DeliveryService {
    public async Task CreateDeliveryOrder(ICreateDeliveryOrderRequest request, MottuDataBaseContext dbContext)
    {
        var deliveryOrder = new Order(request.value);
        await dbContext.Orders.AddAsync(deliveryOrder);
        await dbContext.SaveChangesAsync();
    }
}