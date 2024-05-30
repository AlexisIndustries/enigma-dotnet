using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnigmaDotnet.Models;

public sealed class Reservation
{
    [Key]
    public Guid ReservationId { get; set; }
        
    [Required]
    public DateTime ReservationDate { get; set; }
        
    public ReservationStatus Status { get; set; }
    
    public DateTime ReturnDate { get; set; }
        
    // Внешний ключ для Book
    [ForeignKey("Book")]
    public Guid BookId { get; set; }
    public Book Book { get; set; }
        
    // Внешний ключ для Identity User
    public string UserId { get; set; }
        
    // Навигационное свойство для Identity User
    public User User { get; set; }
}