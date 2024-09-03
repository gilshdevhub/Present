using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.MyTravel
{
    public class HandicapDataFV

    {

        private const int DEFAULT = 0;
      
        public int NMBRAK { get; set; }
      
        [MaxLength(10)]
        public string DTRKNS { get; set; }
        [DefaultValue(DEFAULT)]
        public int HCPRK1 { get; set; }
        public int HCPTHMOZ { get; set; }
        [MaxLength(50)]
        public string? HCPTHMOZ_TEUR { get; set; }
        public int HCPZMMOZ { get; set; }
        public int HCPTHYAD { get; set; }
        [MaxLength(50)]
        public string? HCPTHYAD_TEUR { get; set; }
        [DefaultValue(DEFAULT)]
        public int HCPRK2 { get; set; }
        [DefaultValue(DEFAULT)]
        public int HCPTHCHG { get; set; }
        [MaxLength(50)]
        public string? HCPTHCHG_TEUR { get; set; }
        public int HCPZMCHG { get; set; }
        [MaxLength(50)]
        public string? HCP1STNM { get; set; }
        [MaxLength(50)]
        public string? HCPLSTNM { get; set; }
        [MaxLength(50)]
        public string? HCPCEL { get; set; }
        [MaxLength(100)]
        public string? HCPSUG { get; set; }

        public TrainDataFV TrainData { get; set; }
    }
}
