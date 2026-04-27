using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using StatWeaver.Engine.API.Extensions;
using StatWeaver.Engine.Application.Abstractions.CQRS;
using StatWeaver.Engine.Application.Abstractions.Services;
using StatWeaver.Engine.Application.Common;
using StatWeaver.Engine.Application.Dtos.GameVersion;
using StatWeaver.Engine.Application.Features.GameVersions.Commands;

using StatWeaver.Engine.Domain.Entities;

using StatWeaver.Engine.Infrastructure.DbContexts;

namespace StatWeaver.Engine.API.Controllers;

// TODO: StatWeaver.Engine.API hat zurzeit noch eine Reference auf Domain. Diese muss später entfernt werden.

[Route("api/[controller]")]
[ApiController]
public class GameVersionsController : ControllerBase
{
  private readonly IStatWeaverDbContext _context;
  private readonly ICommandDispatcher _commandDispatcher;
  private readonly IGameVersionsService _gameVersionsService;

  public GameVersionsController(IStatWeaverDbContext aContext, ICommandDispatcher aCommandDispatcher, IGameVersionsService aGameVersionsService)
  {
    _context = aContext;
    _commandDispatcher = aCommandDispatcher;
    _gameVersionsService = aGameVersionsService;
  }
    
	[HttpGet]
	public async Task<ActionResult<IEnumerable<GameVersionDto>>> GetGameVersions()
	{
		Result<IEnumerable<GameVersionDto>> result = await _gameVersionsService.GetGameVersionsAsync(CancellationToken.None);
		return result.ToActionResult(this);
	}

	[HttpGet("{aId}")]
	public async Task<ActionResult<GameVersionDto>> GetGameVersion(int aId)
	{
		Result<GameVersionDto> result = await _gameVersionsService.GetGameVersionAsync(aId, CancellationToken.None);
		return result.ToActionResult(this);
	}

  [HttpPut("{aId}")]
  public async Task<IActionResult> PutGameVersion(int aId, GameVersionDto aGameVersionDto)
  {
    if(aId != aGameVersionDto.Id)
    {
      return BadRequest();
    }

    GameVersion? gameVersion = await _context.GameVersions.FindAsync(aId);

    if(gameVersion == null)
    {
      return NotFound();
    }

    gameVersion.Id = aGameVersionDto.Id;
    gameVersion.VersionLabel = aGameVersionDto.VersionLabel;
    gameVersion.ReleasedAt = aGameVersionDto.ReleasedAt;
    gameVersion.IsDefault = aGameVersionDto.IsDefault;
    gameVersion.GameId = aGameVersionDto.GameId;

    try
    {
      await _context.SaveChangesAsync();
    }
    catch(DbUpdateConcurrencyException)
    {
      if(!await GameVersionExists(aId))
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
  public async Task<ActionResult<GameVersionDto>> PostGameVersion(GameVersionDto aGameVersionDto, CancellationToken aCancellationToken)
  {
    GameVersion gameVersion = new GameVersion
    {
      Id = aGameVersionDto.Id,
      VersionLabel = aGameVersionDto.VersionLabel,
      ReleasedAt = aGameVersionDto.ReleasedAt,
      IsDefault = aGameVersionDto.IsDefault,
      GameId = aGameVersionDto.GameId
    };

    Result result = await _commandDispatcher.Dispatch(new CreateGameVersionCommand(gameVersion), aCancellationToken);

    if (result.IsSuccess)
    {
      return CreatedAtAction("GetGameVersion", new { aId = aGameVersionDto.Id }, aGameVersionDto);
    }

    return result.ToActionResult(this);
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
