using StatWeaver.Engine.Application.Common;
using StatWeaver.Engine.Application.Dtos.GameVersion;

namespace StatWeaver.Engine.Application.Abstractions.Services;

public interface IGameVersionsService
{
	Task<IEnumerable<GameVersionDto>> GetGameVersionsAsync();

	Task<Result<GameVersionDto>> GetGameVersionAsync(int aId, CancellationToken aCancellationToken);
}