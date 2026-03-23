using StatWeaver.Engine.Application.Common;
using StatWeaver.Engine.Application.Dtos.GameVersion;

namespace StatWeaver.Engine.Application.Abstractions.Services;

public interface IGameVersionsService
{
	Task<Result<IEnumerable<GameVersionDto>>> GetGameVersionsAsync(CancellationToken aCancellationToken);

	Task<Result<GameVersionDto>> GetGameVersionAsync(int aId, CancellationToken aCancellationToken);
}