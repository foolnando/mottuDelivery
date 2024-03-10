public class Order
{
    public Guid Id { get; init; }
    public DateOnly DeliveryDate  { get; private set; }
    public DateOnly CreatedAt  { get; private set; }

    public string Status { get; private set; }
    public double Value { get; private set; }
    public Guid RentId { get; private set; }
    public RentDriverVehicle Rent { get; set; }
    public ICollection<OrderNotification> Notifications { get; set; }



    public Order(double value)
    {
       Value = value;
       CreatedAt = DateOnly.FromDateTime(DateTime.Now);
       Status = "disponivel";
       Id = Guid.NewGuid();
       Notifications = new List<OrderNotification>();

    }

}