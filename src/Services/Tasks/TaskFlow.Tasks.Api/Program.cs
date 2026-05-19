using TaskFlow.Tasks.Application;
using TaskFlow.Tasks.Infrastructure;
using TaskFlow.Tasks.Api.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHealthChecks()
    .AddSqlServer(
        connectionString: builder.Configuration.GetConnectionString("TasksDatabase")!,
        name: "tasks-database");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}
app.UseGlobalExceptionHandling();

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.MapControllers();


app.MapHealthChecks("/health");

app.MapHealthChecks("/health/ready");

app.Run();

public partial class Program;