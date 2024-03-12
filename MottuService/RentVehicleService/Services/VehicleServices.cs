using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuService.DataBase;
using ThirdParty.BouncyCastle.Asn1;

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
       rent.UpdateRentVehicleEndDate(endDate);
       await dbContext.SaveChangesAsync();

       return new IGetExpectedValueChargeResponse(value); 
    }

    private async Task AddRentVehicleToDatabase(RentDriverVehicle rentVehicle, MottuDataBaseContext dbContext) {
        await dbContext.Rents.AddAsync(rentVehicle);
        await dbContext.SaveChangesAsync();
    }


    private void UpdateExpectedEndDateAndValueRentVehicle(RentDriverVehicle rentVehicle, DateOnly expectedEndDate, double expectedValueOfRent) {
        rentVehicle.UpdateRentVehicleEndDate(expectedEndDate);
        rentVehicle.UpdateRentVehicleValue(expectedValueOfRent);
    }

    public double ValueOfRentBasedOnRentType(int numberDaysToRent) {
        return numberDaysToRent * rentValuePerDay[numberDaysToRent];
    }

    public DateOnly SetRentExpectedEndDate(DateOnly startDate, int numberDaysToRent){
        return startDate.AddDays(numberDaysToRent);
    }

    public async Task UpdateVehicleStatus(Guid id, string status, MottuDataBaseContext dbContext) {
       var vehicle = await dbContext.Vehicles.SingleOrDefaultAsync(vehicle => vehicle.Id == id);
       if (vehicle == null) {
        throw new NotFoundException();
       }
       vehicle.UpdateVehicleStatus(status);
       await dbContext.SaveChangesAsync();

    }

    public async Task isVehicleAvailable(Guid vehicleId,  MottuDataBaseContext dbContext) {
        var existingVehicle = await dbContext.Vehicles.FirstOrDefaultAsync(vehicle => vehicle.Id == vehicleId && vehicle.Status == "available");
        if (existingVehicle == null) {
            throw new NotFoundException($"Vehicle with id {vehicleId} not available.");
        }
    }

    public async Task driverValidationToRent(Guid driverId,  MottuDataBaseContext dbContext){
        await isDriverExists(driverId, dbContext);
        await isDriverQualifiedToRent(driverId, dbContext);



    }
    public async Task isDriverQualifiedToRent(Guid driverId,  MottuDataBaseContext dbContext) {
        var existingDriver = await dbContext.Drivers.FirstOrDefaultAsync(driver => driver.CnhType == "A");
        if (existingDriver == null) {
            throw new NotFoundException($"Driver with id {driverId} cnh type must be A.");
        }
    }
    public async Task isDriverExists(Guid driverId,  MottuDataBaseContext dbContext) {
        var existingDriver = await dbContext.Drivers.FirstOrDefaultAsync(driver => driver.Id == driverId);
        if (existingDriver == null) {
            throw new NotFoundException($"Driver with id {driverId} not exist.");
        }
    }

    public async Task RentVehicle(ICreateRentVehicleRequest request, MottuDataBaseContext dbContext) {
        var vehicleId = request.vehicleId;
        var driverId = request.driverId;
        var numberDaysToRent = request.numberDaysToRent;

        await isVehicleAvailable(vehicleId, dbContext);
        await driverValidationToRent(driverId, dbContext);
        


        var rentVehicle = new RentDriverVehicle(vehicleId, driverId, numberDaysToRent);
        var expectedValueOfRent = ValueOfRentBasedOnRentType(numberDaysToRent);
        var expectedEndDate = SetRentExpectedEndDate(rentVehicle.StartDate, numberDaysToRent);


        UpdateExpectedEndDateAndValueRentVehicle(rentVehicle, expectedEndDate, expectedValueOfRent);
        await AddRentVehicleToDatabase(rentVehicle, dbContext);

        await UpdateVehicleStatus(vehicleId, "unavaible", dbContext);
    }

    public async Task DeleteVehicle(Guid id, MottuDataBaseContext dbContext) {
       var vehicle = await dbContext.Vehicles.SingleOrDefaultAsync(vehicle => vehicle.Id == id);
       if (vehicle == null) {
        throw new NotFoundException();
       }
       dbContext.Vehicles.Remove(vehicle);
       await dbContext.SaveChangesAsync();

    }

    public async Task UpdatePlateVehicle(Guid id, string plate, MottuDataBaseContext dbContext) {
       var vehicle = await dbContext.Vehicles.SingleOrDefaultAsync(vehicle => vehicle.Id == id);
       if (vehicle == null) {
        throw new NotFoundException();
       }
       vehicle.UpdateVehiclePlate(plate);
       await dbContext.SaveChangesAsync();

    }

    public async Task<Vehicle>  GetVehicle(string plate, MottuDataBaseContext dbContext) {

        var vehicle = await dbContext.Vehicles.FirstOrDefaultAsync(vehicle => vehicle.Plate == plate);
        if (vehicle == null) {
            throw new NotFoundException();
       }
        return vehicle;
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
