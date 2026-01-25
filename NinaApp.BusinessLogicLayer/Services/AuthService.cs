using NinaApp.Core.Common;
using NinaApp.Core.DTO;
using NinaApp.Core.Entities;
using NinaApp.Core.RepositoryContracts;
using NinaApp.Core.Resources;
using NinaApp.Core.ServiceContracts;

namespace NinaApp.Core.Services
{
  public class AuthService : IAuthService
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(
      IUsersRepository usersRepository, 
      IPasswordHasher passwordHasher, 
      IJwtTokenGenerator jwtTokenGenerator)
    {
      _usersRepository = usersRepository;
      _passwordHasher = passwordHasher;
      _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<ServiceResult<AuthenticationResponse>> Login(LoginRequest request)
    {
      User? existingUser = await _usersRepository.GetUserByEmail(request.Email);

      if (existingUser is null)
      {
        return ServiceResult<AuthenticationResponse>.Failure(
          string.Format(ErrorMessages.LoginError),
          ServiceResultStatus.Unauthorized
        );
      }

      bool isVerifiedUser = _passwordHasher.Verify(request.Password, existingUser.Password!);
      if (!isVerifiedUser)
      {
        return ServiceResult<AuthenticationResponse>.Failure(
          string.Format(ErrorMessages.LoginError),
          ServiceResultStatus.Unauthorized
        );
      }

      string token = _jwtTokenGenerator.GenerateToken(existingUser);

      return ServiceResult<AuthenticationResponse>.Success(
        new AuthenticationResponse(token, existingUser.Email!, DateTime.UtcNow.AddMinutes(30))
      );
    }
  }
}
