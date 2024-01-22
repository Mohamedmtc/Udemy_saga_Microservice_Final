using Core;
using Core.RabbitMq.BusConfiguration;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Orchestrator.Presistance;
using Orchestrator.StateMachine.Order;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<OrchSagaDbContext>((provider, dbContextBuilder) =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    dbContextBuilder.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.MigrationsAssembly(typeof(OrchSagaDbContext).Assembly.FullName);
        sqlOptions.MigrationsHistoryTable($"__{nameof(OrchSagaDbContext)}");
    });
});

builder.Services.AddHostedService<RecreateDatabaseHostedService<OrchSagaDbContext>>();

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddSagaStateMachine<OrderStateMachine, OrderStateData>()
                .EntityFrameworkRepository(r =>
                {
                    r.ConcurrencyMode = ConcurrencyMode.Pessimistic; // or use Optimistic, which requires RowVersion
                    r.ExistingDbContext<OrchSagaDbContext>();

                });

    cfg.AddBus(provider => RabbitMqBus.ConfigureBusWebApi(provider, builder.Configuration));
});

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
