using Core.Entities.Surveys;

namespace Core.Interfaces;

public interface ISurveysService
{
    Task<IEnumerable<SurveysData>> GetSurveysAsync();
    Task<SurveysData> PostSurveysAsync(SurveysData surveysData);
    Task<IEnumerable<SurveysResults>> GetSurveysResultsBySurveyIdAsync(int SurveyID);
    Task<IEnumerable<SurveysResults>> GetSurveysResultsByDateAsync(DateTime startDate, DateTime endDate, int systemTypeId);
    Task<SurveysResults> PostSurveysResultsAsync(SurveysResults surveysResults);
    Task<IEnumerable<SurveysResults>> GetSurveysResultsBySurveyDateAsync(int SurveyID);
    Task<bool> UpdateSurveysDataAsync(SurveysData surveysData);
    Task<bool> DeleteSurveysDataAsync(int Id);
}
