namespace Pincred.Template.Domain.Interfaces.Repositories.Base;

public interface IBaseRepository<TEntity> where
    TEntity : class
{
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task<TEntity?> GetAsync(Guid id);
    Task<List<TEntity>> GetAllAsync();
}