namespace someCrud.domain.dtos;

class CommonErrorResponse
{
    public required string message {get; set;}

    public object? metadata {get; set;}
}


class CommonResponse
{
    public string? message {get; set;}
    public required object response {get; set;}

    public object? metadata {get; set;}
}