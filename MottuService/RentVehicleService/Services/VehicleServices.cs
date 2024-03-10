using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuService.DataBase;

public class VehicleService
{   

    public Dictionary<int, double> rentValuePerDay = new Dictionary<int, double>(){{7,30}, {15,28}, {30,22}};
    public Dictionary<int, double> rentFeePerDay =  new Dictionary<int, double>(){{7,1.20}, {15,1.40}, {30,1.60 }};
    public double CalculateRentFee(int numberDaysToRent, DateOnly expectedEndDate, DateOnly endDate){
        var numberDaysOfFee = expectedEndDate.DayNumber - endDate.DayNumber;
        return numberDaysOfFee * rentValuePerDay[numberDaysToRent] * rentFeePerDay[numberDaysToRent];
    }

    public double CalculateRentLateFee(DateOnly expectedEndDate, DateOnly endDate){
        var numberDaysOfFee = endDate.DayNumber - expectedEndDate.DayNumber;
        return numberDaysOfFee * 50;
    }

    public async Task<IGetExpectedValueChargeResponse> GetRentVehicleExcharge(Guid id, DateOnly endDate,  MottuDataBaseContext dbContext) {
        var rent = await dbContext.Rents.FirstOrDefaultAsync(vehicle => vehicle.Id == id);
        if (rent == null) {
            throw new NotFoundException();
       }
       var value = rent.Value;
       var numberDaysOfReturn = rent.ExpectedEndDate.DayNumber - endDate.DayNumber;
       if(numberDaysOfReturn>0) {value += CalculateRentFee(rent.NumberDaysToRent,rent.ExpectedEndDate, endDate);}
       if(numberDaysOfReturn<0) {value += CalculateRentLateFee(rent.ExpectedEndDate, endDate);}
       return new IGetExpectedValueChargeResponse(value); 
    }

    private async Task AddRentVehicleToDatabase(RentDriverVehicle rentVehicle, MottuDataBaseContext dbContext) {
        await dbContext.Rents.AddAsync(rentVehicle);
        await dbContext.SaveChangesAsync();
    }

    private void UpdateExpectedEndDateAndValueRentVehicle(RentDriverVehicle rentVehicle, DateOnly expectedEndDate, double expectedValueOfRent) {
        rentVehicle.UpdateRentVehicleExpectedEndDate(expectedEndDate);
        rentVehicle.UpdateRentVehicleValue(expectedValueOfRent);
    }

    public double ValueOfRentBasedOnRentType(int numberDaysToRent) {
        return numberDaysToRent * rentValuePerDay[numberDaysToRent];
    }

    public DateOnly SetRentExpectedEndDate(DateOnly startDate, int numberDaysToRent){
        return startDate.AddDays(numberDaysToRent);
    }

    public async Task RentVehicle(ICreateRentVehicleRequest request, MottuDataBaseContext dbContext) {
        var vehicleId = request.vehicleId;
        var driverId = request.driverId;
        var startDate = request.startDate;
        var numberDaysToRent = request.numberDaysToRent;

        var rentVehicle = new RentDriverVehicle(vehicleId, driverId, startDate,numberDaysToRent);
        var expectedValueOfRent = ValueOfRentBasedOnRentType(numberDaysToRent);
        var expectedEndDate = SetRentExpectedEndDate(startDate, numberDaysToRent);

        UpdateExpectedEndDateAndValueRentVehicle(rentVehicle, expectedEndDate, expectedValueOfRent);
        await AddRentVehicleToDatabase(rentVehicle, dbContext);
    }

    public async Task UpdatePlateVehicle(Guid id, string plate, MottuDataBaseContext dbContext) {
       var vehicle = await dbContext.Vehicles.SingleOrDefaultAsync(vehicle => vehicle.Id == id);
       if (vehicle == null) {
        throw new NotFoundException();
       }
       vehicle.UpdateVehiclePlate(plate);
       await dbContext.SaveChangesAsync();

    }

    public async Task<List<Vehicle>> GetVehicle(string plate, MottuDataBaseContext dbContext) {
        var vehicles = await dbContext.Vehicles.Where(vehicle => vehicle.Plate == plate).ToListAsync();
        if(vehicles.Count == 0) {
            throw new NotFoundException();
        }
        return vehicles;
    }

    public async Task<bool> IsPlateRegistered(string plate, MottuDataBaseContext dbContext)
    {
        return await dbContext.Vehicles.AnyAsync(vehicle => vehicle.Plate == plate);
    }
    public async Task CreateVehicle(ICreateVehicleRequest request, MottuDataBaseContext dbContext)
    {
        var plateAlreadyExists = await IsPlateRegistered(request.plate, dbContext);
        if (plateAlreadyExists)
        {
            throw new ConflictException("Can not register vehicle, this plate it is already registered");
        }

        var vehicle = new Vehicle(request.plate, request.model, request.year);
        await dbContext.Vehicles.AddAsync(vehicle);
        await dbContext.SaveChangesAsync();
    }
}
