public record ICreateVehicleRequest(string plate, string model, int year);
public record IGetVehiclRequest(string plate);