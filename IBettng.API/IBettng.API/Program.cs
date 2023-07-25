using IBetting.DataAccess;
using IBetting.Services.BackgroundServices;
using IBetting.Services.BettingService;
using IBetting.Services.DeserializeService;
using IBetting.Services.MatchService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<IBettingDbContext>(options =>
                 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IBettingService, BettingService>();
builder.Services.AddScoped<IXmlService, XmlService>();

builder.Services.AddHostedService<DataUploaderBackgroudService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
