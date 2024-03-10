public class Vehicle
{
    public Guid Id { get; init; }
    public string Plate { get; private set; }
    public string Model { get; private set; }
    public int Year { get; private set; }
    public string Status { get; private set; }
    public ICollection<RentDriverVehicle> Rents { get; set; }


    public Vehicle(string plate, string model, int year)
    {
        Plate = plate;
        Id = Guid.NewGuid();
        Model = model;
        Year = year;
        Status = "available";
        Rents = new List<RentDriverVehicle>();

    }

    public void UpdateVehiclePlate(string plate){
        Plate = plate;
    }
}