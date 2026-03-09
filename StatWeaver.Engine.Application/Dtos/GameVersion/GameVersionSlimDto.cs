namespace StatWeaver.Engine.Application.Dtos.GameVersion;

public record GameVersionSlimDto(
	int Id,
	string VersionLabel,
	DateTime ReleasedAt,
	bool IsDefault);
