using NoteTakingApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddInfrastructureDependencies(builder.Configuration);


var app = builder.Build();


app.UseHttpsRedirection();

app.Run();