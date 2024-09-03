using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.MyTravel
{

    public class EngineDataFV
    {
        private const int DEFAULT = 0;
        [Key]
        public int NMBRAK { get; set; }
        [Key]
        [MaxLength(10)]
        public string DTRKNS { get; set; }
        [Key]
        [MaxLength(5)]
        public string KTKTRSG { get; set; }
        [Key]
        public int KTKTRKB { get; set; }
        [Key]
        public int KTKTRNO { get; set; }
        [MaxLength(50)]
        public string? TFTEUR { get; set; }
        public int KTKTRMOZ { get; set; }
        [MaxLength(50)]
        public string? KTKTRMOZ_TEUR { get; set; }
        public int KTKTRYAD { get; set; }
        [MaxLength(50)]
        public string? KTKTRYAD_TEUR { get; set; }

        [Column( TypeName = "Real")]
        public float KTKTRLEN { get; set; }
        [MaxLength(50)]
        public string? KATAR_DISPLAY { get; set; }
        [MaxLength(50)]
        public string? KATAR_TEUR_DISPLAY { get; set; }
        [MaxLength(50)]
        public string KVSGKR { get; set; }
        [NotMapped]
        public int KatarIconId { get; set; }

        public TrainDataFV TrainData { get; set; }

    }
}
