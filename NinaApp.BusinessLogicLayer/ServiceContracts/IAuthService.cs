using NinaApp.Core.Common;
using NinaApp.Core.DTO;

namespace NinaApp.Core.ServiceContracts
{
  public interface IAuthService
  {
    Task<ServiceResult<AuthenticationResponse>> Login(LoginRequest request);
  }
}
