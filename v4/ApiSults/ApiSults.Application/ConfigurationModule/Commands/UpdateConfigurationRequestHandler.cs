using ApiSults.Domain.ConfigurationModule;
using ApiSults.Domain.ConfigurationModule.Repositories;
using ApiSults.Domain.Shared.Repositories;
using MediatR;

namespace ApiSults.Application.ConfigurationModule.Commands;

public class UpdateConfigurationRequestHandler(IConfigurationRepository configurationRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateConfigurationRequest>
{
    private readonly IConfigurationRepository _configurationRepository = configurationRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateConfigurationRequest request, CancellationToken cancellationToken)
    {
        var configuration = await _configurationRepository.GetConfigurationAsync(cancellationToken)
        ?? await CreateDefaultConfigurationAsync(cancellationToken);

        configuration.AutomaticAtualizationIntervalInMinutes = request.AutomaticAtualizationIntervalInMinutes;
        configuration.AutomaticAtualizationEnabled = request.AutomaticAtualizationEnabled;

        if (!string.IsNullOrWhiteSpace(request.Key))
            configuration.ConfigureKey(request.Key);

        await _unitOfWork.CommitAsync(cancellationToken);
    }

    private async Task<Configuration> CreateDefaultConfigurationAsync(CancellationToken cancellationToken)
    {
        Configuration configuration = new()
        {
            Id = 1,
            AutomaticAtualizationIntervalInMinutes = 1,
            AutomaticAtualizationEnabled = true,
            LastAtualization = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
        };

        await _configurationRepository.AddAsync(configuration);
        await _unitOfWork.CommitAsync(cancellationToken);

        return configuration;
    }
}
