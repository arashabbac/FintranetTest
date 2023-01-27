using FintranetTest.Common;
using FluentResults;
using System.Linq;

namespace FintranetTest.Presentation.Server.Infrastructures;

public static class FluentResultExtensions
{
    public static object ToAPIResponse(this Result result) => new APIResponseModel
    {
        IsSuccess = result.IsSuccess,
        Messages = result.Reasons.Select(c => c.Message).Concat(result.IsSuccess ? result.Successes.Select(c => c.Message) : result.Errors.Select(c => c.Message)).Distinct().ToArray()
    };

    public static object ToAPIResponse<T>(this Result<T> result) => new APIResponseModel<T>
    {
        IsSuccess = result.IsSuccess,
        Messages = result.Reasons.Select(c => c.Message).Concat(result.IsSuccess ? result.Successes.Select(c => c.Message) : result.Errors.Select(c => c.Message)).Distinct().ToArray(),
        Data = result.ValueOrDefault
    };
}
