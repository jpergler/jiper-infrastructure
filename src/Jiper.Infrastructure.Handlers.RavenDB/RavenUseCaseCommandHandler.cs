using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Jiper.Infrastructure.Handlers.RavenDB;

public abstract class RavenUseCaseCommandHandler<TUseCase>(IDocumentStore documentStore)
    : IUseCaseHandler<TUseCase>, IDisposable
    where TUseCase : IUseCase
{
    protected IAsyncDocumentSession Session { get; private set; } = null!;

    public async Task Handle(TUseCase request, CancellationToken cancellationToken)
    {
        Session = documentStore.OpenAsyncSession();

        await ExecuteCommand(request, Session, cancellationToken);

        await Session.SaveChangesAsync(cancellationToken);
    }

    protected abstract Task ExecuteCommand(TUseCase useCase, IAsyncDocumentSession session, CancellationToken ct = default);

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

    public async Task<TResult> Handle(TUseCase request, CancellationToken cancellationToken)
    {
        Session = documentStore.OpenAsyncSession();

        var result = await ExecuteCommand(request, Session, cancellationToken);

        await Session.SaveChangesAsync(cancellationToken);

        return result;
    }

    protected abstract Task<TResult> ExecuteCommand(TUseCase useCase, IAsyncDocumentSession session, CancellationToken ct = default);

    public void Dispose()
    {
        Session?.Dispose();
    }
}