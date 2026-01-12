using Microsoft.EntityFrameworkCore;
using WebApi.Application.Repositories;
using WebApi.Application.Services;
using WebApi.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<StatisticService>();

bool useEf = builder.Configuration.GetValue<bool>("UseEf");

string? connectionString = builder.Configuration.GetConnectionString("Default");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Connection string 'Default' is not configured."
    );
}

if (useEf)
{
    builder.Services.AddDbContext<WebApi.Infrastructure.Persistence.AppDbContext>(opt =>
        opt.UseSqlite(connectionString));

    builder.Services.AddScoped<IStatisticRepository, EntityFrameworkStatisticRepository>();
}
else
    builder.Services.AddScoped<IStatisticRepository>(sp => { return new AdoNetStatisticRepository(connectionString); });

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();