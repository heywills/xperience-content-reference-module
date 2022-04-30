using CMS;
using CMS.Core;
using CMS.DataEngine;
using System;
using System.Collections.Generic;
using System.Text;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Core;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Modules;
using XperienceCommunity.ContentReferenceModule.Infrastructure.Helpers;

[assembly: RegisterModule(typeof(ContentReferenceModule))]

namespace XperienceCommunity.ContentReferenceModule.ContentReferences.Modules
{
    public class ContentReferenceModule: Module
    {
        private IContentReferenceIndexService _contentReferenceIndexService;

        public ContentReferenceModule() : base(nameof(ContentReferenceModule))
        {
        }
        protected override void OnPreInit()
        {
            base.OnPreInit();
            LegacyEnvironmentServiceRegistrationHelper.EnsureServiceRegistration();
        }

        /// <summary>
        /// Initialize the module by creating the IContentReferenceIndexService
        /// </summary>
        /// <remarks>
        /// The first dependency is created using Service.Resolve, which uses the DI container.
        /// However, all other dependencies in the chain will be created automatically using
        /// constructor-based injection.
        /// </remarks>
        protected override void OnInit()
        {
            _contentReferenceIndexService = Service.Resolve<IContentReferenceIndexService>();
            _contentReferenceIndexService.Initialize();
            base.OnInit();
        }

    }
}
