using FluentValidation;
using NinaApp.Core.DTO;

namespace NinaApp.Core.Validators
{
  public class UserUpdationValidator: AbstractValidator<UserUpdation>
  {
    public UserUpdationValidator() {
      RuleFor(x => x.Name)
        .MinimumLength(3).WithMessage("Name must have at least 3 characters")
        .MaximumLength(80).WithMessage("Name cannot exceed 80 characters.");

      RuleFor(x => x.Email)
        .EmailAddress().WithMessage("A valid email address is required.")
        .MinimumLength(7).WithMessage("Email must have at least 7 characters.")
        .MaximumLength(50).WithMessage("Email cannot exceed 50 characters.");

      RuleFor(x => x.Password)
        .MinimumLength(8).WithMessage("Password must have at least 8 characters.")
        .MaximumLength(50).WithMessage("Password cannod exceed 50 characters.");
    }
  }
}
