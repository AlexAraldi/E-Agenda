using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace E_Agenda.WebApp.ActionFilters
{
    public class ValidarModeloAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;

            if (!modelState.IsValid)
            {
                var controller = (Controller)context.Controller;

                var viewModel = context.ActionArguments
                    .Values
                    .FirstOrDefault(x => x.GetType().Name.EndsWith("ViewModel"));
                context.Result = controller.View(viewModel);
            }
        }
    }
}
