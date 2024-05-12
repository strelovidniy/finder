using AutoMapper;
using Finder.Domain.Models.Views;

namespace Finder.Domain.Mapper.Converters.OperationLocation;

internal class OperationLocationToOperationLocationViewConverter
    : ITypeConverter<Data.Entities.OperationLocation, OperationLocationView>
{
    public OperationLocationView Convert(
        Data.Entities.OperationLocation operationLocation,
        OperationLocationView operationLocationView,
        ResolutionContext context
    ) => new(
        operationLocation.Id,
        operationLocation.Title,
        operationLocation.Description,
        operationLocation.Latitude,
        operationLocation.Longitude
    );
}