using Xunit;
using FluentAssertions;
using someCrud.domain.services;
using someCrud.DI.repositories;
using FakeItEasy;

namespace someCrud.tests;

class NoteServicesTest
{
    NoteService _noteService;
    INoteRepository _noteRepository;

    NoteServicesTest()
    {
        _noteRepository = A.Fake<INoteRepository>();
        _noteService = new NoteService(_noteRepository);
    }
}