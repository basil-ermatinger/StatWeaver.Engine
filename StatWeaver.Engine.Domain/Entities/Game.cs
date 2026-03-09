using System.ComponentModel.DataAnnotations;

namespace StatWeaver.Engine.Domain.Entities;

public class Game
{
	public int Id { get; set; }

	public Guid Uid { get; set; }

	public required string Name { get; set; }

	public bool IsActive { get; set; }

	public DateTime _CreatedAt { get; set; }

	public DateTime _ModifiedAt { get; set; }

	public ICollection<GameVersion>? GameVersions { get; set; } 
}
