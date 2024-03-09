public class Driver
{
    public Guid Id { get; init; }
    public string Cnpj { get; private set; }
    public string Name { get; private set; }
    public string CnhNumber { get; private set; }
    public string CnhType { get; private set; }
    public string CnhS3Path { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public ICollection<RentDriverVehicle> Rents { get; set; }
    public ICollection<OrderNotification> Notifications { get; set; }



    public Driver(string cnpj, string name, string cnhType, string cnhNumber, string cnhS3Path, DateOnly birthDate)
    {
        Cnpj = cnpj;
        Id = Guid.NewGuid();
        Name = name;
        BirthDate = birthDate;
        CnhS3Path = cnhS3Path;
        CnhType = cnhType;
        CnhNumber = cnhNumber;
        Rents = new List<RentDriverVehicle>();
        Notifications = new List<OrderNotification>();
    }
}