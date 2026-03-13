using StatWeaver.Engine.Domain.Entities;

namespace StatWeaver.Engine.Application.Abstractions.Persistence;

public interface IGameVersionRepository
{
	Task<GameVersion> GetByIdAsync(Guid id);
}
