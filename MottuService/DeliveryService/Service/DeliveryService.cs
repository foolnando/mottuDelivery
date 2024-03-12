using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Microsoft.EntityFrameworkCore;
using MottuService.DataBase;
using MottuService.Notifications;

public class DeliveryService {
    public BasicAWSCredentials credentials = new BasicAWSCredentials("AKIA2UC3E3CMH5XWUUU2", "oGQ91BUzuoTHD3Z4FsHhKY+HeKqqlvUm/6CL4rZU");
    public AmazonSQSClient sqsClient;
    public SqsPublisher publisher;


     public DeliveryService()
    {
        sqsClient = new AmazonSQSClient(credentials, RegionEndpoint.USEast1);
        publisher = new SqsPublisher(sqsClient);

    }
    public async Task<Guid> GetRentDriverId(Guid rentId, MottuDataBaseContext dbContext){
        var rent = await dbContext.Rents.SingleOrDefaultAsync(rent => rent.Id == rentId);
        if (rent == null) {
            throw new NotFoundException();
        }
        return rent.DriverId;
    }
    public async Task isDriverNotified(Guid rentId, Guid orderId, MottuDataBaseContext dbContext){
        var driverId = await GetRentDriverId(rentId, dbContext);
        var notification = await dbContext.
                                Notifications.
                                SingleOrDefaultAsync(
                                notification => 
                                notification.OrderId == orderId 
                                && notification.DriverId ==driverId);

        if (notification == null) {
            throw new NotFoundException();
        }
        return;

    }
    public async Task UpdateRentStatus(Guid id, string status, MottuDataBaseContext dbContext){
        var rent = await dbContext.Rents.SingleOrDefaultAsync(rent => rent.Id == id);
        if (rent == null) {
            throw new NotFoundException();
        }
       rent.UpdateRentVehicleStatus(status);
       await dbContext.SaveChangesAsync();
    }

    public async Task UpdateOrderStatus(Guid id, string status, MottuDataBaseContext dbContext){
        var order = await dbContext.Orders.SingleOrDefaultAsync(order => order.Id == id);
        if (order == null) {
            throw new NotFoundException();
        }
       order.UpdateOrderStatus(status);
       await dbContext.SaveChangesAsync();
    }
    public async Task UpdateOrderDeliveryDate(Guid id, MottuDataBaseContext dbContext){
        var order = await dbContext.Orders.SingleOrDefaultAsync(order => order.Id == id);
        if (order == null) {
            throw new NotFoundException();
        }
       order.UpdateOrderDeliveryDate(DateOnly.FromDateTime(DateTime.Today));
       await dbContext.SaveChangesAsync();
    }

    public async Task HandleOrderDelivery(IConfirmOrderRequest request, MottuDataBaseContext dbContext, string orderStatus, string rentStatus) {
    var orderId = request.orderId;
    var rentId = request.rentId;

    await UpdateOrderStatus(orderId, orderStatus, dbContext);
    await UpdateRentStatus(rentId, rentStatus, dbContext);
}

    public async Task PushDeliveryNotificationAsync(Guid driverId, Guid orderId){
        await publisher.PublishAync("mottu-push-notification-queue",new NotificationMessage{
            DriverId = driverId,
            OrderId = orderId

        });
    }
    public async Task CreateDeliveryOrder(ICreateDeliveryOrderRequest request, MottuDataBaseContext dbContext)
    {
        var deliveryOrder = new Order(request.value);
        await dbContext.Orders.AddAsync(deliveryOrder);
        await dbContext.SaveChangesAsync();
        var driversAvailable = await dbContext.Rents.Where(rent => rent.Status == "available").ToListAsync();
        Console.WriteLine(driversAvailable.Count());
        

        foreach (var rent in driversAvailable){
            if(deliveryOrder.Status == "disponivel"){
                await PushDeliveryNotificationAsync(rent.DriverId, deliveryOrder.Id);
            }
        }
      
    }
}