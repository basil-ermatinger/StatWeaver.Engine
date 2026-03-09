using StatWeaver.Engine.Application.Dtos.GameVersion;

namespace StatWeaver.Engine.Application.Dtos.Game;

public record GameDto(
	int Id,
	string Name,
	bool IsActive,
	List<GameVersionSlimDto>? GameVersions);
