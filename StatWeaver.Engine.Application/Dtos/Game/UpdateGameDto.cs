using System.ComponentModel.DataAnnotations;

namespace StatWeaver.Engine.Application.Dtos.Game;

public class UpdateGameDto
{
	public required int Id { get; set; }

	[MaxLength(50)]
	public required string Name { get; set; }

	public bool IsActive { get; set; }
}