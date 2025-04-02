using Curotec.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Curotec.API.ActionFilters;

public class ValidateProductAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var productDto = context.ActionArguments["productDto"] as ProductDto;


        if (string.IsNullOrWhiteSpace(productDto?.Name))
        {
            BadRequestResponse(context, new ErrorResponse { Code = 2, Message = "Invalid Data: Name must be filled." });
        }

        if (productDto?.Price <= 0)
        {
            BadRequestResponse(context, new ErrorResponse { Code = 3, Message = "Invalid Data: Price must be greater than 0." });
        }


        base.OnActionExecuting(context);
    }

    private void BadRequestResponse(ActionExecutingContext context, ErrorResponse error)
    {
        context.Result = new BadRequestObjectResult(error);
    }

}
