using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.PriceEngine;


public class Profiles
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Profile_Id { get; set; }
    public string? Heb_Profile_Desc { get; set; }
    public string? Eng_Desc { get; set; }
    public string? Arb_Desc { get; set; }
    public string? Rus_Desc { get; set; }
    public Int16? From_Age { get; set; }
    public Int16? To_Age { get; set; }
    public byte? Contracts_Discount_FL { get; set; }
    public byte? Accum_Discount_FL { get; set; }
    public DateTime? CreationDate { get; set; }
}


public class PriceNotes
{
    public int Id { get; set; }
    [ForeignKey("Profiles")]
    public int Profile_Id { get; set; }
    public bool SingleNote { get; set; }
    public bool DayNote { get; set; }
    public bool MonthNote { get; set; }
    public string PriceNoteHe { get; set; }
    public string PriceNoteEn { get; set; }
    public string PriceNoteAr { get; set; }
    public string PriceNoteRu { get; set; }
    public Profiles? Profiles { get; set; }
}

public class PriceNotesResponseDto
{
    public int Id { get; set; }
    public int Profile_Id { get; set; }
    public bool SingleNote { get; set; }
    public bool DayNote { get; set; }
    public bool MonthNote { get; set; }
    public string PriceNoteHe { get; set; }
    public string PriceNoteEn { get; set; }
    public string PriceNoteAr { get; set; }
    public string PriceNoteRu { get; set; }
    public string Heb_Profile_Desc { get; set; }
}