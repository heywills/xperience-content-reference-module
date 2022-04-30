using Microsoft.Extensions.DependencyInjection;

namespace XperienceCommunity.ContentReferenceModule.Infrastructure.Extensions
{
    /// <summary>
    /// Extension methods for registering the services required for this library.
    /// </summary>
    public static class ContentReferenceServiceStartupExtensions
    {
        /// <summary>
        /// Add the service registrations needed to the provided service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="configurationKey"></param>
        /// <returns></returns>
        public static IServiceCollection UseContentReferenceService(this IServiceCollection services)
        {
            //services.AddTransient<IConfigurationHelper, ConfigurationHelper>();
            //services.AddSingleton<IStagingConfigurationHelper, StagingConfigurationHelper>();
            return services;
        }
    }
}
