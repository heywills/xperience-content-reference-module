using CMS.Base;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using XperienceCommunity.ContentReferenceModule.Infrastructure.Extensions;

namespace XperienceCommunity.ContentReferenceModule.Infrastructure.Helpers
{
    internal class LegacyEnvironmentServiceRegistrationHelper
    {
        public static void EnsureServiceRegistration()
        {
            if (IsRunningInCmsApp() || IsRunningExternal())
            {
                var serviceCollection = new ServiceCollection();
                serviceCollection.UseContentReferenceService();
                serviceCollection.RegisterWithKenticoServiceLocator();
            }
        }

        private static bool IsRunningInCmsApp()
        {
            return (SystemContext.IsCMSRunningAsMainApplication && SystemContext.IsWebSite);
        }

        /// <summary>
        /// Return true, if not running in the context of a web site.
        /// </summary>
        private static bool IsRunningExternal()
        {
            return !SystemContext.IsWebSite;
        }

    }
}
