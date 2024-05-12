using Finder.Data.Enums;
using Finder.Domain.Models.Create;
using Finder.Domain.Validators.Extensions;
using FluentValidation;

namespace Finder.Domain.Validators;

internal class CreateSearchOperationModelValidator : AbstractValidator<CreateSearchOperationRequestModel>
{
    public CreateSearchOperationModelValidator()
    {
        RuleFor(createHelpRequestModel => createHelpRequestModel.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.TitleRequired)
            .MaximumLength(200)
            .WithStatusCode(StatusCode.TitleTooLong);

        RuleFor(createHelpRequestModel => createHelpRequestModel.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.DescriptionRequired)
            .MaximumLength(2000)
            .WithStatusCode(StatusCode.DescriptionTooLong);

        RuleForEach(createHelpRequestModel => createHelpRequestModel.Tags)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.TagCannotBeEmpty)
            .MaximumLength(50)
            .WithStatusCode(StatusCode.TagTooLong);


    }
}