using Microsoft.EntityFrameworkCore;
using StatWeaver.Engine.Application.Abstractions.CQRS;
using StatWeaver.Engine.Application.Common;
using StatWeaver.Engine.Application.Dtos.GameVersion;

using StatWeaver.Engine.Infrastructure.DbContexts;

namespace StatWeaver.Engine.Application.Features.GameVersions.Queries;

public class GetGameVersionsQueryHandler : IQueryHandler<GetGameVersionsQuery, IEnumerable<GameVersionDto>>
{
	private readonly IStatWeaverDbContext _context;

	public GetGameVersionsQueryHandler(IStatWeaverDbContext aContext)
	{
		_context = aContext;
	}

	public async Task<Result<IEnumerable<GameVersionDto>>> Handle(GetGameVersionsQuery aQuery, CancellationToken aCancellationToken)
	{
		IEnumerable<GameVersionDto> gameVersions = await _context.GameVersions
			.Select(gv => new GameVersionDto(gv.Id, gv.VersionLabel, gv.ReleasedAt, gv.IsDefault, gv.GameId))
			.ToListAsync(cancellationToken: aCancellationToken);

		return Result.Success(gameVersions);
	}
}
