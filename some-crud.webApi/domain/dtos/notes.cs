namespace someCrud.domain.dtos;

public class CreateNoteDto
{
    public required string Title { get; set; }
    public required string Body {get; set; }
}

public class UpdateNoteDto : CreateNoteDto {}

public class NoteFiltersDto : PaginationFilters, IDateFilters
{
    public DateTime? Date { get; set; }

    public string? Title {get; set;}

    public override string ToString() => $"Page: {Page}, Size: {Size}, Date: {Date}, Title: {Title}";
}