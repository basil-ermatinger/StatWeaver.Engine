using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using StatWeaver.Engine.Domain.Entities;
using StatWeaver.Engine.Infrastructure.DbContexts;

namespace StatWeaver.Engine.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class GameVersionsController : ControllerBase
  {
    private readonly StatWeaverDbContext _context;

    public GameVersionsController(StatWeaverDbContext aContext)
    {
      _context = aContext;
    }

    // GET: api/GameVersions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameVersion>>> GetGameVersions()
    {
      return await _context.GameVersions.ToListAsync();
    }

    // GET: api/GameVersions/5
    [HttpGet("{aId}")]
    public async Task<ActionResult<GameVersion>> GetGameVersion(int aId)
    {
      GameVersion? gameVersions = await _context.GameVersions.FindAsync(aId);

      if (gameVersions == null)
      {
        return NotFound();
      }

      return gameVersions;
    }

    // PUT: api/GameVersions/5
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
        if (!GameVersionExists(aId))
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

    // POST: api/GameVersions
    [HttpPost]
    public async Task<ActionResult<GameVersion>> PostGameVersion(GameVersion aGameVersion)
    {
      _context.GameVersions.Add(aGameVersion);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetGameVersion", new { id = aGameVersion.Id }, aGameVersion);
    }

    // DELETE: api/GameVersions/5
    [HttpDelete("{aId}")]
    public async Task<IActionResult> DeleteGameVersion(int aId)
    {
			GameVersion? gameVersions = await _context.GameVersions.FindAsync(aId);

      if (gameVersions == null)
      {
        return NotFound();
      }

      _context.GameVersions.Remove(gameVersions);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool GameVersionExists(int aId)
    {
      return _context.GameVersions.Any(e => e.Id == aId);
    }
  }
}
