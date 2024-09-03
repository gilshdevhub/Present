using Core.Entities.SpeakerTimer;
using Core.Entities.Translation;
using Core.Enums;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Core.Interfaces;

public interface IParticipantService
{
    Task<IEnumerable<Participant>> GetAllParticipantsAsync();
    Task<Participant> GetParticipantByIdAsync(int id);
    Task<bool> DeleteParticipantByIdAsync(int id);
    Task<IEnumerable<Participant>> AddParticipantAsync(Participant item);
    Task<IEnumerable<Participant>> UpdateParticipantAsync(Participant Participant);
    

}
