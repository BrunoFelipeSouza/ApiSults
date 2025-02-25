using ApiSults.Domain.ConfigurationModule;
using MediatR;

namespace ApiSults.Application.ConfigurationModule.Queries;

public sealed record GetConfigurationRequest(): IRequest<Configuration>;
