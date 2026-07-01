using someCrud.domain.models;
using someCrud.DI.repositories;
using someCrud.domain.dtos;
using System.Reflection.Metadata.Ecma335;

namespace someCrud.domain.services;

public class NoteService {

    public NoteService(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    private INoteRepository _noteRepository;

    public Note create(CreateNoteDto note) { 
        Note newNote = new Note
        {
            body = note.Body,
            title = note.Title
        };
        return _noteRepository.create(newNote);
    }
    public Note? getOne(int id) { return _noteRepository.get(id);}
    public PaginationBase<Note> getAll(NoteFiltersDto filters)
    {
        return _noteRepository.getAll(filters);
    }
    public bool update(int id, Note note) { return _noteRepository.update(id, note) != null; }
    public bool delete(int id) { return _noteRepository.delete(id); }
}