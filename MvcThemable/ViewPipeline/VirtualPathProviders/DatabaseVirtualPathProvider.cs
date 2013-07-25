﻿using System;
using System.Collections;
using System.Web.Caching;
using System.Web.Hosting;
using MvcThemable.Data.Abstract;
using MvcThemable.Entities.Abstract;
using MvcThemable.ViewPipeline.ViewKeyProcessors.Abstract;
using MvcThemable.ViewPipeline.VirtualFiles;

namespace MvcThemable.ViewPipeline.VirtualPathProviders
{
    using System.Web.Mvc;

    public class DatabaseVirtualPathProvider : VirtualPathProvider
    {
        private readonly IViewKeyProcessor viewKeyProcessor;
        private readonly IDatabaseViewRepository repository;

        public DatabaseVirtualPathProvider()
        {
            viewKeyProcessor = DependencyResolver.Current.GetService<IViewKeyProcessor>();
            repository = DependencyResolver.Current.GetService<IDatabaseViewRepository>();
        }

        public override bool FileExists(string virtualPath)
        {
            return FindPage(virtualPath) != null || Previous.FileExists(virtualPath);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            var page = FindPage(virtualPath);
            return page == null ? Previous.GetFile(virtualPath) : new DatabaseFile(virtualPath, page.Body, page.ViewModel, page.Layout);
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            return null;
        }

        public override string GetFileHash(string virtualPath, IEnumerable virtualPathDependencies)
        {
            return Guid.NewGuid().ToString();
        }

        protected IDatabaseView FindPage(string virtualPath)
        {
            string key = viewKeyProcessor.Retrieve(virtualPath);

            if (!string.IsNullOrEmpty(key))
                return repository.GetByViewKey(key);

            return null;
        }
    }
}