using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace EnigmaDotnet.Models;

public sealed class Book
{
    [Key]
    public Guid BookId { get; set; }
    public string Title { get; set; }
    public string Publisher { get; set; }
    public string ISBN { get; set; }
    public int Year { get; set; }
    public byte[] Image { get; set; }
    public int Quantity { get; set; }

    // Внешний ключ для Author
    public Guid AuthorId { get; set; }
        
    // Навигационное свойство для Author
    public Author Author { get; set; }
}