using System.ComponentModel.DataAnnotations;

namespace StatWeaver.Engine.Application.Dtos.GameVersion;

public class CreateGameVersionDto
{
	[MaxLength(50)]
	public required string VersionLabel { get; set; }

	public required DateTime ReleasedAt { get; set; }

	public bool IsDefault { get; set; }

	public required int GameId { get; set; }
}