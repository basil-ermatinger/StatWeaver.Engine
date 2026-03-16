using Microsoft.EntityFrameworkCore;

using StatWeaver.Engine.Application.Common;
using StatWeaver.Engine.Application.Dtos.GameVersion;
using StatWeaver.Engine.Domain.Entities;
using StatWeaver.Engine.Infrastructure.DbContexts;

namespace StatWeaver.Engine.Application.Features.GameVersions.Queries;

public class GetGameVersionsQueryHandler
{
	private readonly IStatWeaverDbContext _context;

	public GetGameVersionsQueryHandler(IStatWeaverDbContext aContext)
	{
		_context = aContext;
	}

	public async Task<Result<IEnumerable<GameVersionDto>>> Handle(GetGameVersionQuery aQuery, CancellationToken aCancellationToken)
	{
		IEnumerable<GameVersionDto> gameVersions = await _context.GameVersions
			.Select(gv => new GameVersionDto(gv.Id, gv.VersionLabel, gv.ReleasedAt, gv.IsDefault, gv.GameId))
			.ToListAsync();

		return Result.Success(gameVersions);
	}
}
