using StatWeaver.Engine.Application.Common;

namespace StatWeaver.Engine.Application.Abstractions.CQRS;

public interface ICommandDispatcher
{
	Task<Result> Dispatch<TCommand>(TCommand aCommand, CancellationToken aCancellationToken) where TCommand : ICommand;

	Task<Result<TResponse>> Dispatch<TCommand, TResponse>(TCommand aCommand, CancellationToken aCancellationToken) where TCommand : ICommand<TResponse>;
}
