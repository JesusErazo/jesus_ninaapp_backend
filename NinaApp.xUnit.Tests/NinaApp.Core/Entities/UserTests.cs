using FluentAssertions;
using NinaApp.Core.Entities;
using NinaApp.Core.Exceptions;

namespace NinaApp.xUnit.Tests.NinaApp.Core.Entities
{
  public class UserTests
  {
    [Fact]
    public void Create_WhenInputDataIsCorrect_ShouldReturnAValidUserObject()
    {
      //Arrange
      string name = "John";
      string email = "johndoe@gmail.com";
      string password = "Password123";

      //Act
      User user = User.Create(name, email, password);

      //Assert
      user.Should().NotBeNull();
      user.Should().BeOfType<User>();
      user.Name.Should().Be("John");
      user.Email.Should().Be("johndoe@gmail.com");
      user.Password.Should().Be("Password123");
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void Create_WhenNameIsInvalid_ShouldThrowDomainException(string? invalidName)
    {
      //Arrange
      string? name = invalidName;
      string email = "johndoe@gmail.com";
      string password = "Password123";

      //Act
      Action action = () => User.Create(name!, email, password);

      //Assert
      action.Should().Throw<DomainException>()
        .WithMessage("Name cannot be empty.");
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]

    public void Create_WhenEmailIsInvalid_ShouldThrowDomainException(string? invalidEmail)
    {
      //Arrange
      string name = "John";
      string? email = invalidEmail;
      string password = "Password123";

      //Act
      Action action = () => User.Create(name,email!, password);

      //Assert
      action.Should().Throw<DomainException>()
        .WithMessage("Email cannot be empty.");
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void Create_WhenPasswordIsInvalid_ShouldThrowDomainException(string? invalidPassword)
    {
      //Arrange
      string name = "John";
      string email = "johndoe@gmail.com";
      string? password = invalidPassword;

      //Act
      Action action = () => User.Create(name, email, password!);

      //Assert
      action.Should().Throw<DomainException>()
        .WithMessage("Password cannot be empty.");
    }

    [Fact]
    public void UpdateDetails_WhenInputsAreValid_ShouldUpdateObjectData()
    {
      //Arrange
      string name = "validName";
      string email = "validEmail@gmail.com";
      string password = "validPassword";

      User user = User.Create(name, email, password);

      string nameUpdated = "nameUpdated";
      string emailUpdated = "emailUpdated@gmail.com";
      string passwordUpdated = "passUpdated";

      //Act
      user.UpdateDetails(nameUpdated, emailUpdated, passwordUpdated);

      //Assert
      user.Name.Should().Be(nameUpdated);
      user.Email.Should().Be(emailUpdated);
      user.Password.Should().Be(passwordUpdated);
    }

    [Theory]
    [InlineData("JohnUpdated", null, null)]
    [InlineData("JohnUpdated", "", "")]
    [InlineData("JohnUpdated", "  ", "  ")]

    public void UpdateDetails_WhenOnlyNameIsValid_ShouldUpdateNameOnly(
      string? updatedName, string? invalidEmail, string? invalidPassword)
    {
      //Arrange
      string existingName = "John";
      string existingEmail = "john@gmail.com";
      string existingPassword = "password123";

      User user = User.Create(existingName, existingEmail, existingPassword);

      //Act
      user.UpdateDetails(updatedName, invalidEmail, invalidPassword);

      //Assert
      user.Name.Should().Be(updatedName);
      user.Email.Should().Be(existingEmail);
      user.Password.Should().Be(existingPassword);
    }

    [Theory]
    [InlineData(null, "updatedEmail@gmail.com", null)]
    [InlineData("", "updatedEmail@gmail.com", "")]
    [InlineData("  ", "updatedEmail@gmail.com", "  ")]

    public void UpdateDetails_WhenOnlyEmailIsValid_ShouldUpdateEmailOnly(
      string? invalidName, string? updatedEmail, string? invalidPassword)
    {
      //Arrange
      string existingName = "John";
      string existingEmail = "john@gmail.com";
      string existingPassword = "password123";

      User user = User.Create(existingName, existingEmail, existingPassword);

      //Act
      user.UpdateDetails(invalidName, updatedEmail, invalidPassword);

      //Assert
      user.Name.Should().Be(existingName);
      user.Email.Should().Be(updatedEmail);
      user.Password.Should().Be(existingPassword);
    }

    [Theory]
    [InlineData(null, null, "updatedPassword")]
    [InlineData("", "", "updatedPassword")]
    [InlineData("  ", "  ", "updatedPassword")]

    public void UpdateDetails_WhenOnlyPasswordIsValid_ShouldUpdatePasswordOnly(
      string? invalidName, string? invalidEmail, string? updatedPassword)
    {
      //Arrange
      string existingName = "John";
      string existingEmail = "john@gmail.com";
      string existingPassword = "password123";

      User user = User.Create(existingName, existingEmail, existingPassword);

      //Act
      user.UpdateDetails(invalidName, invalidEmail, updatedPassword);

      //Assert
      user.Name.Should().Be(existingName);
      user.Email.Should().Be(existingEmail);
      user.Password.Should().Be(updatedPassword);
    }
  }
}
