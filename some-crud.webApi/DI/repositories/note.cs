namespace someCrud.DI.repositories;

using someCrud.domain.dtos;
using someCrud.domain.models;

public interface INoteRepository : IBaseRepository<Note, NoteFiltersDto> {}