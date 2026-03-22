using StatWeaver.Engine.Application.Abstractions.CQRS;

using StatWeaver.Engine.Domain.Entities;

namespace StatWeaver.Engine.Application.Features.GameVersions.Commands;

public sealed record CreateGameVersionCommand : ICommand
{
	public GameVersion? GameVersion { get; set; }

	public CreateGameVersionCommand(GameVersion? aGameVersion)
	{
		GameVersion = aGameVersion;
	}
}
