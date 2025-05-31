using Microsoft.EntityFrameworkCore;

namespace DotnetBatchInjection.Services;

public class BaseService<T> : IBaseService<T> where T : Entity
{
    protected readonly AtsDbContext _context;

    protected BaseService(AtsDbContext context)
    {
        _context = context;
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _context.Set<T>()
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>()
            .Where(e => !e.IsDeleted)
            .ToListAsync();
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.IsDeleted = false;

        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        var existing = await _context.Set<T>()
            .FirstOrDefaultAsync(e => e.Id == entity.Id && !e.IsDeleted);
        if (existing == null) throw new KeyNotFoundException($"{typeof(T).Name} not found");

        _context.Entry(existing).CurrentValues.SetValues(entity);
        existing.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return existing;
    }

    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Set<T>()
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        if (entity == null) return false;

        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}
