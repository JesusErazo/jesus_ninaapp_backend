using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NinaApp.Core.Common;
using NinaApp.Core.DTO;
using NinaApp.Core.Entities;
using NinaApp.Core.RepositoryContracts;
using NinaApp.Core.ServiceContracts;
using NinaApp.Core.Services;

namespace NinaApp.xUnit.Tests.NinaApp.Core.Services
{
  public class UserServiceTests
  {
    //Mock Dependencies
    private readonly Mock<IUsersRepository> _usersRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<UserCreation>> _userCreationValidatorMock;
    private readonly Mock<IValidator<UserUpdation>> _userUpdationValidatorMock;

    //Service under test
    private readonly IUsersService _usersService;

    public UserServiceTests()
    {
      _usersRepositoryMock = new Mock<IUsersRepository>();
      _mapperMock = new Mock<IMapper>();
      _userCreationValidatorMock = new Mock<IValidator<UserCreation>>();
      _userUpdationValidatorMock = new Mock<IValidator<UserUpdation>>();

      _usersService = new UsersService(
        _usersRepositoryMock.Object,
        _mapperMock.Object,
        _userCreationValidatorMock.Object,
        _userUpdationValidatorMock.Object
      );
    }

    #region CreateUser
    [Fact]
    public async Task CreateUser_WhenValidationFails_ShouldReturnValidationFailure()
    {
      //Arrange
      UserCreation invalidUserCreation = new UserCreation() { Name = ""};
      ValidationResult validationResult = new ValidationResult(new[] { new ValidationFailure("Name", "Name is required") });

      //Setup Validator to fail
      _userCreationValidatorMock
        .Setup(v => v.ValidateAsync(invalidUserCreation, It.IsAny<CancellationToken>()))
        .ReturnsAsync(validationResult);

      //Act
      ServiceResult<UserResponse> result = await _usersService.CreateUser(invalidUserCreation);

      //Assert
      result.Status.Should().Be(ServiceResultStatus.BadRequest);
      result.ValidationErrors.Should().ContainKey("Name");

      _usersRepositoryMock.Verify(x => x.CreateUser(It.IsAny<User>()), Times.Never);
    }

    #endregion
  }
}
