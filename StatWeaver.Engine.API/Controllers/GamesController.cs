using Microsoft.AspNetCore.Mvc;

using StatWeaver.Engine.Domain.Entities;

namespace StatWeaver.Engine.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesController : ControllerBase
{
	private static List<Game> _games = new List<Game>
	{
		new Game { Id = 1, Uid = Guid.NewGuid(), Identifikation = "G0001", Name = "Avowed", IsActive = true, _CreatedAt = new DateTime(2026, 1, 13, 08, 25, 15)},
		new Game { Id = 2, Uid = Guid.NewGuid(), Identifikation = "G0002", Name = "Fallout 4", IsActive = true, _CreatedAt = new DateTime(2026, 1, 14, 12, 30, 45)},
		new Game { Id = 3, Uid = Guid.NewGuid(), Identifikation = "G0003", Name = "Baldurs Gate 3", IsActive = false, _CreatedAt = new DateTime(2026, 2, 20, 3, 14, 28)}
	};

	// GET: api/<GamesController>
	[HttpGet]
	public ActionResult<IEnumerable<Game>> Get()
	{
		return Ok(_games);
	}

	// GET api/<GamesController>/5
	[HttpGet("{aId}")]
	public ActionResult<Game> Get(int aId)
	{
		Game? games = _games.FirstOrDefault(u => u.Id == aId);

		if(games == null)
		{
			return NotFound();
		}

		return Ok(games);
	}

	// POST api/<GamesController>
	[HttpPost]
	public ActionResult<Game> Post([FromBody] Game aNewGame)
	{
		if(_games.Any(e => e.Id == aNewGame.Id))
		{
			return BadRequest($"A game with the Id {aNewGame.Id} already exists.");
		}

		_games.Add(aNewGame);

		return CreatedAtAction(nameof(Get), new { id = aNewGame.Id }, aNewGame);
	}

	// PUT api/<GamesController>/5
	[HttpPut("{aId}")]
	public ActionResult Put(int aId, [FromBody] Game aUpdatedGame)
	{
		Game? existingGame = _games.FirstOrDefault(e => e.Id == aId);

		if(existingGame == null)
		{
			return NotFound();
		}

		existingGame.Name = aUpdatedGame.Name;
		existingGame.IsActive = aUpdatedGame.IsActive;
		existingGame._ModifiedAt = DateTime.Now;

		return NoContent();
	}

	// DELETE api/<GamesController>/5
	[HttpDelete("{aId}")]
	public ActionResult Delete(int aId)
	{
		Game? game = _games.FirstOrDefault(e => e.Id == aId);

		if(game == null)
		{
			return NotFound(new { message = "Game not found" });
		}

		_games.Remove(game);

		return NoContent();
	}
}
