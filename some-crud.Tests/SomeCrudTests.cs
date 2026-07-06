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
using Microsoft.Extensions.Logging.Abstractions;
using FakeItEasy.Sdk;

namespace someCrud.tests;

public class NoteServicesTest
{
    NoteService _noteService;
    INoteRepository _noteRepository;

    public NoteServicesTest()
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

    private async Task<NoteService> getNoteService()
    {
        var noteRepository = new SqlNoteRepository(await getDbContext(), getMapper());
        return new NoteService(noteRepository);
    }

    [Fact]
    public async Task NoteService_Create_ShouldCreateNote()
    {   
        var noteService = await getNoteService();

        var noteDto = new CreateNoteDto
        {
            Body = "Some text to test",
            Title = "Testing code"
        };

        var newNote = await noteService.create(noteDto);

        newNote.Should().BeOfType<Note>();
        newNote.title.Should().Be(noteDto.Title);
    }

    [Fact]
    public async Task NoteService_GetOne_Success()
    {
        var noteService = await getNoteService();
        
        var noteDto = new CreateNoteDto
        {
            Body = "Some text to test",
            Title = "Testing code"
        };

        var newNote = await noteService.create(noteDto);

        var theNote = await noteService.getOne(newNote.id);

        theNote.Should().BeOfType<Note>();
        theNote.title.Should().Be(newNote.title);
    }

    [Fact]
    public async Task NoteService_GetOne_Failure()
    {
        var noteService = await getNoteService();
        var inexistentId = 10;
        
        var noteDto = new CreateNoteDto
        {
            Body = "Some text to test",
            Title = "Testing code"
        };

        var newNote = await noteService.create(noteDto);
        var theNote = await noteService.getOne(inexistentId);

        theNote.Should().Be(null);
    }

    [Fact]
    public async Task NoteService_Update_Success()
    {
        var newTitle = "The title was updated";
        var newBody = "The body was updated";
        var noteService = await getNoteService();
        var noteDto = new CreateNoteDto
        {
            Body = "Some text to test",
            Title = "Let's update"
        };

        var createdNote = await noteService.create(noteDto);

        var updateNoteDto = new UpdateNoteDto
        {
            Body = newBody,
            Title = newTitle,
        };

        var result = await noteService.update(createdNote.id, updateNoteDto);

        var updatedNote = await noteService.getOne(createdNote.id);

        result.Should().Be(true);
        updatedNote.Should().BeOfType<Note>();
        updatedNote.title.Should().Be(newTitle);
        updatedNote.body.Should().Be(newBody);
    }

    [Fact]
    public async Task NoteService_Update_FailedWithWrongID()
    {
        var newTitle = "The title was updated";
        var newBody = "The body was updated";
        var noteService = await getNoteService();
        var noteDto = new CreateNoteDto
        {
            Body = "Some text to test",
            Title = "Let's update"
        };

        var createdNote = await noteService.create(noteDto);

        var updateNoteDto = new UpdateNoteDto
        {
            Body = newBody,
            Title = newTitle,
        };

        var result = await noteService.update(1000, updateNoteDto);
        
        var updatedNote = await noteService.getOne(createdNote.id);

        result.Should().Be(false);
        updatedNote.Should().BeOfType<Note>();
        updatedNote.title.Should().NotBe(newTitle);
        updatedNote.body.Should().NotBe(newBody);
    }

    [Fact]
    public async Task NoteService_Delete_Success()
    {
        // Given
        var noteService = await getNoteService();
        var noteDto = new CreateNoteDto
        {
            Body = "Some text to test",
            Title = "Testing code"
        };
        

        // When
        var newNote = await noteService.create(noteDto);
        var result = await noteService.delete(newNote.id);
        var createdNote = await noteService.getOne(newNote.id);

        // Then
        result.Should().Be(true);
        createdNote.Should().Be(null);
    }

    [Fact]
    public async Task NoteService_Delete_FailWhenWrongId()
    {
        // Given
        var noteService = await getNoteService();
        var noteDto = new CreateNoteDto
        {
            Body = "Some text to test",
            Title = "Testing code"
        };
        

        // When
        var newNote = await noteService.create(noteDto);
        var result = await noteService.delete(10);
        var createdNote = await noteService.getOne(newNote.id);

        // Then
        result.Should().Be(false);
        createdNote.Should().BeOfType<Note>();
        createdNote.title.Should().Be(newNote.title);
    }

    [Fact]
    public async Task NoteService_GetAll_SuccessEmpty()
    {
        var noteService = await getNoteService();
        var noteFilters = new NoteFiltersDto
        {
            Page = 1,
            Size = 10,
        };

        var notes = await noteService.getAll(noteFilters);

        notes.items.Should().HaveCount(0);
        notes.size.Should().Be(10);
        notes.page.Should().Be(1);
    }

    [Fact]
    public async Task NoteService_GetAll_EmptyWithWrongParameters()
    {
        var noteService = await getNoteService();
        var noteFilters = new NoteFiltersDto
        {
            Page = -1,
            Size = -20,
        };

        var notes = await noteService.getAll(noteFilters);

        notes.items.Should().HaveCount(0);
        notes.size.Should().Be(10);
        notes.page.Should().Be(1);
    }

    [Fact]
    public async Task NoteService_GetAll_ShouldFindByTitle()
    {
        var noteService = await getNoteService();
        var noteFilters = new NoteFiltersDto
        {
            Page = 1,
            Size = 10,
            Title = "Some random text to test"
        };

        var createNoteDto = new CreateNoteDto
        {
            Title = noteFilters.Title,
            Body = "Some Body to test this feature."
        }; 

        await noteService.create(createNoteDto);

        var notes = await noteService.getAll(noteFilters);

        notes.items.Should().HaveCount(1);
        notes.size.Should().Be(10);
        notes.page.Should().Be(1);
    }
}