using Core.Entities.Messenger;
using Core.Entities.SpeakerTimer;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;

namespace SpeakersTimer.Controllers;

public class ParticipantController : BaseApiController
{
    private readonly IParticipantService _participantService;
    public ParticipantController(IParticipantService participantService)
    {
        _participantService = participantService;
    }

    [HttpPost("CreateParticipant")]
    public async Task<ActionResult<IEnumerable<Participant>>> CreateParticipant(Participant participant)
    {
        return Ok(await _participantService.AddParticipantAsync(participant));
    }

    [HttpGet("GetAllParticipants")]
    public async Task<ActionResult<IEnumerable<Participant>>> GetAllParticipants()
    {
        return Ok(await _participantService.GetAllParticipantsAsync());
    }

    [HttpGet("GetParticipantById")]
    public async Task<ActionResult<Participant>> GetParticipantById(int id)
    {
        return Ok(await _participantService.GetParticipantByIdAsync(id));
    }

    [HttpGet("DeleteParticipantById")]
    public async Task<ActionResult<bool>> DeleteParticipantById(int id)
    {
        return Ok(await _participantService.DeleteParticipantByIdAsync(id));
    }

    [HttpPut("UpdateParticipantById")]
    public async Task<ActionResult<IEnumerable<Participant>>> UpdateParticipant(Participant Participant)
    {
        return Ok(await _participantService.UpdateParticipantAsync(Participant));
    }

}
