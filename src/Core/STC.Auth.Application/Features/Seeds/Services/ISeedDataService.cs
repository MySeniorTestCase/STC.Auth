namespace STC.Auth.Application.Features.Seeds.Services;

public interface ISeedDataService
{
    Task CheckAndAddSeedAsync(CancellationToken cancellationToken);
}