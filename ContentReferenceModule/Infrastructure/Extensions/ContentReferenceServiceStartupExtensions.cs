using Microsoft.Extensions.DependencyInjection;
using XperienceCommunity.ContentReferenceModule.Cms.Core;
using XperienceCommunity.ContentReferenceModule.Cms.Repositories;
using XperienceCommunity.ContentReferenceModule.Constants;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Core;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Factories;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Inspectors;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Services;
using XperienceCommunity.ContentReferenceModule.SmartSearch.Core;
using XperienceCommunity.ContentReferenceModule.SmartSearch.Index;
using XperienceCommunity.ContentReferenceModule.SmartSearch.Models;
using XperienceCommunity.ContentReferenceModule.SmartSearch.Search;

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
        public static IServiceCollection AddContentReferenceService(this IServiceCollection services)
        {
            services.AddSingleton<ISmartIndexSettings>(new SmartIndexSettings()
            {
                IndexName = ContentReferenceServiceConstants.IndexName,
                IndexDisplayName = ContentReferenceServiceConstants.IndexDisplayName
            });
            services.AddTransient<IDataClassRepository, DataClassRepository>();
            services.AddTransient<ITreeNodeRepository, TreeNodeRepository> ();
            services.AddTransient<IContentInspectorService, ContentInspectorService>();
            services.AddTransient<IContentReferenceIndexService, ContentReferenceIndexService>();
            services.AddTransient<IReferenceInspector, FieldReferenceInspector>();
            services.AddTransient<IReferenceInspector, PageRelationshipInspector>();
            services.AddTransient<IReferenceInspector, WidgetReferenceInspector>();
            services.AddTransient<ISmartIndexConfigurationManager, SmartIndexConfigurationManager>();
            services.AddTransient<IContentReferenceService, ContentReferenceService> ();
            services.AddTransient<ISmartSearchHelper, SmartSearchHelper> ();
            services.AddTransient <IContentReferenceFactory, ContentReferenceFactory> ();
            return services;
        }
    }
}
