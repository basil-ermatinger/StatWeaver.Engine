using StatWeaver.Engine.Application.Common;

namespace StatWeaver.Engine.Application.Abstractions.CQRS;

public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
{
	Task<Result<TResponse>> Handle(TQuery aQuery, CancellationToken aCancellationToken);
}
