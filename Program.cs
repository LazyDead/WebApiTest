using WebApi.Application.Repository;
using WebApi.Application.Services;
using WebApi.Infrastructure.Repository;
using WebApi.Infrastructure.SQLIte;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddTransient<StatisticService>();
builder.Services.AddTransient<IStatisticRepository,SQLiteStatisticRepository>();
builder.Services.AddScoped<SQLiteService>(sp =>
{
    var cfg = sp.GetRequiredService<IConfiguration>();
    var connectOptions = cfg.GetSection("DataBase")["ConnectionString"];
    return new SQLiteService(connectOptions);
});
var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();