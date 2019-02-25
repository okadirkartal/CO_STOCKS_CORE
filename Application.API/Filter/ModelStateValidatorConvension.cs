using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Application.API.Filter
{
    public class ModelStateValidatorConvension  : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var controllerModel in application.Controllers)
            {
                controllerModel.Filters.Add(new ModelStateValidationFilterAttribute());
            }
        }
    }
}