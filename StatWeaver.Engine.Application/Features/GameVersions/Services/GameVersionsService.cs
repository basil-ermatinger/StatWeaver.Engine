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

	public async Task<IEnumerable<GameVersionDto>> GetGameVersionsAsync()
	{
		GetGameVersionQuery query = new GetGameVersionQuery();

		Result<IEnumerable<GameVersionDto>> result = await _queryDispatcher.Dispatch<GetGameVersionQuery, IEnumerable<GameVersionDto>>(query, CancellationToken.None);

		if(!result.IsSuccess)
		{
			throw new InvalidOperationException("Failed to retrieve game versions");
		}

		return result.Value;
	}
}
