using StatWeaver.Engine.Application.Dtos.GameVersion;

namespace StatWeaver.Engine.Application.Abstractions.Services;

public interface IGameVersionsService
{
	Task<IEnumerable<GameVersionDto>> GetGameVersionsAsync();
}