using Microsoft.Extensions.DependencyInjection;
using StatWeaver.Engine.Application.Abstractions.CQRS;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace StatWeaver.Engine.Application.Common;

public class CommandDispatcher : ICommandDispatcher
{
	private IServiceProvider _serviceProvider { get; set; }

	public CommandDispatcher(IServiceProvider aServiceProvider)
	{
		_serviceProvider = aServiceProvider;
	}

	public Task<Result> Dispatch<TCommand>(TCommand aCommand, CancellationToken aCancellationToken) where TCommand : ICommand
	{
		ICommandHandler<TCommand> handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
		return handler.Handle(aCommand, aCancellationToken);
	}

	public Task<Result<TResponse>> Dispatch<TCommand, TResponse>(TCommand aCommand, CancellationToken aCancellationToken) where TCommand : ICommand<TResponse>
	{
		ICommandHandler<TCommand, TResponse> handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResponse>>();
		return handler.Handle(aCommand, aCancellationToken);
	}
}
