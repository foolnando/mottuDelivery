using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.EntityFrameworkCore;
using MottuService.DataBase;

public class DriverService
{

    public BasicAWSCredentials credentials = new BasicAWSCredentials("AKIA2UC3E3CMH5XWUUU2", "oGQ91BUzuoTHD3Z4FsHhKY+HeKqqlvUm/6CL4rZU");
    public AmazonS3Client s3Client;
    public DriverService()
    {
        s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast1);
    }

    public async Task<List<Driver>> GetDriver(string cnpj, MottuDataBaseContext dbContext) {
            var drivers = await dbContext.Drivers.Where(vehicle => vehicle.Cnpj == cnpj).ToListAsync();
            if(drivers.Count == 0) {
                throw new NotFoundException();
            }
            return drivers;
        }
    public async Task<bool> IsDriverRegistered(string cnpj, MottuDataBaseContext dbContext)
    {
        return await dbContext.Drivers.AnyAsync(driver => driver.Cnpj == cnpj);
    }
    public async Task<IResult> CreateDriver(ICreateDriverRequest request, MottuDataBaseContext dbContext)
    {
        var driverAlreadyExists = await IsDriverRegistered(request.cnpj, dbContext);
        
        if (driverAlreadyExists)
        {
            return Results.Conflict("Can not register driver, this CNPJ it is already registered");
        }
        Console.WriteLine(request);
        var driver = new Driver(request.cnpj,
                                 request.name,
                                 request.cnhType,
                                 request.cnhNumber,
                                 request.birthdate);
        await dbContext.Drivers.AddAsync(driver);
        await dbContext.SaveChangesAsync();
        return Results.Ok("Driver registered");
    }

    public async Task UploadPictureToS3bucket(IFormFile cnhImage, Guid driverId){
        var keyFileName = "cnh-image-" + driverId.ToString();
            var s3Request=  new PutObjectRequest(){
                BucketName = "mottu-drivers-license-pictures",
                Key=keyFileName,
                InputStream = cnhImage.OpenReadStream(),
            };
            await s3Client.PutObjectAsync(s3Request);
    }

    public async Task UpdateDriverCnhPicture(IFormFile cnhImage, Guid driverId, MottuDataBaseContext dbContext){
        var driver = await dbContext.Drivers.SingleOrDefaultAsync(driver => driver.Id == driverId);
        if (driver == null) {
            throw new NotFoundException();
        }
        await UploadPictureToS3bucket(cnhImage, driverId);
        var s3PathCnhImage = "s3://mottu-drivers-license-pictures/cnhImage"+driverId.ToString();
        driver.UpdateDriverCnhS3Path(s3PathCnhImage);
        await dbContext.SaveChangesAsync();
    }

    
}
