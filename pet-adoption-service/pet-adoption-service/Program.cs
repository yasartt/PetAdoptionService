using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using pet_adoption_service.Models;
using pet_adoption_service.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ShelterService>();
builder.Services.AddScoped<PetAdopterService>();
builder.Services.AddScoped<PetService>();
builder.Services.AddScoped<VeterinarianService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Access the Configuration object through the HostingEnvironment
var configuration = builder.Configuration;

// Register DbContext here
builder.Services.AddDbContext<PetAdoptionDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
