using NoteTakingApi.Features.Auth;
using NoteTakingApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddApplicationDependencies(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
builder.Services.AddEndpointsApiExplorer(); // нужно для Minimal API
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();

Register.Endpoint.Map(app);

app.Run();
