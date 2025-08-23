using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalAPI.Domain.Entities;

public class Admin
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } = default!;

    [Required]
    [StringLength(255)]
    public string Email { get; set; } = default!;

    [Required]
    [StringLength(50)]
    public string Password { get; set; } = default!;
   
    [Required]
    [StringLength(10)]
    public string Perfil { get; set; } = default!;
}
