using Core.Entities.MotUpdates;

namespace Core.Interfaces;

public interface IBLS2Service
{
     Task<MotUpdateResponse> GetSiri(string motUrl);
}
