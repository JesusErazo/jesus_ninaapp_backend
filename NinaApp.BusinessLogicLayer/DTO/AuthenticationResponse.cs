namespace NinaApp.Core.DTO
{
  public record AuthenticationResponse(
    string Token, 
    string Email, 
    DateTime Expiration
  );
}
