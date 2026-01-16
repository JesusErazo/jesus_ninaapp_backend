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
      TestValidationResult<UserUpdation>? result  = 
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
  }
}
