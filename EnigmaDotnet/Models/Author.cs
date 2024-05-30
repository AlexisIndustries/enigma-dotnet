using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace EnigmaDotnet.Models;

public sealed class Author
{
    [Key]
    public Guid AuthorId { get; set; }
    public string Name { get; set; }
    public string Biography { get; set; }
        
    // Список книг, написанных автором
    public ICollection<Book> Books { get; set; }
}