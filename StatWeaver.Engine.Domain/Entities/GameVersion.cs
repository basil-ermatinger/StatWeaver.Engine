using System.ComponentModel.DataAnnotations;

namespace StatWeaver.Engine.Domain.Entities;

public class GameVersion
{
	public int Id { get; set; }

	public Guid Uid { get; set; }

	[MaxLength(50)]
	public required string VersionLabel { get; set; }

	public required DateTime ReleasedAt { get; set; }

	public bool IsDefault { get; set; }

	public DateTime _CreatedAt { get; set; }

	public DateTime _ModifiedAt { get; set; }

	public int GameId { get; set; }

	public Game? Game { get; set; }
}
