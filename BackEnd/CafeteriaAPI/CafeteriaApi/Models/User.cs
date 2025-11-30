using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaApi.Models;

[Index(nameof(Email), IsUnique = true)]
public class User{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserID { get; set; }

    [Required] [StringLength(30)] public string Email { get; set; } = null!;
    [Required] [StringLength(20)] public string Name { get; set; } = null!;
    [Required] [StringLength(60)] public string Password { get; set; } = null!;
    
}