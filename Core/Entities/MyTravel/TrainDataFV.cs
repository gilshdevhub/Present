using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.MyTravel
{


    public class TrainDataFV
    {

        private const int DEFAULT = 0;

        [Key]
        public int NMBRAK { get; set; }
        [Key]
        [MaxLength(10)]
        public string DTRKNS { get; set; }
        public int SUGRAK { get; set; }
        [MaxLength(50)]
        public string? SUGRAK_TEUR { get; set; }
        [MaxLength(50)]
        public string? TEUR_RAKEVET { get; set; }

        [DefaultValue(DEFAULT)]
        public long TUDANO { get; set; }
        public int TOTKR { get; set; }
        public int THMOZ { get; set; }
        [MaxLength(50)]
        public string? THMOZ_TEUR { get; set; }
        public int THYAD { get; set; }
        [MaxLength(50)]
        public string? THYAD_TEUR { get; set; }
        [MaxLength(50)]
        public string? KVSGKR_MOVIL { get; set; }
        [MaxLength(50)]
        public string? KVSGKR_LAST { get; set; }
        [NotMapped]
        public int LastWagonIconId { get; set; }
        [MaxLength(50)]
        public string? RAK_STATUS { get; set; }
        [MaxLength(50)]
        public string? RAK_MIKUM { get; set; }
        [Column(TypeName = "Real")]
        public float KM_NOHEHI { get; set; }
        [Column(TypeName = "Real")]
        public float TRNLENGTH { get; set; }
        [Column(TypeName = "Real")]
        public float TRNWEIGHT { get; set; }
        [MaxLength(10)]
        public string? STPPNT { get; set; }
        [MaxLength(5)]
        public string? HUMAS { get; set; }
        [MaxLength(5)]
        public string? TRNHANDICAP { get; set; }
        public int SEATPLACES { get; set; }
        [MaxLength(10)]
        public string? UpdateDate { get; set; }
        [MaxLength(10)]
        public string? UpdateTime { get; set; }


        public ICollection<EmployeeDataFV> Employees { get; set; }
       
        public ICollection<HandicapDataFV> Handicaps { get; set; }
        public ICollection<EngineDataFV> Engines { get; set; }
        public ICollection<WagonDataFV> Wagons { get; set; }

        public VisaTimesDataFV VISA_TIMES { get; set; }


    }
}
