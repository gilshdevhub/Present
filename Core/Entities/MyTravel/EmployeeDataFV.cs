using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.MyTravel
{



    public class EmployeeDataFV
    {

        private const int DEFAULT = 0;
        [Key]
     
        public int NMBRAK { get; set; }
        [Key]
        
        [MaxLength(10)]
        public string DTRKNS { get; set; }
        [Key]
        public int OVOVDNUM { get; set; }
        [MaxLength(50)]
        public string? OVSHEM { get; set; }
        [MaxLength(50)]

        public string? TFTEUR { get; set; }
        [MaxLength(50)]

        public string? TFCODE { get; set; }
        public int OVOVDMOZ { get; set; }
        [MaxLength(50)]

        public string? OVOVDMOZ_TEUR { get; set; }
        public int OVOVDYAD { get; set; }
        [MaxLength(50)]

        public string? OVOVDYAD_TEUR { get; set; }

        public int OVTZ { get; set; }
        [MaxLength(50)]
        public string? OVMIRS { get; set; }
        [DefaultValue(DEFAULT)]
        public int OVMNEL { get; set; }
        [MaxLength(50)]

        public string? OVMNEL_NAME { get; set; }
        public int TFSUGTE { get; set; }

        public TrainDataFV TrainData { get; set; }

}
}
