using Core.RabbitMq.BusConfiguration;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region MassTransit
builder.Services.AddMassTransit(cfg =>
{
    cfg.AddBus(provider => RabbitMqBus.ConfigureBusWebApi(provider, builder.Configuration));
});
builder.Services.AddHostedService<MassTransitHostedService>();

#endregion



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
