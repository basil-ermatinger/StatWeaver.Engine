using Microsoft.Extensions.DependencyInjection;
using StatWeaver.Engine.Application.Abstractions.CQRS;

namespace StatWeaver.Engine.Application.Common;

public class QueryDispatcher : IQueryDispatcher
{
	private IServiceProvider _serviceProvider { get; set; }

	public QueryDispatcher(IServiceProvider aServiceProvider)
	{
		_serviceProvider = aServiceProvider;
	}

	public Task<Result<TResponse>> Dispatch<TQuery, TResponse>(TQuery aQuery, CancellationToken aCancellationToken) where TQuery : IQuery<TResponse>
	{
		IQueryHandler<TQuery, TResponse> handler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResponse>>();
		return handler.Handle(aQuery, aCancellationToken);
	}
}
