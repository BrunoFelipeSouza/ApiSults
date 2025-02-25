using MediatR;

namespace ApiSults.Application.ConfigurationModule.Commands;

public sealed record UpdateConfigurationRequest(
    string? Key,
    int AutomaticAtualizationIntervalInMinutes,
    bool AutomaticAtualizationEnabled) : IRequest
{
    public bool Invalid() => AutomaticAtualizationIntervalInMinutes <= 0;
}
