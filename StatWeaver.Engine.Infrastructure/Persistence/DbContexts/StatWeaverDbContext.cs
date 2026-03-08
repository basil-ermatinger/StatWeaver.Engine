using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using StatWeaver.Engine.Domain.Entities;

namespace StatWeaver.Engine.Infrastructure.DbContexts;

public class StatWeaverDbContext : DbContext
{
	public StatWeaverDbContext(DbContextOptions<StatWeaverDbContext> options) : base(options) {	}

	public DbSet<Game> Games { get; set; }
	public DbSet<GameVersion> GameVersions { get; set; }

	protected override void OnModelCreating(ModelBuilder aModelBuilder)
	{
		base.OnModelCreating(aModelBuilder);

		foreach (IMutableEntityType entityType in aModelBuilder.Model.GetEntityTypes())
		{
			IMutableProperty? uidProperty = entityType.FindProperty("Uid");

			if (uidProperty != null && uidProperty.ClrType == typeof(Guid))
			{
				uidProperty.SetDefaultValueSql("NEWID()");
			}
		}
	}

	public override int SaveChanges()
	{
		UpdateTimestamps();
		return base.SaveChanges();
	}

	public override Task<int> SaveChangesAsync(CancellationToken aCancellationToken = default)
	{
		UpdateTimestamps();
		return base.SaveChangesAsync(aCancellationToken);
	}

	private void UpdateTimestamps()
	{
		IEnumerable<EntityEntry> entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

		foreach (EntityEntry? entry in entries)
		{
			PropertyEntry? createdAtProperty = entry.Property("_CreatedAt");
			PropertyEntry? modifiedAtProperty = entry.Property("_ModifiedAt");

			if (entry.State == EntityState.Added)
			{
				createdAtProperty?.CurrentValue = DateTime.Now;
				modifiedAtProperty?.CurrentValue = DateTime.Now;
			}
			else if (entry.State == EntityState.Modified)
			{
				modifiedAtProperty?.CurrentValue = DateTime.Now;
			}
		}
	}
}
