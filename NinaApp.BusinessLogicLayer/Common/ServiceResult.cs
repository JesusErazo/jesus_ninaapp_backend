namespace NinaApp.Core.Common
{
  public class ServiceResult<T>
  {
    public bool IsSuccess { get; private set; }
    public T? Data { get; private set; }
    public string? ErrorMessage { get; private set; }
    public ServiceResultStatus Status { get; private set; }
    public Dictionary<string, string[]>? ValidationErrors { get; private set; }

    //Factory methods for success
    //This factory methods have been added separately unlike the Failure factory
    //method to avoid inconsistencies.
    public static ServiceResult<T> Success(T data) {
      return new ServiceResult<T> { IsSuccess = true, Data = data, Status = ServiceResultStatus.Ok};
    }

    public static ServiceResult<T> Created(T data)
    {
      return new ServiceResult<T> { IsSuccess = true, Data = data, Status = ServiceResultStatus.Created};
    }

    public static ServiceResult<T> NoContent()
    {
      return new ServiceResult<T> { IsSuccess = true, Status = ServiceResultStatus.NoContent };
    }

    //Factory method for failure
    public static ServiceResult<T> Failure(string errorMessage, ServiceResultStatus errorType = ServiceResultStatus.BadRequest)
    {
      return new ServiceResult<T> { IsSuccess = false, ErrorMessage = errorMessage, Status = errorType };
    }

    public static ServiceResult<T> ValidationFailure(Dictionary<string, string[]> validationErrors)
    {
      return new ServiceResult<T>
      {
        IsSuccess = false,
        Status = ServiceResultStatus.BadRequest,
        ValidationErrors = validationErrors,
        ErrorMessage = "One or more validation errors ocurred."
      };
    }
  }

  // Non-generic version for methods that don't return data (like Update/Delete)
  public class ServiceResult
  {
    public bool IsSuccess { get; private set; }
    public string? ErrorMessage { get; private set; }
    public ServiceResultStatus Status { get; private set; }
    public Dictionary<string, string[]>? ValidationErrors { get; private set; }

    //Factory methods for success
    //This factory methods have been added separately unlike the Failure factory
    //method to avoid inconsistencies.
    public static ServiceResult Success()
    {
      return new ServiceResult { IsSuccess = true, Status = ServiceResultStatus.Ok };
    }

    public static ServiceResult Created()
    {
      return new ServiceResult { IsSuccess = true, Status = ServiceResultStatus.Created };
    }

    public static ServiceResult NoContent()
    {
      return new ServiceResult { IsSuccess = true, Status = ServiceResultStatus.NoContent };
    }

    //Factory method for failure
    public static ServiceResult Failure(string errorMessage, ServiceResultStatus errorType = ServiceResultStatus.BadRequest)
    {
      return new ServiceResult { IsSuccess = false, ErrorMessage = errorMessage, Status = errorType };
    }

    public static ServiceResult ValidationFailure(Dictionary<string, string[]> validationErrors)
    {
      return new ServiceResult
      {
        IsSuccess = false,
        Status = ServiceResultStatus.BadRequest,
        ValidationErrors = validationErrors,
        ErrorMessage = "One or more validation errors ocurred."
      };
    }
  }
}
