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
        
        Console.WriteLine("Stop index: {0}", stopIndex);
        Console.WriteLine("Items: {0}", notes.Count);

        for (int i = offset; i < notes.Count; i++)
        {
            Note item = notes[i];
            filteredNotes.Add(item);

            Console.WriteLine(item);
            Console.WriteLine("Index: ", i);
            if (i+1 > stopIndex) { break; }
        }
        Console.WriteLine("Notes: {0}, length: {1}", filteredNotes, filteredNotes.Count);
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


    private int getOffset(IPaginationFilters filters)
    {
        return (filters.Page-1)*filters.Size;
    }

    private int getStopIndex(IPaginationFilters filters)
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
            if (filters.Title != null && item.title.Contains(filters.Title))
            {
                notes.Append(item);
            }

            if (filters.Date != null && item.createdAt == filters.Date)
            {
                notes.Append(item);
            }
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