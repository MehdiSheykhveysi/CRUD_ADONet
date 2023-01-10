using App.infrastructure.Shared;
using App.Domain.Contracts;
using System.Data.SqlClient;
using _04.App.infrastructure.SqlRepositories;
using App.ApplicationServices.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUnitOfWork, SqlUnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped(typeof(SqlConnection), sp =>
{
    return new SqlConnection(builder.Configuration.GetConnectionString("dbConnectionString"));
});

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

app.Run();
