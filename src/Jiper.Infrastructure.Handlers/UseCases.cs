using MediatR;

namespace Jiper.Infrastructure.Handlers;

public interface IUseCase : IRequest
{
}

public interface IUseCase<out TResponse>
    : IRequest<TResponse>
    where TResponse : IUseCaseResponse
{
}

public interface IUseCaseResponse
{
}

public interface IUseCaseHandler<TUseCase>
    : IRequestHandler<TUseCase>
    where TUseCase : IUseCase
{
}

public interface IUseCaseHandler<TUseCase, TResponse>
    : IRequestHandler<TUseCase, TResponse>
    where TUseCase : IUseCase<TResponse>
    where TResponse : IUseCaseResponse
{
}