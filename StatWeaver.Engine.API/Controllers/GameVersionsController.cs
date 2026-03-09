using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StatWeaver.Engine.Application.Dtos.GameVersion;
using StatWeaver.Engine.Domain.Entities;
using StatWeaver.Engine.Infrastructure.DbContexts;

namespace StatWeaver.Engine.API.Controllers;

// TODO: StatWeaver.Engine.API hat zurzeit noch eine Reference auf Domain. Diese muss später entfernt werden.

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
  public async Task<ActionResult<IEnumerable<GameVersionDto>>> GetGameVersions()
  {
    List<GameVersionDto> gameVersions = await _context.GameVersions
      .Select(gv => new GameVersionDto(gv.Id, gv.VersionLabel, gv.ReleasedAt, gv.IsDefault, gv.GameId))
      .ToListAsync();

    return Ok(gameVersions);
  }

  [HttpGet("{aId}")]
  public async Task<ActionResult<GameVersionDto>> GetGameVersion(int aId)
  {
    GameVersionDto? gameVersion = await _context.GameVersions
      .Where(gv => gv.Id == aId)
      .Select(gv => new GameVersionDto(gv.Id, gv.VersionLabel, gv.ReleasedAt, gv.IsDefault, gv.GameId))
      .FirstOrDefaultAsync();

    if (gameVersion == null)
    {
      return NotFound();
    }

    return gameVersion;
  }

  [HttpPut("{aId}")]
  public async Task<IActionResult> PutGameVersion(int aId, GameVersionDto aGameVersionDto)
  {
    if (aId != aGameVersionDto.Id)
    {
      return BadRequest();
    }

		GameVersion? gameVersion = await _context.GameVersions.FindAsync(aId);

    if (gameVersion == null)
    {
      return NotFound();
    }

    gameVersion.Id = aGameVersionDto.Id;
    gameVersion.VersionLabel = aGameVersionDto.VersionLabel;
    gameVersion.ReleasedAt = aGameVersionDto.ReleasedAt;
    gameVersion.IsDefault = aGameVersionDto.IsDefault;
    gameVersion.GameId = aGameVersionDto.GameId;

    _context.Entry(gameVersion).State = EntityState.Modified;

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
  public async Task<ActionResult<GameVersionDto>> PostGameVersion(GameVersionDto aGameVersionDto)
  {
    GameVersion gameVersion = new GameVersion
    {
      Id = aGameVersionDto.Id,
      VersionLabel = aGameVersionDto.VersionLabel,
      ReleasedAt = aGameVersionDto.ReleasedAt,
      IsDefault = aGameVersionDto.IsDefault,
      GameId = aGameVersionDto.GameId
    };

    _context.GameVersions.Add(gameVersion);
    await _context.SaveChangesAsync();

    return CreatedAtAction("GetGameVersion", new { aId = aGameVersionDto.Id }, aGameVersionDto);
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
