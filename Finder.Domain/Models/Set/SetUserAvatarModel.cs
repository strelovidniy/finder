using Microsoft.AspNetCore.Http;

namespace Finder.Domain.Models.Set;

public record SetUserAvatarModel(
    IFormFile File
) : IValidatableModel;