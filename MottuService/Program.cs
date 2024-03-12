using Amazon;
using Amazon.SQS;
using dotenv.net;
using Microsoft.OpenApi.Models;
using MottuService.DataBase;
using MottuService.RentVehicleService;

var builder = WebApplication.CreateBuilder(args);

// Configuração de serviços...
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
builder.Services.AddHostedService<SqsConsumer>();
builder.Services.AddSingleton<IAmazonSQS>(_ => new AmazonSQSClient(RegionEndpoint.USEast1));
builder.Services.AddAntiforgery(options => { options.SuppressXFrameOptionsHeader = true; });

DotEnv.Load();

builder.Services.AddScoped<MottuDataBaseContext>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});


app.Use(async (context, next) =>
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<MottuDataBaseContext>();
        dbContext.Database.EnsureCreated();
    }

    await next();
});
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.AddRentVehicleRouter();
app.AddDriverRoute();
app.AddDeliveryRoute();


app.Run();
