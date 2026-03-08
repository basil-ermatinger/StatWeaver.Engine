namespace StatWeaver.Engine.Application.Dtos.GameVersion;

public record GameVersionDto(
	int Id, 
	string VersionLabel, 
	DateTime ReleasedAt, 
	bool IsDefault, 
	string Game);
