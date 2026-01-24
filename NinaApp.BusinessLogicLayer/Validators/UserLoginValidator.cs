using FluentValidation;
using NinaApp.Core.DTO;

namespace NinaApp.Core.Validators
{
  public class UserLoginValidator: AbstractValidator<UserLogin>
  {
    public UserLoginValidator() {
      RuleFor(u => u.Email)
        .NotEmpty().WithMessage("Email is required.")
        .Matches(@"^[a-zA-Z0-9_+-]+(\.[a-zA-Z0-9_+-]+)*@[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?)*\.[a-zA-Z]{2,}$")
          .WithMessage("A valid email address is required.");

      RuleFor(u => u.Password)
        .NotEmpty().WithMessage("Password is required.");
    }
  }
}
