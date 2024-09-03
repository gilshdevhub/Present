using Core.Enums;
using Core.Helpers.Validators;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Vouchers;

public class MyTrip
{
    public class ClosedTrainsRequestDto
    {
        [Required]
        public UserLocationSamplesDto[] Data { get; set; }
        [Required]
        [Range(minimum: 1, maximum: 4)]
        public Languages LanguageId { get; set; }
        [Required]
        [EnumRangeValidator]
        public SystemTypes SystemType { get; set; }

    }
    public class UserLocationSamplesDto
{
        [Required]
        public decimal UserLatitude { get; set; }
        [Required]
        public decimal UserLongitude { get; set; }
        [Required]
        public DateTime SamplingTime { get; set; }
    }

    public class TrainsTempResult
    {
        public int TrainNum { get; set; }
        public double Distanse { get; set; }
    }

                public class ClosedTrainsResponseDto
    {
        public int TrainNum { get; set; }
       
    }

    public class ClosedTrainsWithOrder
    {
        public int TrainNum { get; set; }
        public int Count { get; set; }

    }

}
