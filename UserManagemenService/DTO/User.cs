using System.ComponentModel.DataAnnotations;
using FluentValidation;
using UserManagemenService.Models;
namespace UserManagemenService.DTO;

public class CreateUserDto
{
    [Required]
    public required string Name { get; set; }
    
    [Required]    
    public DateOnly Birthdate { get; set; }
    
    public bool? Active { get; set; }

    public User GetUser() {
        return new User{
            Name = Name, 
            Birthdate = Birthdate, 
            Active = Active ?? true
        };
    }
}

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(user => user.Name).NotNull().NotEmpty();
        RuleFor(user => user.Birthdate).NotNull().NotEmpty();
    }
}


public class UpdateUserDto
{
    [Required]
    public bool? Active { get; set; }
}

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(user => user.Active).NotNull();
    }
}
