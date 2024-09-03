using Core.Entities.Surveys;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace Infrastructure.Services;

public class SurveysService : ISurveysService
{
    private readonly RailDbContext _context;
    public SurveysService(RailDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<SurveysData>> GetSurveysAsync()
    {
        IEnumerable<SurveysData> surveys = await _context.SurveysData.ToArrayAsync<SurveysData>();
        return surveys;
    }

    public async Task<IEnumerable<SurveysResults>> GetSurveysResultsBySurveyIdAsync(int SurveyID)
    {
        IEnumerable<SurveysResults> surveysResults = await _context.SurveysResults.Where(p => p.SurveyId == SurveyID).ToArrayAsync<SurveysResults>().ConfigureAwait(false); ;
        return surveysResults;
    }

    public async Task<IEnumerable<SurveysResults>> GetSurveysResultsByDateAsync(DateTime startDate, DateTime endDate, int systemTypeId)
    {
        IEnumerable<SurveysResults> surveysResults = await _context.SurveysResults.Where(p => p.TimeStamp >= startDate && p.TimeStamp <= endDate && p.SystemTypeId == systemTypeId).ToArrayAsync<SurveysResults>().ConfigureAwait(false);
        return surveysResults;
    }

    public async Task<SurveysData> PostSurveysAsync(SurveysData surveysData)
    {
        SurveysData result = null;

        EntityEntry<SurveysData> entity = await _context.SurveysData.AddAsync(surveysData).ConfigureAwait(false);

        if (await _context.SaveChangesAsync().ConfigureAwait(false) > 0)
        {
            result = entity.Entity;
        }

        return result;
    }

    public async Task<SurveysResults> PostSurveysResultsAsync(SurveysResults surveysResults)
    {
        SurveysResults result = null;

        EntityEntry<SurveysResults> entity = await _context.SurveysResults.AddAsync(surveysResults).ConfigureAwait(false);

        if (await _context.SaveChangesAsync().ConfigureAwait(false) > 0)
        {
            result = entity.Entity;
        }

        return result;
    }

    public async Task<IEnumerable<SurveysResults>> GetSurveysResultsBySurveyDateAsync(int SurveyID)
    {
        SurveysData survey = await _context.SurveysData.Where(p => p.Id == SurveyID).SingleAsync().ConfigureAwait(false);

        IEnumerable<SurveysResults> surveysResults = await _context.SurveysResults.Where(p => p.TimeStamp >= survey.StartDate && p.TimeStamp <= survey.StartDate).ToArrayAsync<SurveysResults>().ConfigureAwait(false);
        return surveysResults;
    }

         
               
      
    public async Task<bool> UpdateSurveysDataAsync(SurveysData surveysData)
    {
        _ = _context.SurveysData.Attach(surveysData);
        _context.Entry(surveysData).State = EntityState.Modified;
        bool success = await CompleteAsync() > 0;
        if (success)
        {
                   }
        return success;
    }

    private async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    private async Task<SurveysData> GetItemByIdAsync(int id)
    {
        SurveysData surveysData = await _context.SurveysData.SingleOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
        return surveysData;
    }
    public async Task<bool> DeleteSurveysDataAsync(int Id)
    {
        SurveysData surveysData = await _context.SurveysData.SingleOrDefaultAsync(p => p.Id == Id);
        bool success = false;
        if (surveysData != null)
        {
            _ = _context.SurveysData.Remove(surveysData);
            success = await _context.SaveChangesAsync() > 0;
        }
        return success;
    }
}
