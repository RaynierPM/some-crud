namespace someCrud.DI.repositories;

using someCrud.domain.dtos;
using someCrud.domain.models;


public interface IBaseRepository<T, F>
{
    public Task<T?> get(int id);
    public abstract Task<PaginationBase<T>> getAll(F filters);
    public Task<T> create(T model);
    public Task<T?> update(int id, T model);
    public Task<bool> delete(int id);
}
