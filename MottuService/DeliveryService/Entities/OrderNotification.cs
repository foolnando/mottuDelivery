public class OrderNotification
{
    public Guid Id { get; init; }
    public Guid OrderId { get; private set; }
    public Guid DriverId { get; private set; }

    public Order Order { get; set; }
    public Driver Driver { get; set; }




    public OrderNotification(Guid driverId, Guid orderId)
    {
       OrderId =  orderId;
       DriverId = driverId;
       
       Id = Guid.NewGuid();

    }

}