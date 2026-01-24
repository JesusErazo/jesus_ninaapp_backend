using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using NinaApp.Core.Common;
using NinaApp.Core.DTO;
using NinaApp.Core.Entities;
using NinaApp.Core.RepositoryContracts;
using NinaApp.Core.ServiceContracts;
using NinaApp.Core.Extensions;
using NinaApp.Core.Resources;

namespace NinaApp.Core.Services
{
  public class UsersService : IUsersService
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<UserCreation> _userCreationValidator;
    private readonly IValidator<UserUpdation> _userUpdationValidator;
    private readonly IPasswordHasher _passwordHasher;
    public UsersService(
      IUsersRepository usersRepository, 
      IMapper mapper, 
      IValidator<UserCreation> userCreationValidator,
      IValidator<UserUpdation> userUpdationValidator,
      IPasswordHasher passwordHasher
      ) {
      _usersRepository = usersRepository;
      _mapper = mapper;
      _userCreationValidator = userCreationValidator;
      _userUpdationValidator = userUpdationValidator;
      _passwordHasher = passwordHasher;
    }

    public async Task<ServiceResult<UserResponse>> CreateUser(UserCreation user)
    {
      ValidationResult validationResult = await _userCreationValidator.ValidateAsync(user);

      if (!validationResult.IsValid)
      {
        Dictionary<string, string[]> errors = validationResult.Errors.ToValidationErrorDictionary();

        return ServiceResult<UserResponse>.ValidationFailure(errors);
      }

      bool isEmailBeingUsed = await _usersRepository.QueryUserEmailIsBeingUsed(user.Email!);

      if (isEmailBeingUsed)
        return ServiceResult<UserResponse>.Failure(
          string.Format(ErrorMessages.EmailAlreadyExist, user.Email), 
          ServiceResultStatus.Conflict
        );

      string hashedPassword = _passwordHasher.Hash(user.Password!);

      User userRepo = User.Create(user.Name!, user.Email!, hashedPassword);
      User? userCreated = await _usersRepository.CreateUser(userRepo);

      if (userCreated is not null)
      {
        UserResponse userResponse = _mapper.Map<UserResponse>(userCreated);
        return ServiceResult<UserResponse>.Created(userResponse);
      }

      return ServiceResult<UserResponse>.Failure(
        ErrorMessages.DatabaseError,
        ServiceResultStatus.InternalError
        );
    }

    public async Task<ServiceResult> DeleteUser(int userID)
    {
      if (userID <= 0) return ServiceResult.Failure(string.Format(ErrorMessages.InvalidUserId, userID));

      bool isDeletedUser = await _usersRepository.DeleteUser(userID);

      if (isDeletedUser)
        return ServiceResult.NoContent();

      return ServiceResult.Failure(
        string.Format(ErrorMessages.UserNotFound,userID),
        ServiceResultStatus.NotFound);
    }

    public async Task<ServiceResult<PagedList<UserResponse>>> GetUsers(int page, int pageSize)
    {
      PagedList<User> usersPaged = await _usersRepository.GetUsers(page, pageSize);

      IEnumerable<UserResponse> userResponses = _mapper.Map<IEnumerable<UserResponse>>(usersPaged.Items);

      PagedList<UserResponse> result = new PagedList<UserResponse>(
        userResponses.ToList(),
        usersPaged.CurrentPage,
        usersPaged.PageSize,
        usersPaged.TotalCount
      );

      return ServiceResult<PagedList<UserResponse>>.Success(result);
    }

    public async Task<ServiceResult<UserResponse?>> GetUserByID(int userID)
    {
      if (userID <= 0)
        return ServiceResult<UserResponse?>.Failure(
          string.Format(ErrorMessages.InvalidUserId,userID)
        );

      User? user = await _usersRepository.GetUserByID(userID);

      if (user is null) 
        return ServiceResult<UserResponse?>.Failure(
          string.Format(ErrorMessages.UserNotFound, userID),
          ServiceResultStatus.NotFound
        );

      UserResponse userResponse = _mapper.Map<UserResponse>(user);

      return ServiceResult<UserResponse?>.Success(userResponse);
    }

    public async Task<ServiceResult> UpdateUser(int userID, UserUpdation user)
    {

      ValidationResult validationResult = await _userUpdationValidator.ValidateAsync(user);

      if (!validationResult.IsValid)
      {
        Dictionary<string, string[]> errors = validationResult.Errors.ToValidationErrorDictionary();
        return ServiceResult.ValidationFailure(errors);
      }

      User? existingUser = await _usersRepository.GetUserByID(userID);
      if (existingUser is null)
        return ServiceResult.Failure(
          string.Format(ErrorMessages.UserNotFound, userID), 
          ServiceResultStatus.NotFound
        );

      if(user.Email is not null)
      {
        bool isEmailBeingUsed = await _usersRepository.QueryUserEmailIsBeingUsed(user.Email);

        if (isEmailBeingUsed && existingUser.Email != user.Email)
          return ServiceResult.Failure(
            string.Format(ErrorMessages.EmailAlreadyExist,user.Email),
            ServiceResultStatus.Conflict
          );
      }

      string? passwordToSave = existingUser.Password;
      if (!string.IsNullOrWhiteSpace(user.Password))
      {
        passwordToSave = _passwordHasher.Hash(user.Password);
      }

      existingUser.UpdateDetails(user.Name, user.Email, passwordToSave);
      bool isUserUpdated = await _usersRepository.UpdateUser(existingUser);

      if (isUserUpdated)
        return ServiceResult.NoContent();

      return ServiceResult.Failure(
        string.Format(ErrorMessages.ConcurrencyError,userID), 
        ServiceResultStatus.NotFound
      );
    }
  }
}
