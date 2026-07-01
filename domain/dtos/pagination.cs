namespace someCrud.domain.dtos;

public class PaginationBase<T>
{
    public required IEnumerable<T> items {get; set;}
    public int page {get; set;}
    public int size {get; set;}

    public int total_items {get; set;}
}

public abstract class IPaginationFilters
{
    public int Page {get { return Math.Max(1, field); } set;}
    public int Size {get { return Math.Min(Math.Max(10, field), 30); } set;}
}

public interface IDateFilters
{
    DateTime? Date {get; set; }
}