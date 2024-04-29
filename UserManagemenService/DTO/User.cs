using FluentValidation;
using UserManagemenService.Models;
namespace UserManagemenService.DTO;

public class CreateUserDto
{
    public required string Name { get; set; }    
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
    public required string Name { get; set; }    
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