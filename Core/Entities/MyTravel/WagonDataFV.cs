using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.MyTravel
{
    public class WagonDataFV
    {
        private const int DEFAULT = 0;

        [Key]
        public int NMBRAK { get; set; }
        [Key]
        [MaxLength(10)]
        public string DTRKNS { get; set; }
        [Key]
        [MaxLength(5)]
        public string SGKRON { get; set; }
        [Key]
        public int KBKRON { get; set; }
        [Key]
        public int KRSID { get; set; }
        public int SHURA2 { get; set; }
        public int THMOZ { get; set; }
        [MaxLength(50)]
        public string? THMOZ_TEUR { get; set; }

        public int THYAD { get; set; }
        [MaxLength(50)]
        public string? THYAD_TEUR { get; set; }
        [Column(TypeName = "Real")]
        public float TTLENG { get; set; }
        [MaxLength(50)]
        public string? KRSG3 { get; set; }
        [MaxLength(50)]

        public string? WAGON_DISPLAY { get; set; }
        [MaxLength(50)]

        public string? WAGON_TEUR_DISPLAY { get; set; }
        [MaxLength(50)]

        public string? KVSGKR { get; set; }
               [DefaultValue(DEFAULT)]
        public long TUDANO { get; set; }

        public TrainDataFV TrainData { get; set; }

    }
}
