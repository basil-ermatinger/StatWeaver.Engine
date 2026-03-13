using StatWeaver.Engine.Application.Abstractions.CQRS;
using StatWeaver.Engine.Application.Common;
using StatWeaver.Engine.Infrastructure.DbContexts;

namespace StatWeaver.Engine.Application.Features.GameVersions.Commands;

public sealed class CreateGameVersionCommandHandler : ICommandHandler<CreateGameVersionCommand>
{
	private readonly IStatWeaverDbContext _context;

	public CreateGameVersionCommandHandler(IStatWeaverDbContext aContext)
	{
		_context = aContext;
	}

	public async Task<Result> Handle(CreateGameVersionCommand aCommand, CancellationToken aCancellationToken)
	{
		if (aCommand.GameVersion == null)
		{
			return Result.Failure(Error.NullValue);
		}

		_context.GameVersions.Add(aCommand.GameVersion);

		await _context.SaveChangesAsync();

		return Result.Success();
	}
}
