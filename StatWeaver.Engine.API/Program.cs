using StatWeaver.Engine.Application.Abstractions.CQRS;
using StatWeaver.Engine.Application.Abstractions.Services;
using StatWeaver.Engine.Application.Common;
using StatWeaver.Engine.Application.Dtos.GameVersion;
using StatWeaver.Engine.Application.Features.GameVersions.Commands;
using StatWeaver.Engine.Application.Features.GameVersions.Queries;
using StatWeaver.Engine.Application.Features.GameVersions.Services;

using StatWeaver.Engine.Infrastructure.DbContexts;
using StatWeaver.Engine.Infrastructure.ServiceRegistration;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Services of the container
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<IStatWeaverDbContext, StatWeaverDbContext>();
builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();
builder.Services.AddScoped<ICommandHandler<CreateGameVersionCommand>, CreateGameVersionCommandHandler>();
builder.Services.AddScoped<IGameVersionsService, GameVersionsService>();
builder.Services.AddScoped<IQueryHandler<GetGameVersionsQuery, IEnumerable<GameVersionDto>>, GetGameVersionsQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetGameVersionQuery, GameVersionDto>, GetGameVersionQueryHandler>(); 

builder.Services
	.AddControllers()
	.AddJsonOptions(opt => { opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles; });

builder.Services.AddOpenApi();

WebApplication app = builder.Build();

// HTTP request pipeline
if(app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
