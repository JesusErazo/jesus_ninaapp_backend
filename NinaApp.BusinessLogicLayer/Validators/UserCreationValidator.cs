using FluentValidation;
using NinaApp.Core.DTO;

namespace NinaApp.Core.Validators
{
  public class UserCreationValidator : AbstractValidator<UserCreation>
  {
    public UserCreationValidator()
    {
      RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Name is required.")
        .MinimumLength(3).WithMessage("Name must have at least 3 characters.")
        .MaximumLength(80).WithMessage("Name cannot exceed 80 characters.");

      // Break down of the Regex:
      // ^                         -> Start of string
      // [a-zA-Z0-9_+-]+           -> Local part start: Allows letters, numbers, underscore, plus, hyphen.
      // (\.[a-zA-Z0-9_+-]+)* -> Followed by: Optional groups starting with a dot (prevents consecutive dots "..").
      // @                         -> The separator.
      // [a-zA-Z0-9]               -> Domain start: Must be alphanumeric (no hyphen or dot at start).
      // (?:[a-zA-Z0-9-]*[a-zA-Z0-9])? -> Domain middle: Allows hyphens but ensures it doesn't end with a hyphen.
      // (?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?)* -> Subdomains: Handles .co.uk or .sub.domain formats correctly.
      // \.[a-zA-Z]{2,}            -> TLD: Must start with a dot and be at least 2 letters (e.g., .com, .io).
      // $                         -> End of string
      RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email is required.")
        .Matches(@"^[a-zA-Z0-9_+-]+(\.[a-zA-Z0-9_+-]+)*@[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?)*\.[a-zA-Z]{2,}$")
          .WithMessage("A valid email address is required.")
        .MinimumLength(7).WithMessage("Email must have at least 7 characters.")
        .MaximumLength(50).WithMessage("Email cannot exceed 50 characters.");

      RuleFor(x => x.Password)
        .NotEmpty().WithMessage("Password is required.")
        .MinimumLength(8).WithMessage("Password must have at least 8 characters.")
        .MaximumLength(50).WithMessage("Password cannot exceed 50 characters.");
    }
  }
}
