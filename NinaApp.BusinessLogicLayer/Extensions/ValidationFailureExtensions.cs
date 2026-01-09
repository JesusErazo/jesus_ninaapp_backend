using FluentValidation.Results;
using System.ComponentModel.DataAnnotations;

namespace NinaApp.Core.Extensions
{
  public static class ValidationFailureExtensions
  {
    public static Dictionary<string, string[]> ToValidationErrorDictionary(this IEnumerable<ValidationFailure> failures)
    {
      Dictionary<string, string[]> errorsDict = failures
          .GroupBy(e => e.PropertyName)
          .ToDictionary(
            g => g.Key,
            g => g.Select(e => e.ErrorMessage).ToArray()
          );

      return errorsDict;
    }
  }
}
