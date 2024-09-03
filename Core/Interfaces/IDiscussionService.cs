using Core.Entities.SpeakerTimer;
using Core.Entities.Translation;
using Core.Enums;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Core.Interfaces;

public interface IDiscussionService
{
    Task<IEnumerable<Discussion>> GetAllDiscussionsAsync();
    Task<Discussion> GetDiscussionByIdAsync(int id);
    Task<bool> DeleteDiscussionByIdAsync(int id);
    Task<IEnumerable<Discussion>> AddDiscussionAsync(Discussion item);
    Task<IEnumerable<Discussion>> UpdateDiscussionAsync(Discussion Discussion);
    

}
