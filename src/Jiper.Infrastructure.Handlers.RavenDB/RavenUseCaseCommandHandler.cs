using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Jiper.Infrastructure.Handlers.RavenDB;

public abstract class RavenUseCaseCommandHandler<TUseCase>(IDocumentStore documentStore)
    : IUseCaseHandler<TUseCase>, IDisposable
    where TUseCase : IUseCase
{
    protected IAsyncDocumentSession Session { get; private set; } = null!;

    public async Task Execute(TUseCase useCase, CancellationToken cancellationToken)
    {
        Session = documentStore.OpenAsyncSession();

        await ExecuteCommand(useCase, cancellationToken);

        await Session.SaveChangesAsync(cancellationToken);
    }

    protected abstract Task ExecuteCommand(TUseCase useCase, CancellationToken ct = default);

    public void Dispose()
    {
        Session?.Dispose();
    }
}

public abstract class RavenUseCaseCommandHandler<TUseCase, TResult>(IDocumentStore documentStore)
    : IUseCaseHandler<TUseCase, TResult>, IDisposable
    where TUseCase : IUseCase<TResult>
{
    protected IAsyncDocumentSession Session { get; private set; } = null!;

    public async Task<TResult> Execute(TUseCase useCase, CancellationToken cancellationToken)
    {
        Session = documentStore.OpenAsyncSession();

        var result = await ExecuteCommand(useCase, cancellationToken);

        await Session.SaveChangesAsync(cancellationToken);

        return result;
    }

    protected abstract Task<TResult> ExecuteCommand(TUseCase useCase, CancellationToken ct = default);

    public void Dispose()
    {
        Session?.Dispose();
    }
}