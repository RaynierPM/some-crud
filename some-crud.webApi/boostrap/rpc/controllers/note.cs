using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using someCrud.domain.dtos;
using someCrud.domain.models;
using someCrud.domain.services;

namespace someCrud.bootstrap.rpc.controllers;

[ApiController]
[Route("/")]
public class NoteController (NoteService noteService, ILogger<NoteController> logger) : ControllerBase
{
    private readonly NoteService _noteService = noteService;

    private readonly ILogger<NoteController> _logger = logger;

    [HttpGet("{id}")]
    public async Task<IActionResult> getNote(int id)
    {
        _logger.LogInformation("Route [Get-One note], retrieving id: {id}", id);
        Note? note = await _noteService.getOne(id);
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
    public async Task<IActionResult> GetNotes([FromQuery] NoteFiltersDto filters)
    {
        _logger.LogInformation("Route [Get-All], retrieving filters: {filters}", filters);
        var response = await _noteService.getAll(filters);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNote([FromBody] CreateNoteDto body)
    {
        var newNote = await _noteService.create(body);
        return Ok(new CommonResponse{response = newNote});
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNote(int id, [FromBody] UpdateNoteDto body)
    {
        var note = await _noteService.getOne(id);
        if (note == null)
        {
            return NotFound(new CommonErrorResponse
            {
                message= "The entry requested doesn't exists."
            });
        } 

        if (!await _noteService.update(id, body))
        {
            return BadRequest(new CommonErrorResponse
            {
                message = "The note couldn't be updated"
            });
        } 

        return Ok(note);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> UpdateNote(int id)
    {
        var note = await _noteService.getOne(id);
        if (note == null)
        {
            return NotFound(new CommonErrorResponse
            {
                message= "The entry requested doesn't exists."
            });
        } 

        if (!await _noteService.delete(id))
        {
            return BadRequest(new CommonErrorResponse
            {
                message = "The note couldn't be deleted"
            });
        } 

        return Ok(note);
    }
}