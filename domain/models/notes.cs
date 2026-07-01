namespace someCrud.domain.models;

public class Note : BaseModel {
    public required string title { get; set; }
    public required string body {get; set;}

    public override string ToString()
    {
        return "Title: " + title + ", body: " + body;
    }
}