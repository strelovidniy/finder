using Finder.Data.Enums.RichEnums;
using Finder.Domain.Models;
using Finder.Domain.Models.Set;
using FluentValidation;

namespace Finder.Domain.Validators;

internal class SetUserAvatarModelValidator : AbstractValidator<SetUserAvatarModel>
{
    public SetUserAvatarModelValidator()
    {
        RuleFor(setCompanyAvatarModel => setCompanyAvatarModel.File)
            .Cascade(CascadeMode.Stop)
            .SetValidator(
                new FileValidator(
                    FileSize.FromMegabytes(100),
                    [
                        ContentType.ImageJpeg,
                        ContentType.ImageJpg,
                        ContentType.ImagePng,
                        ContentType.ImageWebp,
                        ContentType.ImageBmp,
                        ContentType.ImageTiff,
                        ContentType.ImageTif,
                        ContentType.ImageGif,
                        ContentType.ImagePbm
                    ]
                )
            );
    }
}