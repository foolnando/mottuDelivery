public class RentDriverVehicle
{
    public Guid Id { get; init; }
    public Guid VehicleId { get; private set; }
    public Vehicle Vehicle { get; set; }
    public Guid DriverId { get; private set; }
    public Driver Driver { get; set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public float Value { get; private set; }
    public string Status { get; private set; }
    public DateOnly ExpectedEndDate { get; private set; }
    public string Category { get; private set; }



    public RentDriverVehicle(Guid vehicleId, 
                             Guid driverId,
                             DateOnly startDate,
                             DateOnly expectedEndDate,
                             float value,
                             string status,
                             string category)
    {
        Id = Guid.NewGuid();
        VehicleId = vehicleId;
        DriverId = driverId;
        StartDate = startDate;
        ExpectedEndDate = expectedEndDate;
        Value = value;
        Status = status;
        Category = category;
    }
}