using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Jiper.Infrastructure.Handlers.RavenDB;

public abstract class RavenUseCaseQueryHandler<TUseCase, TResult>(IDocumentStore store)
    : IUseCaseHandler<TUseCase, TResult>, IDisposable
    where TUseCase : IUseCase<TResult>
{
    protected IAsyncDocumentSession Session { get; private set; } = null!;

    public async Task<TResult> Execute(TUseCase useCase, CancellationToken cancellationToken)
    {
        Session = store.OpenAsyncSession();

        return await ExecuteQuery(useCase, cancellationToken);
    }

    protected abstract Task<TResult> ExecuteQuery(TUseCase useCase, CancellationToken ct = default);

    public void Dispose()
    {
        Session.Dispose();
    }
}