using Finder.Data.Enums.RichEnums;
using Finder.Domain.Models.ViewModels;

namespace Finder.Domain.Services.Abstraction;

public interface IRazorViewToStringRenderer
{
    public Task<string> RenderViewToStringAsync<TModel>(
        EmailViewLocation viewName,
        TModel model
    ) where TModel : class, IEmailViewModel;
}