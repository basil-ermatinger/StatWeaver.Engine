using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
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
		ApplyChangeTracking();
		return base.SaveChanges();
	}

	public override Task<int> SaveChangesAsync(CancellationToken aCancellationToken = default)
	{
		ApplyChangeTracking();
		return base.SaveChangesAsync(aCancellationToken);
	}

	private void ApplyChangeTracking()
	{
		IEnumerable<EntityEntry> entries = ChangeTracker.Entries()
			.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
			.ToList();

		foreach (EntityEntry entry in entries)
		{
			DateTime now = DateTime.Now;
			
			if (entry.State == EntityState.Added)
			{
				entry.Property("_CreatedAt")?.CurrentValue = now;
				entry.Property("_ModifiedAt")?.CurrentValue = now;
			} 
			else
			{
				entry.Property("Uid").IsModified = false;
				entry.Property("_CreatedAt").IsModified = false;
				entry.Property("_ModifiedAt")?.CurrentValue = now;
			}
		}
	}
}
