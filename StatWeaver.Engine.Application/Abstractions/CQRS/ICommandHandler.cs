using StatWeaver.Engine.Application.Common;

namespace StatWeaver.Engine.Application.Abstractions.CQRS;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
	Task<Result> Handle(TCommand aCommand, CancellationToken aCancellationToken);
}
