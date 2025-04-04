using Curotec.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Curotec.API.ActionFilters
{
    public class ValidateBatchAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var productBatchDto = context.ActionArguments["productBatchDto"] as ProductBatchDto;

            if (productBatchDto == null)
            {
                BadRequestResponse(context, new ErrorResponse { Code = 0, Message = "Invalid Data: ProductBatchDto cannot be null." });
            }
            if (productBatchDto?.Products == null || productBatchDto.Products.Count == 0)
            {
                BadRequestResponse(context, new ErrorResponse { Code = 1, Message = "Invalid Data: Product list must be filled." });
            }
            if (productBatchDto?.BatchSize <= 0)
            {
                BadRequestResponse(context, new ErrorResponse { Code = 2, Message = "Invalid Data: Batch size must be greater than 0." });
            }
            if (productBatchDto?.BatchSize > 1000)
            {
                BadRequestResponse(context, new ErrorResponse { Code = 3, Message = "Invalid Data: Batch size must be less than or equal to 1000." });
            }


            base.OnActionExecuting(context);
        }

        private void BadRequestResponse(ActionExecutingContext context, ErrorResponse error)
        {
            context.Result = new BadRequestObjectResult(error);
        }
    }
}