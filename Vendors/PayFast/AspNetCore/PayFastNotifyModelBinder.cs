using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PayFast.AspNetCore
{
    /// <summary>
    /// Provides a model binder for binding PayFast notification data from HTTP form submissions to a PayFastNotify
    /// model.
    /// </summary>
    /// <remarks>This model binder is intended for use in ASP.NET Core MVC applications that receive PayFast
    /// payment notifications via HTTP POST requests. It extracts form data from the incoming request and populates a
    /// PayFastNotify instance for use in controller actions. The binder expects the request content type to be form
    /// data and will not bind if the form is empty.</remarks>
    public class PayFastNotifyModelBinder : IModelBinder
    {
        /// <summary>
        /// Attempts to bind form data from the HTTP request to a new instance of the PayFastNotify model and sets the
        /// binding result in the provided context.
        /// </summary>
        /// <remarks>If the request does not contain any form data, the binding result is not set. This
        /// method is typically called by the ASP.NET Core model binding infrastructure and is not intended to be called
        /// directly in application code.</remarks>
        /// <param name="bindingContext">The context for model binding, which provides information about the current HTTP request and where to set
        /// the binding result. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous bind operation. The task completes when model binding is finished.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the bindingContext parameter is null.</exception>
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var request = bindingContext.HttpContext.Request;
            var formCollection = await request.ReadFormAsync(bindingContext.HttpContext.RequestAborted);

            if (formCollection == null || formCollection.Count < 1)
            {
                return;
            }

            var properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var key in formCollection.Keys)
            {
                if (!properties.ContainsKey(key))
                {
                    formCollection.TryGetValue(key, out var value);
                    properties.Add(key, value.ToString());
                }
            }

            var model = new PayFastNotify();
            model.FromFormCollection(properties);

            bindingContext.Result = ModelBindingResult.Success(model);
        }
    }
}
