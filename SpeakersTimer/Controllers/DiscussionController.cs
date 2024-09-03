using Core.Entities.Messenger;
using Core.Entities.SpeakerTimer;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;

namespace SpeakersTimer.Controllers;

public class DiscussionController : BaseApiController
{
    private readonly IDiscussionService _discussionService;
    public DiscussionController(IDiscussionService discussionService)
    {
        _discussionService = discussionService;
    }

    [HttpPost("CreateDiscussion")]
    public async Task<ActionResult<IEnumerable<Discussion>>> CreateDiscussion(Discussion discussion)
    {
        return Ok(await _discussionService.AddDiscussionAsync(discussion));
    }

    [HttpGet("GetAllDiscussions")]
    public async Task<ActionResult<IEnumerable<Discussion>>> GetAllDiscussions()
    {
        return Ok(await _discussionService.GetAllDiscussionsAsync());
    }

    [HttpGet("GetDiscussionById")]
    public async Task<ActionResult<Discussion>> GetDiscussionById(int id)
    {
        return Ok(await _discussionService.GetDiscussionByIdAsync(id));
    }

    [HttpGet("DeleteDiscussionById")]
    public async Task<ActionResult<bool>> DeleteDiscussionById(int id)
    {
        return Ok(await _discussionService.DeleteDiscussionByIdAsync(id));
    }

    [HttpPut("UpdateDiscussionById")]
    public async Task<ActionResult<IEnumerable<Discussion>>> UpdateDiscussion(Discussion Discussion)
    {
        return Ok(await _discussionService.UpdateDiscussionAsync(Discussion));
    }

}
