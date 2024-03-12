public record ICreateVehicleRequest(string plate, string model, int year);
public record IUpdateVehicleRequest(string plate);
public record ICreateRentVehicleRequest(Guid vehicleId, Guid driverId, int numberDaysToRent);
public record IGetExpectedValueChargeRequest(Guid rentId, DateOnly endDate);
public record IGetExpectedValueChargeResponse(double value);
