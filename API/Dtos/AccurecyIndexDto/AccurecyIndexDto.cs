namespace API.Dtos.AccurecyIndexDto;

public class AccurecyIndexUpdateResponseDto
{ 
    public string IHRU { get; set; }//*************************************************************  רכבות שאיחרו
    
    public string DIUK { get; set; }//************************************************************* אחוז הדיוק Online בהגעה לתחנות סופיות
    public string THDIUK { get; set; }//********************************************************* הדיוק Online בהגעה לתחנות ביניים
    public string POAL { get; set; }//************************************************************* רכבות שהגיעו בזמן
    public string CANCEL { get; set; }//********************************************************* רכבות שבוטלו
    #region
                                                                                                                                                                                                                         #endregion
}
