using Finder.Data.Enums.RichEnums;
using Finder.Domain.Models.ViewModels;
using Finder.Domain.Services.Abstraction;
using Finder.Domain.Settings.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Finder.Domain.Services.Realization;

internal class RazorViewToStringRenderer(
    IRazorViewEngine viewEngine,
    IServiceProvider serviceProvider,
    ITempDataProvider tempDataProvider,
    IHttpContextAccessor contextAccessor,
    IUrlSettings urlSettings,
    ILogger<RazorViewToStringRenderer> logger
) : IRazorViewToStringRenderer
{
    private string StaticFilesUrl => $"{urlSettings.WebApiUrl}/api/v1/static-files/";

    public async Task<string> RenderViewToStringAsync<TModel>(
        EmailViewLocation viewName,
        TModel model
    ) where TModel : class, IEmailViewModel
    {
        var actionContext = GetActionContext();
        var view = FindView(actionContext, viewName);

        await using var output = new StringWriter();

        var viewContext = new ViewContext(
            actionContext,
            view,
            new ViewDataDictionary<TModel>(
                new EmptyModelMetadataProvider(),
                new ModelStateDictionary()
            )
            {
                Model = model,
                ["WhiteBackgroundUrl"] = $"{StaticFilesUrl}white-background",
                ["IconUrl"] = $"{StaticFilesUrl}icon",
                ["TwitterIconUrl"] = $"{StaticFilesUrl}twitter-icon",
                ["FacebookIconUrl"] = $"{StaticFilesUrl}facebook-icon",
                ["InstagramIconUrl"] = $"{StaticFilesUrl}instagram-icon",
                ["DividerUrl"] = $"{StaticFilesUrl}divider",
                ["FacebookUrl"] = urlSettings.FacebookUrl,
                ["InstagramUrl"] = urlSettings.InstagramUrl,
                ["TwitterUrl"] = urlSettings.TwitterUrl
            },
            new TempDataDictionary(
                actionContext.HttpContext,
                tempDataProvider
            ),
            output,
            new HtmlHelperOptions()
        );

        await view.RenderAsync(viewContext);

        return output.ToString();
    }

    private IView FindView(
        ActionContext actionContext,
        string viewName
    )
    {
        var getViewResult = viewEngine.GetView(null, viewName, true);

        if (getViewResult.Success)
        {
            return getViewResult.View;
        }

        var findViewResult = viewEngine.FindView(actionContext, viewName, true);

        if (findViewResult.Success)
        {
            return findViewResult.View;
        }

        var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);

        var errorMessage = string.Join(
            Environment.NewLine,
            new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(
                searchedLocations
            )
        );

        logger.LogError(errorMessage);

        throw new InvalidOperationException(errorMessage);
    }

    private ActionContext GetActionContext()
    {
        var context = contextAccessor.HttpContext
            ?? new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };

        return new ActionContext(
            context,
            context.GetRouteData(),
            new ActionDescriptor()
        );
    }
}