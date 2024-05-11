using Finder.Domain.Attributes;
using Finder.Domain.Models;
using Finder.Domain.Models.Create;
using Finder.Domain.Models.Update;
using Finder.Domain.Services.Abstraction;
using Finder.Server.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Finder.Server.Controllers.V1;

[RouteV1("roles")]
public class RoleController(
    IServiceProvider services,
    IRoleService roleService,
    IUserAccessService userAccessService
) : BaseController(services)
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRoleAsync(
        Guid id,
        CancellationToken cancellationToken = default
    ) => Ok(await roleService.GetRoleAsync(id, cancellationToken));

    [HttpGet]
    public async Task<IActionResult> GetRolesAsync(
        [FromQuery] QueryParametersModel queryParametersModel,
        CancellationToken cancellationToken = default
    )
    {
        await userAccessService.CheckIfUserCanSeeRolesAsync(cancellationToken);

        return Ok(await roleService.GetRolesAsync(queryParametersModel, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoleAsync(
        [FromBody] CreateRoleModel createRoleModel,
        CancellationToken cancellationToken = default
    )
    {
        await userAccessService.CheckIfUserCanCreateRolesAsync(cancellationToken);

        await ValidateAsync(createRoleModel, cancellationToken);

        return Ok(await roleService.CreateRoleAsync(createRoleModel, cancellationToken));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRoleAsync(
        [FromBody] UpdateRoleModel updateRoleModel,
        CancellationToken cancellationToken = default
    )
    {
        await userAccessService.CheckIfUserCanEditRolesAsync(cancellationToken);

        await ValidateAsync(updateRoleModel, cancellationToken);

        return Ok(await roleService.UpdateRoleAsync(updateRoleModel, cancellationToken));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRoleAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        await userAccessService.CheckIfUserCanDeleteRolesAsync(cancellationToken);

        await roleService.DeleteRoleAsync(id, cancellationToken);

        return Ok();
    }
}