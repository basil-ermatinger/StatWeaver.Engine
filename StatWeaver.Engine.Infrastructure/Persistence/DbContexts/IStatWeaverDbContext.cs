using Microsoft.EntityFrameworkCore;
using StatWeaver.Engine.Domain.Entities;

namespace StatWeaver.Engine.Infrastructure.DbContexts;

public interface IStatWeaverDbContext
{
	DbSet<Game> Games { get; set; }

	DbSet<GameVersion> GameVersions { get; set; }

	int SaveChanges();
	
	Task<int> SaveChangesAsync(CancellationToken aCancellationToken = default);
}