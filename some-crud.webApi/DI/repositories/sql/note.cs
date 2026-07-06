using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using someCrud.configuration;
using someCrud.DI.models;
using someCrud.domain.dtos;
using someCrud.domain.models;
using someCrud.helpers;

namespace someCrud.DI.repositories.sql;

public class SqlNoteRepository(AppDbContext context, IMapper mapper) : INoteRepository
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Note> create(Note note)
    {
        NoteEntity entity = _mapper.Map<NoteEntity>(note);
        _context.Add(entity);
        await _context.SaveChangesAsync();

        return _mapper.Map<Note>(entity); 
    }

    public async Task<bool> delete(int id)
    {
        NoteEntity? note = await _context.Notes.FindAsync(id);
        if (note == null)
        {
            return false;
        }

        _context.Remove(note);
        var result = await _context.SaveChangesAsync();

        return result != 0;
    }

    public async Task<Note?> get(int id)
    {
        NoteEntity? entity = await _context.Notes.FindAsync(id);

        if  (entity == null) return null;
         
        Note? note = _mapper.Map<Note?>(entity);
        return note;
    }

    public async Task<PaginationBase<Note>> getAll(NoteFiltersDto filters)
    {
        var query = _context.Notes;
        if (filters.Date != null)
        {
            query.Where(q => q.createdAt == filters.Date);
        }

        if (filters.Title != null)
        {
            query.Where(q => q.title.ToLower().Contains(filters.Title.ToLower()));
        }
        
        query.Take(filters.Size);
        query.Skip(PaginationHelper.GetOffset(filters));
        query.OrderBy(q => q.createdAt);

        List<NoteEntity> result = await query.ToListAsync();
        List<Note> notes = _mapper.Map<List<Note>>(result);
        return new PaginationBase<Note>
        {
            items = notes,
            page = filters.Page,
            size = filters.Size
        };
    }

    public async Task<Note?> update(int id, Note model)
    {
        NoteEntity? note = await _context.Notes.FindAsync(id);
        if (note == null)
        {
            return null;
        }

        note.body = model.body;
        note.title = model.title;

        _context.Update(note);
        var result = await _context.SaveChangesAsync();
        
        if (result == 0)
        {
            return null;
        } else
        {
            return _mapper.Map<Note>(note);
        }

    }
}