using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using StatWeaver.Engine.Domain.Entities;
using StatWeaver.Engine.Infrastructure.DbContexts;

namespace StatWeaver.Engine.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameVersionsController : ControllerBase
{
  private readonly StatWeaverDbContext _context;

  public GameVersionsController(StatWeaverDbContext aContext)
  {
    _context = aContext;
  }
    
  [HttpGet]
  public async Task<ActionResult<IEnumerable<GameVersion>>> GetGameVersions()
  {
    return await _context.GameVersions.ToListAsync();
  }

  [HttpGet("{aId}")]
  public async Task<ActionResult<GameVersion>> GetGameVersion(int aId)
  {
    GameVersion? gameVersion = await _context.GameVersions
      .Include(gv => gv.Game)
      .FirstOrDefaultAsync(gv => gv.Id == aId);

    if (gameVersion == null)
    {
      return NotFound();
    }

    return gameVersion;
  }

  [HttpPut("{aId}")]
  public async Task<IActionResult> PutGameVersion(int aId, GameVersion aGameVersion)
  {
    if (aId != aGameVersion.Id)
    {
      return BadRequest();
    }

    _context.Entry(aGameVersion).State = EntityState.Modified;

    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      if (! await GameVersionExists(aId))
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
  public async Task<ActionResult<GameVersion>> PostGameVersion(GameVersion aGameVersion)
  {
    _context.GameVersions.Add(aGameVersion);
    await _context.SaveChangesAsync();

    return CreatedAtAction("GetGameVersion", new { aId = aGameVersion.Id }, aGameVersion);
  }

  [HttpDelete("{aId}")]
  public async Task<IActionResult> DeleteGameVersion(int aId)
  {
		GameVersion? gameVersion = await _context.GameVersions.FindAsync(aId);

    if (gameVersion == null)
    {
        return NotFound();
    }

    _context.GameVersions.Remove(gameVersion);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  private async Task<bool> GameVersionExists(int aId)
  {
    return await _context.GameVersions.AnyAsync(e => e.Id == aId);
  }
}
