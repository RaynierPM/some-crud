using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using someCrud.domain.dtos;
using someCrud.domain.models;
using someCrud.domain.services;

namespace someCrud.bootstrap.rpc.controllers;

[ApiController]
[Route("/")]
public class NoteController (NoteService noteService) : ControllerBase
{
    private NoteService _noteService = noteService;

    [HttpGet("{id}")]
    public IActionResult getNote(int id)
    {
        Note? note = _noteService.getOne(id);
        if (note == null)
        {
            return NotFound(new CommonErrorResponse
            {
                message = "Note not found"
            });
        }
        
        return Ok(new CommonResponse
        {
            response = note
        });
    }

    [HttpGet]
    public IActionResult GetNotes([FromQuery] NoteFiltersDto filters)
    {
        Console.WriteLine(filters);
        var response = _noteService.getAll(filters);
        return Ok(response);
    }

    [HttpPost]
    public IActionResult CreateNote([FromBody] CreateNoteDto body)
    {
        var newNote = _noteService.create(body);
        return Ok(new CommonResponse{response = newNote});
    }

    [HttpPut("{id}")]
    public IActionResult UpdateNote(int id, [FromBody] CreateNoteDto body)
    {
        var note = _noteService.getOne(id);
        if (note == null)
        {
            return NotFound(new CommonErrorResponse
            {
                message= "The entry requested doesn't exists."
            });
        } 

        note.body = body.Body;
        note.title = body.Title;

        if (!_noteService.update(id, note))
        {
            return BadRequest(new CommonErrorResponse
            {
                message = "The note couldn't be updated"
            });
        } 

        return Ok(note);
    }

    [HttpDelete("{id}")]
    public IActionResult UpdateNote(int id)
    {
        var note = _noteService.getOne(id);
        if (note == null)
        {
            return NotFound(new CommonErrorResponse
            {
                message= "The entry requested doesn't exists."
            });
        } 

        if (!_noteService.delete(id))
        {
            return BadRequest(new CommonErrorResponse
            {
                message = "The note couldn't be deleted"
            });
        } 

        return Ok(note);
    }
}