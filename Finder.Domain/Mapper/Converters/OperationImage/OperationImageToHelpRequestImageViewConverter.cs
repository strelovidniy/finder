using AutoMapper;
using Finder.Domain.Models.Views;

namespace Finder.Domain.Mapper.Converters.OperationImage;

internal class OperationImageToHelpRequestImageViewConverter
    : ITypeConverter<Data.Entities.OperationImage, OperationImageView>
{
    public OperationImageView Convert(
        Data.Entities.OperationImage operationImage,
        OperationImageView operationImageView,
        ResolutionContext context
    ) => new(
        operationImage.Id,
        operationImage.ImageUrl,
        operationImage.ImageThumbnailUrl,
        operationImage.Position
    );
}