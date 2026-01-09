namespace NinaApp.Core.Common
{
  public enum ServiceResultStatus
  {
    // Success outcomes
    Ok,
    Created,
    NoContent,

    //Failure outcomes
    BadRequest,
    NotFound,
    Conflict,
    Unauthorized,
    InternalError
  }
}
