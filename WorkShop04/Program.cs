using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkShop04.Data;
using WorkShop04.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApiDbContext>(option =>
{
    option.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CarManagerJWT;Trusted_Connection=True;MultipleActiveResultSets=true").UseLazyLoadingProxies();
});

builder.Services.AddIdentity<AppUser, IdentityRole>(option =>
{
    option.Password.RequiredLength = 8;
    option.Password.RequireNonAlphanumeric = false;
})
  .AddEntityFrameworkStores<ApiDbContext>()
  .AddDefaultTokenProviders();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
