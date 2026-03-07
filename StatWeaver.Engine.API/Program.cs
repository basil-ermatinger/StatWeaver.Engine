using StatWeaver.Engine.Infrastructure.ServiceRegistration;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Services of the container
builder.Services.AddInfrastructure(builder.Configuration);

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
