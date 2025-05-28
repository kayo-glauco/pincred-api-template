using Microsoft.EntityFrameworkCore;
using Pincred.Template.Data.Contexts;
using Pincred.Template.Domain.Entities.Base;
using Pincred.Template.Domain.Interfaces.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pincred.Template.Data.Repositories;

public abstract class BaseRepository<TEntity> :
    IBaseRepository<TEntity> where TEntity : EntityBase
{
    protected readonly AppDbContext _context;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entity)
    {
        await _context.Set<TEntity>().AddRangeAsync(entity);
    }

    public Task UpdateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity?> GetAsync(Guid id)
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
    }
}