using Microsoft.OpenApi.Models;
using NoteTakingApi.Features.Auth;
using NoteTakingApi.Features.Notes;
using NoteTakingApi.Features.Tags;
using NoteTakingApi.Infrastructure;
using NoteTakingApi.Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddApplicationDependencies(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "NoteTaking API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Write your token in format: Bearer {your JWT}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();

Registration.Endpoint.Map(app);
Authorization.Endpoint.Map(app);
Refresh.Endpoint.Map(app);
CreateNote.Endpoint.Map(app);
GetNoteById.Endpoint.Map(app);
GetNotes.Endpoint.Map(app);
UpdateNote.Endpoint.Map(app);
DeleteNote.Endpoint.Map(app);
GetTags.Endpoint.Map(app);

app.MapHealthChecks("/health");


app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();
