using FluentValidation.TestHelper;
using NinaApp.Core.DTO;
using NinaApp.Core.Validators;

namespace NinaApp.xUnit.Tests.NinaApp.Core.Validators
{
  public class UserUpdationValidatorTests
  {
    private readonly UserUpdationValidator _userUpdationValidator;
    public UserUpdationValidatorTests() {
      _userUpdationValidator = new UserUpdationValidator();
    }

    [Fact]
    public void Validate_WhenAllModelPropertiesAreCorrect_ShouldNotHaveAnyError()
    {
      //Arrange
      UserUpdation validUser = new UserUpdation()
      {
        Name = "Jesus",
        Email = "validemail@gmail.com",
        Password = "Password12345"
      };

      //Act
      TestValidationResult<UserUpdation>? result =
        _userUpdationValidator.TestValidate(validUser);

      //Assert
      result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("Yu")]
    [InlineData("A")]
    public void Validate_WhenNameIsLessThan3Characters_ShouldHaveError(string invalidName)
    {
      //Arrange
      UserUpdation user = new UserUpdation()
      {
        Name = invalidName,
        Email = "validemail@gmail.com",
        Password = "validPassword123"
      };

      //Act
      TestValidationResult<UserUpdation> result =
        _userUpdationValidator.TestValidate(user);

      //Assert
      result.ShouldHaveValidationErrorFor(user => user.Name)
        .WithErrorMessage("Name must have at least 3 characters");
    }

    [Theory]
    [InlineData("AbcdefghijAbcdefghijAbcdefghijAbcdefghijAbcdefghijAbcdefghijAbcdefghijAbcdefghijk")]
    [InlineData("AbcdefghijAbcdefghijAbcdefghijAbcdefghijAbcdefghijAbcdefghijAbcdefghijAbcdefghijkl")]
    public void Validate_WhenNameExceed80Characters_ShouldHaveError(string invalidName)
    {
      //Arrange
      UserUpdation user = new UserUpdation()
      {
        Name = invalidName,
        Email = "validEmail@gmail.com",
        Password = "validPassword123"
      };

      //Act
      TestValidationResult<UserUpdation> result =
        _userUpdationValidator.TestValidate(user);

      //Assert
      result.ShouldHaveValidationErrorFor(user => user.Name)
        .WithErrorMessage("Name cannot exceed 80 characters.");
    }

    [Theory]
    [InlineData("user@gmail.com")]               // Standard
    [InlineData("user.name@gmail.com")]          // Dot in local
    [InlineData("user_name@gmail.com")]          // Underscore
    [InlineData("user-name@gmail.com")]          // Hyphen
    [InlineData("user+tag@gmail.com")]           // Plus (Sub-addressing)
    [InlineData("user@my-domain.com")]           // Hyphen in domain
    [InlineData("user@sub.domain.co.uk")]        // Subdomains + Compound TLD
    [InlineData("123user@gmail.com")]            // Numbers
    public void Validate_WhenEmailIsValid_ShouldNotHaveAnyError(string validEmail)
    {
      //Arrange
      UserUpdation validUser = new UserUpdation()
      {
        Name = "validName",
        Email = validEmail,
        Password = "validPassword"
      };

      //Act
      TestValidationResult<UserUpdation> result = 
        _userUpdationValidator.TestValidate(validUser);

      //Assert
      result.ShouldNotHaveAnyValidationErrors();
    }


    [Theory]
    // 1. Structure Failures
    [InlineData("invalidgmail.com")]         // Missing @
    [InlineData("invalid@gmailcom")]         // Missing TLD dot
    [InlineData("invalid@.com")]             // Domain starts with dot
    [InlineData("@gmail.com")]               // Missing local part
    [InlineData(".@gmail.com")]              // Local part starts with dot
    [InlineData("invalid@gmail.com.")]       // Trailing dot (regex excludes this)

    // 2. Forbidden Special Characters in Local Part (Your requested list)
    [InlineData("inv@alid@gmail.com")]       // Double @
    [InlineData("inv#alid@gmail.com")]       // #
    [InlineData("inv/alid@gmail.com")]       // /
    [InlineData("inv|alid@gmail.com")]       // |
    [InlineData("inv*alid@gmail.com")]       // *
    [InlineData("inv=alid@gmail.com")]       // =
    [InlineData("inv$alid@gmail.com")]       // $
    [InlineData("inv&alid@gmail.com")]       // &
    [InlineData("inv%alid@gmail.com")]       // %
    [InlineData("inv'alid@gmail.com")]       // '
    [InlineData("inv\"alid@gmail.com")]      // "
    [InlineData("inv:alid@gmail.com")]       // :
    [InlineData("inv,alid@gmail.com")]       // ,
    [InlineData("inv;alid@gmail.com")]       // ;
    [InlineData("inv!alid@gmail.com")]       // !
    [InlineData("inv[]alid@gmail.com")]      // []
    [InlineData("inv()alid@gmail.com")]      // ()
    [InlineData("inv\\alid@gmail.com")]      // Backslash

    // 3. Dot Issues (Consecutive or misplaced)
    [InlineData(".invalid@gmail.com")]       // Leading dot
    [InlineData("invalid.@gmail.com")]       // Trailing dot in local
    [InlineData("invalid..name@gmail.com")]  // Consecutive dots in local
    [InlineData("invalid@gmail..com")]       // Consecutive dots in domain
    [InlineData("invalid@gmail.co..m")]      // Consecutive dots in TLD part

    // 4. Domain / TLD Issues
    [InlineData("invalid@gmail.c")]          // TLD too short (regex requires {2,})
    [InlineData("invalid@gmail.123")]        // Numeric TLD (invalid for standard web emails)
    [InlineData("invalid@111.222.333.444")]  // IP Addresses (rejected by TLD requirement)
    [InlineData("invalid@-gmail.com")]       // Domain starts with hyphen
    [InlineData("invalid@gmail-.com")]       // Domain ends with hyphen
    [InlineData("invalid@gmail.co@m")]       // @ in domain
    [InlineData("invalid@gmail.co#m")]       // # in domain
    [InlineData("invalid@gmail.co!m")]       // ! in domain
    [InlineData("invalid@gmail.com@")]       // Trailing @
    [InlineData("invalid@@gmail.com")]       // Double @ separator

    // 5. Whitespace Issues
    [InlineData("invalid gmail.com")]        // Space in middle
    [InlineData(" invalid@gmail.com")]       // Leading space
    [InlineData("invalid@gmail.com ")]       // Trailing space
    public void Validate_WhenEmailIsNotValid_ShouldHaveError(string invalidEmail)
    {
      //Arrange
      UserUpdation user = new UserUpdation()
      {
        Name = "validName",
        Email = invalidEmail,
        Password = "validPassword123",
      };

      //Act
      TestValidationResult<UserUpdation> result = 
        _userUpdationValidator.TestValidate(user);

      //Assert
      result.ShouldHaveValidationErrorFor(user => user.Email)
        .WithErrorMessage("A valid email address is required.");
    }
  }
}
