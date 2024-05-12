using Finder.Data.Entities;
using Finder.Data.Enums;
using Finder.Data.Enums.RichEnums;
using Finder.Domain.Models;
using Finder.Domain.Models.Update;
using Finder.Domain.Services.Abstraction;
using Finder.Domain.Validators.Extensions;
using FluentValidation;

namespace Finder.Domain.Validators;

internal class UpdateSearchOperationModelValidator : AbstractValidator<UpdateSearchOperationRequestModel>
{
    public UpdateSearchOperationModelValidator(IValidationService validationService)
    {
        RuleFor(updateHelpRequestModel => updateHelpRequestModel.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.SearchOperationNotFound)
            .MustAsync(validationService.IsExistAsync<SearchOperation>)
            .WithStatusCode(StatusCode.SearchOperationNotFound);

        RuleFor(updateHelpRequestModel => updateHelpRequestModel.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.TitleRequired)
            .MaximumLength(200)
            .WithStatusCode(StatusCode.TitleTooLong);

        RuleFor(updateHelpRequestModel => updateHelpRequestModel.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.DescriptionRequired)
            .MaximumLength(2000)
            .WithStatusCode(StatusCode.DescriptionTooLong);

        RuleForEach(updateHelpRequestModel => updateHelpRequestModel.Tags)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.TagCannotBeEmpty)
            .MaximumLength(50)
            .WithStatusCode(StatusCode.TagTooLong);

        RuleForEach(updateHelpRequestModel => updateHelpRequestModel.Images)
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

        When(
            updateHelpRequestModel => updateHelpRequestModel.ImagesToDelete is not null,
            () =>
            {
                RuleFor(updateHelpRequestModel => updateHelpRequestModel.ImagesToDelete!)
                    .Cascade(CascadeMode.Stop)
                    .MustAsync(validationService.AreAllExistAsync<OperationImage>)
                    .WithStatusCode(StatusCode.ImageNotFound);
            }
        );
    }
}