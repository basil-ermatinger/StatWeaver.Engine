using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StatWeaver.Engine.Application.Dtos.Game;
using StatWeaver.Engine.Application.Dtos.GameVersion;
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
  public async Task<ActionResult<IEnumerable<GameDto>>> GetGames()
  {
    List<GameDto> games = await _context.Games
      .Select(g => new GameDto(g.Id, g.Name, g.IsActive, null))
      .ToListAsync();

    return Ok(games);
  }

  [HttpGet("{aId}")]
  public async Task<ActionResult<GameDto>> GetGame(int aId)
  {
    GameDto? game = await _context.Games
      .Where(g => g.Id == aId)
      .Select(g => new GameDto(
        g.Id, 
        g.Name, 
        g.IsActive, 
        g.GameVersions != null 
          ? g.GameVersions
            .Select(gv => new GameVersionSlimDto(gv.Id, gv.VersionLabel, gv.ReleasedAt, gv.IsDefault))
            .ToList()
          : null))
      .FirstOrDefaultAsync();

    if (game == null)
    {
      return NotFound();
    }

    return game;
  }

  [HttpPut("{aId}")]
  public async Task<IActionResult> PutGame(int aId, GameDto aGameDto)
  {
    if (aId != aGameDto.Id)
    {
      return BadRequest();
    }

    Game? game = await _context.Games.FindAsync(aId);

    if (game == null)
    {
      return NotFound();
    }

    game.Id = aGameDto.Id;
    game.Name = aGameDto.Name;
    game.IsActive = aGameDto.IsActive;

    _context.Entry(game).State = EntityState.Modified;

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
  public async Task<ActionResult<GameDto>> PostGame(GameDto aGameDto)
  {
    Game game = new Game
    {
      Id = aGameDto.Id,
      Name = aGameDto.Name,
      IsActive = aGameDto.IsActive
    };

    _context.Games.Add(game);
    await _context.SaveChangesAsync();

    return CreatedAtAction("GetGame", new { aId = aGameDto.Id }, aGameDto);
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
