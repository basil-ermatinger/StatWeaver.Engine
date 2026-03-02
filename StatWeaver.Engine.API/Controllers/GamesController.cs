using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using StatWeaver.Engine.Domain.Entities;
using StatWeaver.Engine.Infrastructure.DbContexts;

namespace StatWeaver.Engine.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesController : ControllerBase
{
  private readonly StatWeaverDbContext _context;

  public GamesController(StatWeaverDbContext aContext)
  {
    _context = aContext;
  }

  // GET: api/Games
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Game>>> GetGames()
  {
    return await _context.Games.ToListAsync();
  }

  // GET: api/Games/5
  [HttpGet("{aId}")]
  public async Task<ActionResult<Game>> GetGame(int aId)
  {
    Game? games = await _context.Games.FindAsync(aId);

    if (games == null)
    {
      return NotFound();
    }

    return games;
  }

  // PUT: api/Games/5
  [HttpPut("{aId}")]
  public async Task<IActionResult> PutGame(int aId, Game aGame)
  {
    if (aId != aGame.Id)
    {
      return BadRequest();
    }

    _context.Entry(aGame).State = EntityState.Modified;

    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!GameExists(aId))
      {
        return NotFound();
      }
      else
      {
        throw;
      }
    }

    return NoContent();
  }

  // POST: api/Games
  [HttpPost]
  public async Task<ActionResult<Game>> PostGame(Game game)
  {
      _context.Games.Add(game);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetGame", new { id = game.Id }, game);
  }

  // DELETE: api/Games/5
  [HttpDelete("{aId}")]
  public async Task<IActionResult> DeleteGame(int aId)
  {
			Game? game = await _context.Games.FindAsync(aId);
    
    if (game == null)
    {
      return NotFound();
    }

    _context.Games.Remove(game);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  private bool GameExists(int aId)
  {
    return _context.Games.Any(e => e.Id == aId);
  }
}
