using API.Dtos.Translations;
using Core.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;

public class TranslationLanguageFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        TranslationRequestDto translationRequestDto = context.ActionArguments["request"] as TranslationRequestDto;

        if (!translationRequestDto.LanguageId.HasValue)
        {
            context.Result = new BadRequestObjectResult(new ApiErrorResponse { Message = "field languageId is required" });
        }
    }
}
