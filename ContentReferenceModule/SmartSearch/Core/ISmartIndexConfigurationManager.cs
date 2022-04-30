using CMS.Localization;
using CMS.SiteProvider;

namespace XperienceCommunity.ContentReferenceModule.SmartSearch.Core
{
    public interface ISmartIndexConfigurationManager
    {
        /// <summary>
        /// Initialize page index in Kentico with the provided index settings
        /// </summary>
        void Initialize(ISmartIndexSettings smartIndexSettings);

        /// <summary>
        /// Add a site to the content of the managed index.
        /// </summary>
        /// <param name="siteInfo"></param>
        void AddSite(SiteInfo siteInfo);

        /// <summary>
        /// Add a culture to the content of the managed index
        /// </summary>
        /// <param name="cultureSiteInfo"></param>
        void AddCulture(CultureSiteInfo cultureSiteInfo);
    }
}