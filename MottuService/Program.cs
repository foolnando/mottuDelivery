using MottuService.DataBase;
using MottuService.RentVehicleService;

var builder = WebApplication.CreateBuilder(args);

// Configuração de serviços...
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<MottuDataBaseContext>();
// builder.Services.AddScoped<IVehicleService, VehicleService>();

var app = builder.Build();

// Configuração do pipeline de solicitação...
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.AddRentVehicleRouter();
app.AddDriverRoute();

app.Run();
