public class RentDriverVehicle
{
    public Guid Id { get; init; }
    public Guid VehicleId { get; private set; }
    public Vehicle Vehicle { get; set; }
    public Guid DriverId { get; private set; }
    public Driver Driver { get; set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public double Value { get; private set; }
    public string Status { get; private set; }
    public DateOnly ExpectedEndDate { get; private set; }
    public int NumberDaysToRent { get; private set; }
    public ICollection<Order> Orders { get; set; }





    public RentDriverVehicle(Guid vehicleId, 
                             Guid driverId,
                             DateOnly startDate,
                             int numberDaysToRent)
    {
        Id = Guid.NewGuid();
        VehicleId = vehicleId;
        DriverId = driverId;
        StartDate = startDate;
        Status = "available";
        NumberDaysToRent = numberDaysToRent;
        Orders = new List<Order>();
    }

    public void UpdateRentVehicleExpectedEndDate(DateOnly expectedEndDate){
        ExpectedEndDate = expectedEndDate;
    }

    public void UpdateRentVehicleValue(double value){
        Value = value;
    }

    public void UpdateRentVehicleEndDate(DateOnly endDate){
        EndDate = endDate;
    }
}