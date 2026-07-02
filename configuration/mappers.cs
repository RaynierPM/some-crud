using AutoMapper;
using someCrud.DI.models;
using someCrud.domain.models;

namespace someCrud.configuration;

public class NoteProfile : Profile
{   
    public NoteProfile()
    {
        CreateMap<Note, NoteEntity>();
        CreateMap<NoteEntity, Note>();
    }
}