using Microsoft.EntityFrameworkCore;

using StatWeaver.Engine.Domain.Entities;

namespace StatWeaver.Engine.Infrastructure.DbContexts;

public class StatWeaverDbContext : DbContext
{
	public StatWeaverDbContext(DbContextOptions<StatWeaverDbContext> options) : base(options) {	}

	public DbSet<Game> Games { get; set; }
	public DbSet<GameVersion> GameVersions { get; set; }
}
