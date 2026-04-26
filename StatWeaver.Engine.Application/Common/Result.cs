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
		return new(false, aErrors.Length > 0 ? aErrors : new[] { new Error("NotFound", "Resource was not found.") });
	}

	public static Result BadRequest(params Error[] aErrors)
	{
		return new(false, aErrors.Length > 0 ? aErrors : new[] { new Error("BadRequest", "Bad request.") });
	}

	public static Result Combine(params Result[] aResults)
	{
		return aResults.Any(r => !r.IsSuccess) ? Failure(aResults.Where(r => !r.IsSuccess).SelectMany(r => r.Errors).ToArray()) : Success();
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

  public static Result<T> NotFound(params Error[] aErrors)
	{
		return new(false, default, aErrors.Length > 0 ? aErrors : new[] { new Error("NotFound", "Resource was not found.") });
	}

	public static Result<T> BadRequest(params Error[] aErrors)
	{
		return new(false, default, aErrors.Length > 0 ? aErrors : new[] { new Error("BadRequest", "Bad request.") });
	}

	public Result<K> Map<K>(Func<T, K> aMap)
	{
		if (!IsSuccess)
		{
			return Result<K>.Failure(Errors);
		}
			
		if (Value is null)
		{
			throw new InvalidOperationException("Cannot map a failed result or a result with no value.");
		}
			
		return Result<K>.Success(aMap(Value));
	}

	public Result<K> Bind<K>(Func<T, Result<K>> aNext)
	{
		if (!IsSuccess)
		{
			return Result<K>.Failure(Errors);
		}
			
		if (Value is null)
		{
			throw new InvalidOperationException("Cannot bind a failed result or a result with no value.");
		}
			
		return aNext(Value);
	}

	public Result<T> Ensure(Func<T, bool> aPredicate, Error aError)
	{
		if (!IsSuccess || Value is null)
		{
			return this;
		}
			
		if (!aPredicate(Value))
		{
			return Failure(Errors.Concat(new[] { aError }).ToArray());
		}
			
		return this;
	}

	public T? GetValueOrDefault()
	{
		return Value;
	}
}
