using Microsoft.EntityFrameworkCore;
using StatWeaver.Engine.Application.Abstractions.CQRS;
using StatWeaver.Engine.Application.Common;

using StatWeaver.Engine.Infrastructure.DbContexts;
using System.Diagnostics.Contracts;

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
		if (aCommand.GameVersion is null)
		{
			return Result.Failure(new Error("NullValue", "GameVersion is null"));
		}

		bool gameVersionExisting = await _context.GameVersions.AnyAsync(g => g.VersionLabel.ToLower().Trim() == aCommand.GameVersion.VersionLabel.ToLower().Trim());

		if (gameVersionExisting)
		{
			return Result.Failure(new Error("Conflict", $"Version with label {aCommand.GameVersion.VersionLabel} already exists"));
		}

		try
		{
			_context.GameVersions.Add(aCommand.GameVersion);

			await _context.SaveChangesAsync();

			return Result.Success();
		}
		catch (Exception aException)
		{
			return Result.Failure();
		}
	}
}
