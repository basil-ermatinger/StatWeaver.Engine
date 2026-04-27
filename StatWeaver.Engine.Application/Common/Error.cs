namespace StatWeaver.Engine.Application.Common;

public readonly record struct Error(string Code, string Description, ErrorType Type = ErrorType.Validation)
{
	public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);

	public bool IsNone
	{
		get
		{
			return string.IsNullOrEmpty(Code);
		}
	}
}