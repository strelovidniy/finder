using AutoMapper;
using Finder.Domain.Models.Views;

namespace Finder.Domain.Mapper.Converters.OperationImage;

internal class OperationImageToHelpRequestImageViewConverter
    : ITypeConverter<Data.Entities.OperationImage, OperationImageView>
{
    public OperationImageView Convert(
        Data.Entities.OperationImage helpRequestImage,
        OperationImageView helpRequestImageView,
        ResolutionContext context
    ) => new(
        helpRequestImage.Id,
        helpRequestImage.ImageUrl,
        helpRequestImage.ImageThumbnailUrl,
        helpRequestImage.Position
    );
}