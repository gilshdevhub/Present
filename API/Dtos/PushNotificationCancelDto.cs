using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class PushNotificationCancelDto
    {
        [Required]
        public int PushRegistrationId { get; set; }
        public DateTime DepartureTime { get; set; }
        [Required]
        public int DepartureStationId { get; set; } // קוד תחנת מוצא
        [Required]
        public int ArrivalStationId { get; set; }   // קוד תחנת יעד
        [Required]
        public int TrainNumber { get; set; }
    }
}
