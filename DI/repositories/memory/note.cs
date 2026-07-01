
namespace someCrud.DI.repositories.memory;

using someCrud.DI.repositories;
using someCrud.domain.dtos;
using someCrud.domain.models;
public class InMemoryNoteRepository : INoteRepository
{
    
    public InMemoryNoteRepository()
    {
        storeWrapper = InMemoryStoreWrapper.getInstance();
    }

    private readonly InMemoryStoreWrapper storeWrapper;

    public Note create(Note note)
    {
        var newNote = storeWrapper.store.addNote(note);
        return newNote;
    }

    public bool delete(int id)
    {
        return storeWrapper.store.DeleleNote(id);
    }

    public Note? get(int id)
    {
        return storeWrapper.store.getNote(id);
    }

    public PaginationBase<Note> getAll(NoteFiltersDto filters)
    {

        var notes =  storeWrapper.store.GetNotes(filters);
        var count = storeWrapper.store.count(filters);

        return new PaginationBase<Note>
        {
            items = notes,
            total_items = count,
            page = filters.Page,
            size = filters.Size
        };
    }

    public Note? update(int id, Note note)
    {   
        note.updatedAt = DateTime.Now;
        if (storeWrapper.store.update(id, note))
        {
            return note;
        }

        throw new NotFoundEntry("Notes");
    }
}