namespace StatWeaver.Engine.Application.Dtos.GameVersion;

public record GameVersionsDto(
	int Id, 
	string VersionLabel, 
	DateTime ReleasedAt, 
	bool IsDefault, 
	int GameId);
