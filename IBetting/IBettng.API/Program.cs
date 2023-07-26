using IBetting.DataAccess;
using IBetting.DataAccess.Repositories;
using IBetting.Services.BackgroundServices;
using IBetting.Services.DataConsumeService;
using IBetting.Services.DataSavingService;
using IBetting.Services.DeserializeService;
using IBetting.Services.MappingService;
using IBetting.Services.MatchService;
using IBettng.API.MappingProfiles;
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
builder.Services.AddScoped<IDataConsumeService, DataConsumeService>();
builder.Services.AddScoped<IDataSavingService, DataSavingService>();
builder.Services.AddScoped<IMappingService, MappingService>();
builder.Services.AddScoped<IXmlService, XmlService>();
builder.Services.AddScoped<ISportRepository, SportRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IBetRepository, BetRepository>();
builder.Services.AddScoped<IOddRepository, OddRepository>();

builder.Services.AddHostedService<DataUploaderBackgroudService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

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
