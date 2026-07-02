using System.ComponentModel.DataAnnotations;

namespace someCrud.DI.models;

public class BaseModel {
    [Key]
    public int id {get; set;}
    public DateTime createdAt { get; set; } = DateTime.Now;
    public DateTime updatedAt { get; set; } = DateTime.Now;
}