using StatWeaver.Engine.Application.Abstractions.CQRS;
using StatWeaver.Engine.Application.Dtos.GameVersion;

namespace StatWeaver.Engine.Application.Features.GameVersions.Queries;

public record GetGameVersionQuery : IQuery<IEnumerable<GameVersionDto>>
{
}
