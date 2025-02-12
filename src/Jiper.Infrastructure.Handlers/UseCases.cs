using MediatR;

namespace Jiper.Infrastructure.Handlers;

public interface IUseCase : IRequest
{
}

public interface IUseCase<out TResponse>
    : IRequest<TResponse>
{
}

public interface IUseCaseHandler<in TUseCase> : IRequestHandler<TUseCase>
    where TUseCase : IUseCase
{
    Task Execute(TUseCase useCase, CancellationToken ct = default);
    Task IRequestHandler<TUseCase>.Handle(TUseCase useCase, CancellationToken ct) => Execute(useCase, ct);
}

public interface IUseCaseHandler<in TUseCase, TResult> : IRequestHandler<TUseCase, TResult>
    where TUseCase : IUseCase<TResult>
{
    Task<TResult> Execute(TUseCase useCase, CancellationToken ct = default);
    Task<TResult> IRequestHandler<TUseCase, TResult>.Handle(TUseCase useCase, CancellationToken ct) => Execute(useCase, ct);
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