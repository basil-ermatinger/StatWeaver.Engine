using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using StatWeaver.Engine.Infrastructure.DbContexts;

namespace StatWeaver.Engine.Testing.TestInfrastructure;

[TestClass]
public abstract class SqlServerIntegrationTestBase
{
	protected StatWeaverDbContext Context { get; set; } = null!;
	protected string DatabaseName { get; private set; } = null!;
	protected string ConnectionString { get; private set; } = null!;

	private readonly IConfiguration _configuration;

	protected SqlServerIntegrationTestBase() : this(GetConfiguration())
	{
	}

	public SqlServerIntegrationTestBase(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	protected static IConfiguration GetConfiguration()
	{
		return new ConfigurationBuilder()
			.AddJsonFile("appsettings.json", optional: false)
			.Build();
	}

	[TestInitialize]
	public async Task TestInitialize()
	{
		DatabaseName = $"StatWeaverTest_{Guid.NewGuid():N}";

		string? baseConnectionString = _configuration.GetConnectionString("StatWeaverDbConnectionString");

		if (string.IsNullOrEmpty(baseConnectionString))
		{
			throw new InvalidOperationException("The connection string ‘StatWeaverDbConnectionString’ was not found in the configuration.");
		}

		SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(baseConnectionString)
		{
			InitialCatalog = DatabaseName
		};

		ConnectionString = builder.ConnectionString;

		DbContextOptions<StatWeaverDbContext> options = new DbContextOptionsBuilder<StatWeaverDbContext>()
			.UseSqlServer(ConnectionString)
			.Options;

		Context = new StatWeaverDbContext(options);

		await Context.Database.EnsureDeletedAsync();

		// Für Tests: Erstelle DB basierend auf aktuellem Model (ohne Migrationen)
		await Context.Database.EnsureCreatedAsync();

		await SeedAsync();

		await InitializeHandlerAsync();
	}

	[TestCleanup]
	public async Task TestCleanup()
	{
		try
		{
			if(Context != null)
			{
				await Context.Database.EnsureDeletedAsync();
				await Context.DisposeAsync();
			}
		}
		catch
		{
			// TODO BasilErmatinger 20260314: optional loggen
		}
	}

	protected virtual Task SeedAsync()
	{
		return Task.CompletedTask;
	}

	protected virtual Task InitializeHandlerAsync()
	{
		return Task.CompletedTask;
	}
}
