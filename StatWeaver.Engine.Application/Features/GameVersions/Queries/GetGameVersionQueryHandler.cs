using Microsoft.EntityFrameworkCore;

using StatWeaver.Engine.Application.Abstractions.CQRS;
using StatWeaver.Engine.Application.Common;
using StatWeaver.Engine.Application.Dtos.GameVersion;

using StatWeaver.Engine.Infrastructure.DbContexts;

namespace StatWeaver.Engine.Application.Features.GameVersions.Queries;

public class GetGameVersionQueryHandler : IQueryHandler<GetGameVersionQuery, GameVersionDto>
{
	private readonly IStatWeaverDbContext _context;

	public GetGameVersionQueryHandler(IStatWeaverDbContext aContext)
	{
		_context = aContext;
	}

	public async Task<Result<GameVersionDto>> Handle(GetGameVersionQuery aQuery, CancellationToken aCancellationToken)
	{
		GameVersionDto? gameVersion = await _context.GameVersions
			.Where(gv => gv.Id == aQuery.Id)
			.Select(gv => new GameVersionDto(gv.Id, gv.VersionLabel, gv.ReleasedAt, gv.IsDefault, gv.GameId))
			.FirstOrDefaultAsync(cancellationToken: aCancellationToken);

		if (gameVersion is null)
		{
			return Result.Failure<GameVersionDto>(Error.NotFound($"Game version with ID '{aQuery.Id}' was not found."));
		}

		return Result<GameVersionDto>.Success(gameVersion);
	}
}
