using StatWeaver.Engine.Application.Common;

namespace StatWeaver.Engine.Application.Abstractions.CQRS;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
	Task<Result> Handle(TCommand aCommand, CancellationToken aCancellationToken);
}

public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse>
{
	Task<Result<TResponse>> Handle(TCommand aCommand, CancellationToken aCancellationToken);
}