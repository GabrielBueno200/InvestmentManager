using Api.Auth.Application;
using Api.Auth.Infrastructure;
using InvestmentManager.Shared.Configurations.Extensions;
using InvestmentManager.Shared.Configurations.Filters;
using InvestmentManager.Shared.Configurations.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomResultFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.AddJwtSecurityScheme());

builder.Services
    .AddApplication()
    .AddInfrastructure();

builder.Services.AddJwtAuthentication();

builder.Services
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddProblemDetails();

builder.Services.AddTransient<AuthenticationMiddleware>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();

app.UseMiddleware<AuthenticationMiddleware>();

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();
