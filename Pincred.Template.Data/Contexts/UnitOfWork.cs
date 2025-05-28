using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Pincred.Template.Domain.Attributes;
using Pincred.Template.Domain.Interfaces.Repositories;
using Pincred.Template.Domain.Interfaces.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pincred.Template.Data.Contexts;

[Repository]
public class UnitOfWork(
    AppDbContext appDbContext,
    IServiceProvider serviceProvider) : IUnitOfWork
{
    private bool _disposed;
    private IDbContextTransaction? _dbContextTransaction;
    
    #region Properties

    public ISummaryRespository Summaries => serviceProvider.GetRequiredService<ISummaryRespository>() ??
        throw new ArgumentNullException(nameof(ISummaryRespository));

    #endregion

    #region Actions

    public async Task SaveChangesAsync()
        => await appDbContext.SaveChangesAsync();

    public async Task BeginTransactionAsync()
    {
        if (_dbContextTransaction is null)
            _dbContextTransaction = await appDbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_dbContextTransaction != null)
        {
            await SaveChangesAsync();
            await _dbContextTransaction.CommitAsync();
            await _dbContextTransaction.DisposeAsync();
            _dbContextTransaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_dbContextTransaction != null)
        {
            await _dbContextTransaction.RollbackAsync();
            await _dbContextTransaction.DisposeAsync();
            _dbContextTransaction = null;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                appDbContext?.Dispose();
                _dbContextTransaction?.Dispose();
            }

            _disposed = true;
        }
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion
}