using Xunit;
using FluentAssertions;
using someCrud.domain.services;
using someCrud.DI.repositories;
using FakeItEasy;
using someCrud.domain.models;
using Microsoft.EntityFrameworkCore;
using someCrud.configuration;
using someCrud.domain.dtos;
using someCrud.DI.repositories.sql;
using AutoMapper;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace someCrud.tests;

public class NoteServicesTest
{
    NoteService _noteService;
    INoteRepository _noteRepository;

    NoteServicesTest()
    {
        _noteRepository = A.Fake<INoteRepository>();
        _noteService = new NoteService(_noteRepository);
    }

    private async Task<AppDbContext> getDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var AppDbContext = new AppDbContext(options);
        AppDbContext.Database.EnsureCreated();

        return AppDbContext;
    }

    private IMapper getMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(NoteProfile));
        }, NullLoggerFactory.Instance);

        return config.CreateMapper();
    }

    [Fact]
    public async Task NoteService_Create_ShouldCreateNote()
    {   
        var noteRepository = new SqlNoteRepository(await getDbContext(), getMapper());
        var noteService = new NoteService(noteRepository);

        var noteDto = new CreateNoteDto
        {
            Body = "Some text to test",
            Title = "Testing code"
        };

        var newNote = await noteService.create(noteDto);

        newNote.Should().BeOfType<Note>();
        newNote.title.Should().Be(noteDto.Title);
    }
}