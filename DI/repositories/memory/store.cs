using someCrud.domain.dtos;
using someCrud.domain.models;

namespace someCrud.DI.repositories.memory;

class InMemoryStore
{
    private Dictionary<int, Note> _notes = [];
    private int next_id = 1;

    private int getIdNIncrement()
    {
        return next_id++;
    }

    public Dictionary<int, Note> notes {get {return _notes;} private set{}}

    public Note addNote(Note note)
    {
        note.id = getIdNIncrement();
        if (_notes.ContainsKey(note.id)) { throw new DuplicatedEntry("Notes");}
        _notes[note.id] = note;
        return note;
    }

    public Note? getNote(int id)
    {
        if (_notes.ContainsKey(id))
        {
            return _notes[id];
        }
        return null;
    }

    public bool exists(int id)
    {
        return _notes.ContainsKey(id);
    }

    public int count(NoteFiltersDto filters)
    {
        return processFilters(filters).Count;
    }
    public List<Note> GetNotes(NoteFiltersDto filters)
    {
        int offset = getOffset(filters);
        int stopIndex = getStopIndex(filters);
        List<Note> filteredNotes = [];

        List<Note> notes = processFilters(filters);

        for (int i = offset; i < notes.Count; i++)
        {
            Note item = notes[i];
            filteredNotes.Add(item);

            if (i+1 > stopIndex) { break; }
        }
        return filteredNotes;
    }

    public bool DeleleNote(int id)
    {
        if (!exists(id))
        {
            return false;
        }

        _notes.Remove(id);
        return true;
    }

    public bool update(int id, Note note)
    {
        if (!exists(id))
        {
            return false;
        }

        _notes[id] = note;
        return true;
    }


    private int getOffset(PaginationFilters filters)
    {
        return (filters.Page-1)*filters.Size;
    }

    private int getStopIndex(PaginationFilters filters)
    {
        var offset = getOffset(filters);
        return offset + filters.Size;
    }

    private List<Note> processFilters(NoteFiltersDto filters)
    {
        List<Note> notes = [];

        if (filters.Title == null && filters.Date == null)
        {
            return _notes.Values.ToList<Note>();
        }

        foreach (var item in _notes.Values.ToList())
        {   
            int activeFilters = 0;
            int passedFilters = 0;
            if (filters.Title != null)
            {   
                activeFilters++;
                if (item.title.ToLower().Contains(filters.Title.ToLower())) passedFilters++;
            }

            if (filters.Date != null)
            {
                activeFilters++;
                if (item.createdAt == filters.Date) passedFilters++;
            }
            if (activeFilters == passedFilters) notes.Add(item);
        }
        return notes;
    }

}

class InMemoryStoreWrapper
{
    static private InMemoryStoreWrapper? _instance = null;

    public static InMemoryStoreWrapper getInstance()
    {
        if (_instance == null) _instance = new InMemoryStoreWrapper();

        return _instance;
    }

    private InMemoryStoreWrapper()
    {
        store = new InMemoryStore();
    }

    public InMemoryStore store;
}