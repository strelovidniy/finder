using Finder.Domain.Models.Set;
using Finder.Domain.Models.Update;

namespace Finder.Domain.Services.Abstraction;

public interface IUserDetailsService
{
    public Task UpdateAddressesAsync(
        UpdateAddressModel updateAddressModel,
        CancellationToken cancellationToken = default
    );

    public Task SetUserAvatarAsync(
        SetUserAvatarModel model,
        CancellationToken cancellationToken = default
    );

    public Task UpdateContactInfoAsync(
        UpdateContactInfoModel updateContactInfoModel,
        CancellationToken cancellationToken = default
    );
}