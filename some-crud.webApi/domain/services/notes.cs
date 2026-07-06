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

    public async Task<Note> create(CreateNoteDto note) { 
        Note newNote = new Note
        {
            body = note.Body,
            title = note.Title
        };
        return await _noteRepository.create(newNote);
    }
    public async Task<Note?> getOne(int id) { return await _noteRepository.get(id);}
    public async Task<PaginationBase<Note>> getAll(NoteFiltersDto filters)
    {
        return await _noteRepository.getAll(filters);
    }
    public async Task<bool> update(int id, CreateNoteDto note) { return await _noteRepository.update(id, new Note{id=id, body=note.Body, title=note.Title}) != null; }
    public async Task<bool> delete(int id) { return await _noteRepository.delete(id); }
}