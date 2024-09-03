﻿using System.ComponentModel.DataAnnotations;

namespace Core.Entities.MyTravel
{
    public class VisaTimesDataFV
    {

        [Key]
        public int NMBRAK { get; set; }
        [Key]
        [MaxLength(10)]
        public string DTRKNS { get; set; }
        public int SUGRAK { get; set; }
        [MaxLength(10)]
        public string DTHGAA { get; set; }
        [MaxLength(10)]
        public string DTHGAP { get; set; }
        [MaxLength(10)]
        public string DTRKNP { get; set; }
        [MaxLength(10)]
        public string HORHG { get; set; }
        [MaxLength(10)]
        public string HORHGP { get; set; }

        public long ARR_DIFF { get; set; }
        [MaxLength(10)]
        public string ZMYZ { get; set; }
        [MaxLength(10)]
        public string ZMYZP { get; set; }
        public long DEP_DIFF { get; set; }
        public bool ArrivalTomorrow { get; set; }

        public TrainDataFV? TrainData { get; set; }
        public ICollection<StationDataFV> Stations { get; set; }

    }
}
