using NoteTakingApi.Common.Extensions;
using NoteTakingApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddInfrastructureDependencies(builder.Configuration);
builder.Services.AddLogger(builder.Configuration);


var app = builder.Build();


app.UseHttpsRedirection();


app.Run();