using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserApi.Models;

public class Users
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserID { get; set; }
    
    [Required]
        public string Name { get; set; } = "";
    [Required]
        public string Email { get; set; } = "";
    [Required]
        public string Passwords { get; set; } = "";
}