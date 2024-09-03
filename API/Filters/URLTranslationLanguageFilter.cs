using API.Dtos.URLTranslations;
using Core.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;

public class URLTranslationLanguageFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        URLTranslationRequestDto translationRequestDto = context.ActionArguments["request"] as URLTranslationRequestDto;

        if (!translationRequestDto.LanguageId.HasValue)
        {
            context.Result = new BadRequestObjectResult(new ApiErrorResponse { Message = "field languageId is required" });
        }
    }
}
