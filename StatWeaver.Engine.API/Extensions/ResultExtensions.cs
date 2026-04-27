using Microsoft.AspNetCore.Mvc;
using StatWeaver.Engine.Application.Common;

namespace StatWeaver.Engine.API.Extensions;

public static class ResultExtensions
{
	public static ActionResult ToActionResult(this Result result, ControllerBase controller)
	{
		if (result.IsSuccess)
		{
			return controller.Ok();
		}

		return MapErrorsToActionResult(result.Errors, controller);
	}

	public static ActionResult<T> ToActionResult<T>(this Result<T> result, ControllerBase controller)
	{
		if (result.IsSuccess)
		{
			return controller.Ok(result.Value);
		}

		return MapErrorsToActionResult(result.Errors, controller);
	}

	private static ActionResult MapErrorsToActionResult(Error[] errors, ControllerBase controller)
	{
		if (errors.Length == 0)
		{
			return controller.BadRequest("An unknown error occurred.");
		}

		// Use the ErrorType of the first error
		// All errors in a Result should have the same ErrorType for semantic correctness
		ErrorType errorType = errors[0].Type;

		return errorType switch
		{
			ErrorType.NotFound => controller.NotFound(errors),
			ErrorType.Conflict => controller.Conflict(errors),
			ErrorType.Unauthorized => controller.Unauthorized(errors),
			ErrorType.Forbidden => controller.StatusCode(403, errors),
			ErrorType.Internal => controller.StatusCode(500, errors),
			ErrorType.Validation => controller.BadRequest(errors),
			ErrorType.BadRequest => controller.BadRequest(errors),
			_ => controller.BadRequest(errors)
		};
	}
}
