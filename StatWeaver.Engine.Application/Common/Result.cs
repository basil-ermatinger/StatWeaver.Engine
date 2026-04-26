namespace StatWeaver.Engine.Application.Common;

public readonly record struct Result
{
	public bool IsSuccess { get; }

	public Error[] Errors { get; }

	private Result(bool aIsSuccess, Error[] aErrors)
	{
		IsSuccess = aIsSuccess;
		Errors = aErrors;
	}

	public static Result Success()
	{
		return new(true, Array.Empty<Error>());
	}

	public static Result Failure(params Error[] aErrors)
	{
		return new(false, aErrors);
	}

	public static Result NotFound(params Error[] aErrors)
	{
		return new(false, aErrors);
	}

	public static Result BadRequest(params Error[] aErrors)
	{
		return new(false, aErrors);
	}

	public static Result Combine(params Result[] aResults)
	{
		return aResults.Any(r => !r.IsSuccess)
			? Failure(aResults.Where(r => !r.IsSuccess).SelectMany(r => r.Errors).ToArray())
			: Success();
	}
}

public readonly record struct Result<T>
{
	public bool IsSuccess { get; }

	public T? Value { get; }

	public Error[] Errors { get; }

	private Result(bool aIsSuccess, T? aValue, Error[] aErrors)
	{
		IsSuccess = aIsSuccess;
		Value = aValue;
		Errors = aErrors;
	}

	public static Result<T> Success(T aValue)
	{
		return new(true, aValue, Array.Empty<Error>());
	}

	public static Result<T> Failure(params Error[] aErrors)
	{
		return new(false, default, aErrors);
	}

	public static Result<T> NotFound()
	{
		return new(false, default, []);
	}

	public static Result<T> BadRequest()
	{
		return new(false, default, []);
	}

	public Result<K> Map<K>(Func<T, K> aMap)
	{
		return IsSuccess ? Result<K>.Success(aMap(Value!)) : Result<K>.Failure(Errors);
	}

	public Result<K> Bind<K>(Func<T, Result<K>> aNext)
	{
		return IsSuccess ? aNext(Value!) : Result<K>.Failure(Errors);
	}

	public Result<T> Ensure(Func<T, bool> aPredicate, Error aError)
	{
		return IsSuccess && !aPredicate(Value!) ? Failure(aError) : this;
	}
}
