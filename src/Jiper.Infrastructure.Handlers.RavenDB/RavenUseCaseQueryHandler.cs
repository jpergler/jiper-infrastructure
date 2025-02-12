using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Jiper.Infrastructure.Handlers.RavenDB;

public abstract class RavenUseCaseQueryHandler<TUseCase, TResult>(IDocumentStore store)
    : IUseCaseHandler<TUseCase, TResult>, IDisposable
    where TUseCase : IUseCase<TResult>
{
    protected IAsyncDocumentSession Session { get; private set; } = null!;
    
    public async Task<TResult> Handle(TUseCase request, CancellationToken cancellationToken)
    {
        Session = store.OpenAsyncSession();
        
        return await ExecuteQuery(request, Session, cancellationToken);
    }

    protected abstract Task<TResult> ExecuteQuery(TUseCase useCase, IAsyncDocumentSession session, CancellationToken ct = default);

    public void Dispose()
    {
        Session.Dispose();
    }
}
