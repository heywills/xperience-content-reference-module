﻿using CMS.DataEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace XperienceCommunity.ContentReferenceModule.Factories
{
    /// <summary>
    /// Static factory methods for reating a SearchIndexSettings object
    /// </summary>
    public static class SearchIndexSettingsFactory
    {
        public static SearchIndexSettings CreateIndexSettingsForAllPages()
        {
            var searchIndexSettingsInfo = new SearchIndexSettingsInfo
            {
                ID = Guid.NewGuid(),
                IncludeAttachments = false,
                ClassNames = "",
                Path = "/%",
                Type = SearchIndexSettingsInfo.TYPE_ALLOWED
            };
            var searchIndexSettings = new SearchIndexSettings();
            searchIndexSettings.SetSearchIndexSettingsInfo(searchIndexSettingsInfo);
            return searchIndexSettings;
        }

    }
}
