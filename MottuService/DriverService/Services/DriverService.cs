using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MottuService.DataBase;

public class DriverService
{

    public async Task<bool> IsDriverRegistered(string cnpj, MottuDataBaseContext dbContext)
    {
        return await dbContext.Drivers.AnyAsync(driver => driver.Cnpj == cnpj);
    }
    public async Task<IResult> CreateDriver(ICreateDriverRequest request, MottuDataBaseContext dbContext)
    {
        var driverAlreadyExists = await IsDriverRegistered(request.cnpj, dbContext);
        Console.WriteLine(driverAlreadyExists);
        
        if (driverAlreadyExists)
        {
            return Results.Conflict("Can not register driver, this CNPJ it is already registered");
        }

        var vehicle = new Driver(request.cnpj,
                                 request.name,
                                 request.cnhType,
                                 request.cnhNumber,
                                 "s3_path",
                                 request.birthdate);
        await dbContext.Drivers.AddAsync(vehicle);
        await dbContext.SaveChangesAsync();
        return Results.Ok("Driver registered");
    }
}
