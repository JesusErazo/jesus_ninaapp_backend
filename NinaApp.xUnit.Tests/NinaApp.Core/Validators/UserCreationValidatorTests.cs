using FluentValidation.TestHelper;
using NinaApp.Core.DTO;
using NinaApp.Core.Validators;

namespace NinaApp.xUnit.Tests.NinaApp.Core.Validators
{
  public class UserCreationValidatorTests
  {
    //Arrange
    private readonly UserCreationValidator _userCreationValidator;

    public UserCreationValidatorTests() {
      _userCreationValidator = new UserCreationValidator();
    }

    [Fact]
    public void Validate_WhenAllModelPropertiesAreCorrect_ShouldNotHaveAnyError()
    {
      //Arrange
      UserCreation model = new UserCreation()
      {
        Name = "validName",
        Email = "valid@gmail.com",
        Password = "validPassword",
      };

      //Act
      TestValidationResult<UserCreation>? result =
        _userCreationValidator.TestValidate(model);

      //Assert
      result.ShouldNotHaveAnyValidationErrors();
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
      UserCreation model = new UserCreation()
      {
        Name = "validName",
        Email = validEmail,
        Password = "validPassword",
      };

      //Act
      TestValidationResult<UserCreation>? result =
        _userCreationValidator.TestValidate(model);

      //Assert
      result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Validate_WhenNameIsNullOrWhiteSpace_ShouldHaveError(string? invalidName)
    {
      //Arrange
      UserCreation model = new UserCreation { 
        Name = invalidName, 
        Email= "valid@gmail.com", 
        Password = "validPassword"
      };

      //Act
      TestValidationResult<UserCreation>? result = 
        _userCreationValidator.TestValidate(model);

      //Assert
      result.ShouldHaveValidationErrorFor(user => user.Name)
        .WithErrorMessage("Name is required.");
    }

    [Theory]
    [InlineData("ab")]
    [InlineData("a")]

    public void Validate_WhenNameIsLessThan3Characters_ShouldHaveError(string invalidName)
    {
      //Arrange
      UserCreation model = new UserCreation
      {
        Name = invalidName,
        Email = "valid@gmail.com",
        Password = "validPassword"
      };

      //Act
      TestValidationResult<UserCreation> result =
        _userCreationValidator.TestValidate(model);

      //Assert
      result.ShouldHaveValidationErrorFor(user => user.Name)
        .WithErrorMessage("Name must have at least 3 characters.");
    }

    [Fact]
    public void Validate_WhenNameHasMoreThan80Characters_ShouldHaveError()
    {
      //Arrange
      string nameWith81Characters = "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabc";

      UserCreation model = new UserCreation
      {
        Name = nameWith81Characters,
        Email = "valid@gmail.com",
        Password = "validPassword"
      };

      //Act
      TestValidationResult<UserCreation> result =
        _userCreationValidator.TestValidate(model);

      //Assert
      result.ShouldHaveValidationErrorFor(user => user.Name)
        .WithErrorMessage("Name cannot exceed 80 characters.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Validate_WhenEmailIsNullOrWhiteSpace_ShouldHaveError(string? invalidEmail)
    {
      //Arrange
      UserCreation model = new UserCreation
      {
        Name = "validName",
        Email = invalidEmail,
        Password = "validPassword"
      };

      //Act
      TestValidationResult<UserCreation> result =
        _userCreationValidator.TestValidate(model);

      //Assert
      result.ShouldHaveValidationErrorFor(user => user.Email)
        .WithErrorMessage("Email is required.");
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

    public void Validate_WhenEmailIsInvalidFormat_ShouldHaveError(string invalidEmail)
    {
      //Arrange
      UserCreation model = new UserCreation
      {
        Name = "validName",
        Email = invalidEmail,
        Password = "validPassword"
      };

      //Act
      TestValidationResult<UserCreation> result =
        _userCreationValidator.TestValidate(model);

      //Assert
      result.ShouldHaveValidationErrorFor(user => user.Email)
        .WithErrorMessage("A valid email address is required.");

    }

    [Theory]
    [InlineData("a@a.ia")]
    [InlineData("a@a.a")]
    public void Validate_WhenEmailIsLessThan7Characters_ShouldHaveError(string invalidEmail)
    {
      //Arrange
      UserCreation model = new UserCreation
      {
        Name = "validName",
        Email = invalidEmail,
        Password = "validPassword"
      };

      //Act
      TestValidationResult<UserCreation> result =
        _userCreationValidator.TestValidate(model);

      //Assert
      result.ShouldHaveValidationErrorFor(user => user.Email)
        .WithErrorMessage("Email must have at least 7 characters.");
    }

    [Fact]
    public void Validate_WhenEmailHasMoreThan50Characters_ShouldHaveError()
    {
      //Arrange
      string emailWith51Characters = "abcdefghijklmnopqrstuvwxyzabcdefghijklmno@gmail.com";
      UserCreation model = new UserCreation
      {
        Name = "validName",
        Email = emailWith51Characters,
        Password = "validPassword"
      };

      //Act
      TestValidationResult<UserCreation> result =
        _userCreationValidator.TestValidate(model);

      //Assert
      result.ShouldHaveValidationErrorFor(user => user.Email)
        .WithErrorMessage("Email cannot exceed 50 characters.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Validate_WhenPasswordIsNullOrWhiteSpace_ShouldHaveError(string? invalidPassword)
    {
      //Arrange
      UserCreation model = new UserCreation
      {
        Name = "validName",
        Email = "validEmail@gmail.com",
        Password = invalidPassword
      };

      //Act
      TestValidationResult<UserCreation> result =
        _userCreationValidator.TestValidate(model);

      //Assert
      result.ShouldHaveValidationErrorFor(user => user.Password)
        .WithErrorMessage("Password is required.");
    }

    [Theory]
    [InlineData("passwor")]
    [InlineData("admin")]
    [InlineData("root")]
    public void Validate_WhenPasswordIsLessThan8Characters_ShouldHaveError(string invalidPassword)
    {
      //Arrange
      UserCreation model = new UserCreation
      {
        Name = "validName",
        Email = "valid@gmail.com",
        Password = invalidPassword
      };

      //Act
      TestValidationResult<UserCreation> result =
        _userCreationValidator.TestValidate(model);

      //Assert
      result.ShouldHaveValidationErrorFor(user => user.Password)
        .WithErrorMessage("Password must have at least 8 characters.");
    }

    [Fact]
    public void Validate_WhenPasswordHasMoreThan50Characters_ShouldHaveError()
    {
      //Arrange
      string passwordWith51Characters = "abcdefghijklmnopqrstuvwxyzabcdefghijklmnoabcdefghij";
      UserCreation model = new UserCreation
      {
        Name = "validName",
        Email = "validEmail@gmail.com",
        Password = passwordWith51Characters
      };

      //Act
      TestValidationResult<UserCreation> result =
        _userCreationValidator.TestValidate(model);

      //Assert
      result.ShouldHaveValidationErrorFor(user => user.Password)
        .WithErrorMessage("Password cannot exceed 50 characters.");
    }

  }
}
