using System.ComponentModel.DataAnnotations;

namespace StatWeaver.Engine.Application.Dtos.Game;

public class CreateGameDto
{
	[MaxLength(50)]
	public required string Name { get; set; }

	public bool IsActive { get; set; }
}