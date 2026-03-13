using StatWeaver.Engine.Application.Common;

namespace StatWeaver.Engine.Application.Abstractions.CQRS;

public interface IQueryDispatcher
{
	Task<Result<TResponse>> Dispatch<TQuery, TResponse>(TQuery query, CancellationToken ct) where TQuery : IQuery<TResponse>;
}
