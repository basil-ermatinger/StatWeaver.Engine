using StatWeaver.Engine.Application.Abstractions.CQRS;
using StatWeaver.Engine.Application.Abstractions.Services;
using StatWeaver.Engine.Application.Common;
using StatWeaver.Engine.Application.Dtos.GameVersion;
using StatWeaver.Engine.Application.Features.GameVersions.Queries;

namespace StatWeaver.Engine.Application.Features.GameVersions.Services;

public class GameVersionsService : IGameVersionsService
{
	IQueryDispatcher _queryDispatcher;

	public GameVersionsService(IQueryDispatcher aQueryDispatcher)
	{
		_queryDispatcher = aQueryDispatcher;
	}

	public async Task<Result<IEnumerable<GameVersionDto>>> GetGameVersionsAsync(CancellationToken aCancellationToken)
	{
		GetGameVersionsQuery query = new();

		Result<IEnumerable<GameVersionDto>> result = await _queryDispatcher.Dispatch<GetGameVersionsQuery, IEnumerable<GameVersionDto>>(query, aCancellationToken);

		return result;
	}

	public async Task<Result<GameVersionDto>> GetGameVersionAsync(int aId, CancellationToken aCancellationToken)
	{
		GetGameVersionQuery query = new() { Id = aId };

		Result<GameVersionDto> result = await _queryDispatcher.Dispatch<GetGameVersionQuery, GameVersionDto>(query, aCancellationToken);
		
		return result;
	}
}
