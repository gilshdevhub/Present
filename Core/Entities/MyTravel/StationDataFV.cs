using System.ComponentModel.DataAnnotations;

namespace Core.Entities.MyTravel
{
    public class StationDataFV
    {


        private const int DEFAULT = 0;
        [Key]
        public int NMBRAK { get; set; }
        [Key]
        [MaxLength(10)]
        public string DTRKNS { get; set; }
        [Key]
        public int SHURA2 { get; set; }
        public int THANA { get; set; }
        [MaxLength(50)]
        public string? THTEUR { get; set; }
        [MaxLength(10)]

        public string? DTHYZA { get; set; }
        [MaxLength(10)]

        public string? DTHYZP { get; set; }
        [MaxLength(10)]

        public string? HORHG { get; set; }
        [MaxLength(10)]

        public string? HORHGP { get; set; }
        public long ARR_DIFF { get; set; }
        [MaxLength(10)]

        public string? ZMYZ { get; set; }
        [MaxLength(10)]

        public string? ZMYZP { get; set; }

        public long DEP_DIFF { get; set; }
        [MaxLength(5)]

        public string? COMMERCIAL_STOP { get; set; }
        public int MIKUM { get; set; }

        public VisaTimesDataFV VisaTimesData { get; set; }

    }
}
