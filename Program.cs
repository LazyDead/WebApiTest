using WebApi.Application.Repositories;
using WebApi.Application.Services;
using WebApi.Infrastructure.Repositories;
using WebApi.Infrastructure.SQLIte;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddTransient<StatisticService>();
builder.Services.AddTransient<IStatisticRepository,SqLiteStatisticRepository>(sp=>{
    var cfg = sp.GetRequiredService<IConfiguration>();
    var connectOptions = cfg.GetSection("DataBase")["ConnectionString"];
    return new SqLiteStatisticRepository(connectOptions);
});
var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();