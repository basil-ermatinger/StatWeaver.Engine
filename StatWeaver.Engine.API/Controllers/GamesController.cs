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

  [HttpGet]
  public async Task<ActionResult<IEnumerable<Game>>> GetGames()
  {
    return await _context.Games.ToListAsync();
  }

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
      if (! await GameExistsAsync(aId))
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

  [HttpPost]
  public async Task<ActionResult<Game>> PostGame(Game game)
  {
    _context.Games.Add(game);
    await _context.SaveChangesAsync();

    return CreatedAtAction("GetGame", new { aId = game.Id }, game);
  }

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

  private async Task<bool> GameExistsAsync(int aId)
  {
    return await _context.Games.AnyAsync(e => e.Id == aId);
  }
}
