namespace StatWeaver.Engine.Application.Abstractions.CQRS;

public interface ICommand : IBaseCommand
{

}

public interface ICommand<TResponse> : IBaseCommand
{

}
