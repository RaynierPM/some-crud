namespace someCrud.domain.models;



public class BaseModel {
    public int id {get; set;}
    public DateTime createdAt { get; set; } = DateTime.Now;
    public DateTime updatedAt { get; set; } = DateTime.Now;
}