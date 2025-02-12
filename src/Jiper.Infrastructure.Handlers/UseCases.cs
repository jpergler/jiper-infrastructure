using MediatR;

namespace Jiper.Infrastructure.Handlers;

public interface IUseCase : IRequest
{
}

public interface IUseCase<out TResponse>
    : IRequest<TResponse>
{
}

public interface IUseCaseHandler<in TUseCase>
    : IRequestHandler<TUseCase>
    where TUseCase : IUseCase
{
}

public interface IUseCaseHandler<in TUseCase, TResponse>
    : IRequestHandler<TUseCase, TResponse>
    where TUseCase : IUseCase<TResponse>
{
}

public interface IUseCaseExecutor
{
    Task Execute(IUseCase useCase, CancellationToken ct = default);
    Task<TResult> Execute<TResult>(IUseCase<TResult> useCase, CancellationToken ct = default);
}

public class UseCaseExecutor(IMediator mediator) : IUseCaseExecutor
{
    public async Task Execute(IUseCase useCase, CancellationToken ct = default)
    {
        await mediator.Send(useCase, ct);
    }

    public async Task<TResult> Execute<TResult>(IUseCase<TResult> useCase, CancellationToken ct = default)
    {
        return await mediator.Send(useCase, ct);
    }
}