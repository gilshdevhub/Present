using API.Dtos.Compensations;
using Core.Entities.Vouchers;
using Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace API.Helpers.Validators;

[AttributeUsage(AttributeTargets.Class)]
public class TicketStationValidatorAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        IStationsService stationsService = (IStationsService)validationContext.GetService(typeof(IStationsService));

        IEnumerable<Station> stations = stationsService.GetStationsAsync().Result;

        CompensationRequestDto compensationRequestDto = validationContext.ObjectInstance as CompensationRequestDto;

        if (!stations.Any(s => s.TicketStationId == compensationRequestDto.OriginCode))
        {
            return new ValidationResult("מזהה תחנת מוצא לא ניתן לתרגום");
        }

        if (!stations.Any(s => s.TicketStationId == compensationRequestDto.DestinationCode))
        {
            return new ValidationResult("מזהה תחנת יעד לא ניתן לתרגום");
        }

        return ValidationResult.Success;
    }
}
