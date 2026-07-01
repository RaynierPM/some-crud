namespace someCrud.DI.repositories;

using someCrud.domain.dtos;
using someCrud.domain.models;


public interface IBaseRepository<T, F>
{
    public T? get(int id);
    public abstract PaginationBase<T> getAll(F filters);
    public T create(T model);
    public T? update(int id, T model);
    public bool delete(int id);
}

public interface INoteRepository : IBaseRepository<Note, NoteFiltersDto> {}