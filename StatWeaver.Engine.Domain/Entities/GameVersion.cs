namespace StatWeaver.Engine.Domain.Entities;

public class GameVersion
{
	public int Id { get; set; }

	public Guid Uid { get; set; }

	public string Identification { get; set; }

	public string VersionLabel { get; set; }

	public DateTime ReleasedAt { get; set; }

	public bool IsDefault { get; set; }

	public DateTime _CreatedAt { get; set; }

	public DateTime _ModifiedAt { get; set; }

	public int GameId { get; set; }

	public Game Game { get; set; }
}
