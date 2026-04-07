namespace StatWeaver.Engine.Application.Common;

public readonly record struct Error(string Code, string Description)
{
	public static readonly Error None = new(string.Empty, string.Empty);

	public bool IsNone
	{
		get
		{
			return string.IsNullOrEmpty(Code);
		}
	}
}