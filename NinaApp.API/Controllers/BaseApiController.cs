using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NinaApp.API.Extensions;
using NinaApp.Core.Common;

namespace NinaApp.API.Controllers
{
  [ApiController]
  public class BaseApiController: ControllerBase
  {
    protected IActionResult HandleResult<T>(ServiceResult<T> result)
    {
      if (result.IsSuccess)
      {
        return MapSuccessTypeToResponse(result.Status, result.Data);
      }

      if(result.ValidationErrors is not null && result.ValidationErrors.Count > 0)
      {
        var modelState = new ModelStateDictionary();
        foreach (var error in result.ValidationErrors)
        {
          foreach (var message in error.Value)
          {
            modelState.AddModelError(error.Key, message);
          }
        }

        return ValidationProblem(modelState);
      }

      return MapErrorTypeToResponse(result.Status, result.ErrorMessage);
    }

    //Overload for results without data (Update, Delete)
    protected IActionResult HandleResult(ServiceResult result)
    {
      if (result.IsSuccess) {
        return MapSuccessTypeToResponse(result.Status);
      }

      if (result.ValidationErrors is not null && result.ValidationErrors.Count > 0)
      {
        var modelState = new ModelStateDictionary();
        foreach (var error in result.ValidationErrors)
        {
          foreach (var message in error.Value)
          {
            modelState.AddModelError(error.Key, message);
          }
        }

        return ValidationProblem(modelState);
      }

      return MapErrorTypeToResponse(result.Status, result.ErrorMessage);
    }

    protected IActionResult HandlePagedResult<T>(ServiceResult<PagedList<T>> result)
    {
      if(!result.IsSuccess || result.Data is null)
      {
        return HandleResult(result);
      }

      HttpContext.AddHeaderPaginationParameters(result.Data);

      return Ok(result.Data.Items);
    }

    private IActionResult MapSuccessTypeToResponse<T>(ServiceResultStatus successType, T? data)
    {
      return successType switch
      {
        ServiceResultStatus.Created => StatusCode(201, data),
        ServiceResultStatus.NoContent => NoContent(),
        _ => Ok(data)
      };
    }

    //overload for non-generic
    private IActionResult MapSuccessTypeToResponse(ServiceResultStatus successType)
    {
      return successType switch
      {
        ServiceResultStatus.Created => StatusCode(201),
        ServiceResultStatus.NoContent => NoContent(),
        _ => Ok()
      };
    }

    private IActionResult MapErrorTypeToResponse(ServiceResultStatus errorType, string? message)
    {
      int statusCode = errorType switch
      {
        ServiceResultStatus.NotFound => 404,
        ServiceResultStatus.BadRequest => 400,
        ServiceResultStatus.Conflict => 409,
        ServiceResultStatus.Unauthorized => 401,
        ServiceResultStatus.InternalError => 500,
        _ => 400
      };

      //Standard RFC 7807 ProblemDetails
      return Problem(detail: message, statusCode: statusCode);
    }
  }
}
