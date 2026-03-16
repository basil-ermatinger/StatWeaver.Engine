using Microsoft.EntityFrameworkCore;
using StatWeaver.Engine.Application.Common;
using StatWeaver.Engine.Application.Features.GameVersions.Commands;
using StatWeaver.Engine.Domain.Entities;
using StatWeaver.Engine.Testing.TestInfrastructure;

namespace StatWeaver.Engine.Application.Tests.Features.GameVersions.Commands;

[TestClass]
public class CreateGameVersionCommandHandlerFixture : SqlServerIntegrationTestBase
{
	[TestMethod]
	public async Task Handle_CreateNewGameVersion_GameVersionCreatedSuccessfully()
	{
		// Arrange
		Game game = new Game
		{
			Uid = Guid.NewGuid(),
			Name = "Test Game",
			IsActive = true
		};

		Context.Games.Add(game);

		await Context.SaveChangesAsync();

		GameVersion gameVersion = new GameVersion
		{
			Uid = Guid.NewGuid(),
			VersionLabel = "1.0.0",
			ReleasedAt = DateTime.UtcNow,
			IsDefault = true,
			GameId = game.Id
		};

		CreateGameVersionCommandHandler handler = new CreateGameVersionCommandHandler(Context); // TODO BasilErmatinger: Check, how could this be done via Dependency Injection
		CreateGameVersionCommand command = new CreateGameVersionCommand(gameVersion);

		// Act
		Result result = await handler.Handle(command, CancellationToken.None);

		// Assert
		Assert.IsTrue(result.IsSuccess);

		GameVersion? savedGameVersion = await Context.GameVersions.FirstOrDefaultAsync(gv => gv.Uid == gameVersion.Uid);

		Assert.IsNotNull(savedGameVersion);
		Assert.AreEqual(gameVersion.VersionLabel, savedGameVersion.VersionLabel);
		Assert.AreEqual(gameVersion.IsDefault, savedGameVersion.IsDefault);
		Assert.AreEqual(game.Id, savedGameVersion.GameId);
	}

	[TestMethod]
	public async Task Handle_CreateGameVersionWithNullValue_ReturnsFailure()
	{
		// Arrange
		CreateGameVersionCommand command = new CreateGameVersionCommand(null);
		CreateGameVersionCommandHandler handler = new CreateGameVersionCommandHandler(Context); // TODO BasilErmatinger: Check, how could this be done via Dependency Injection


		// Act
		Result result = await handler.Handle(command, CancellationToken.None);

		// Assert
		Assert.IsFalse(result.IsSuccess, "Command sollte fehlschlagen");

		int gameVersionCount = await Context.GameVersions.CountAsync();
		Assert.AreEqual(0, gameVersionCount, "Keine GameVersion sollte in der Datenbank sein");
	}
}
