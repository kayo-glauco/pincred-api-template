namespace Pincred.Template.Domain.Interfaces.Repositories.Base;

public interface IUnitOfWork : IDisposable
{
    ISummaryRespository Summaries { get; }

    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task SaveChangesAsync();
}