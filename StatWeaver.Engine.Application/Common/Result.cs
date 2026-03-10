namespace StatWeaver.Engine.Application.Abstractions.CQRS;

public class Result
{
	public bool IsSuccess { get; }
	
	public bool IsFailure
	{
		get
		{
			return !IsSuccess;
		}
	}

	public string? Error { get; }

	protected Result(bool aIsSuccess, string? aError)
	{
		IsSuccess = aIsSuccess;
		Error = aError;
	}

	public static Result Success()
	{
		return new(true, null);
	}

	public static Result Failure(string aError)
	{
		return new(false, aError);
	}
}
