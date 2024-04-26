
using System.ComponentModel.DataAnnotations;

namespace UserManagemenService.Models;

public class User
{
    public long Id { get; set; }
    
    [Required]
    public required string Name { get; set; }

    // [Required]
    // public required string Birthdate { get; set; }

    // [Required]
    // public bool Active { get; set; }

    public void CopyValues(User u) {
        this.Name = u.Name;
    }
}
