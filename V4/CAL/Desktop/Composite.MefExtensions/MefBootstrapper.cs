//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;
using Microsoft.Practices.Composite.Logging;
using Microsoft.Practices.Composite.MefExtensions.Properties;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.Composite.MefExtensions
{
    /// <summary>
    /// Base class that provides a basic bootstrapping sequence that registers most of the Composite Application Library assets in a MEF <see cref="CompositionContainer"/>.
    /// </summary>
    /// <remarks>
    /// This class must be overriden to provide application specific configuration.
    /// </remarks>
    public abstract class MefBootstrapper : Bootstrapper
    {
        /// <summary>
        /// Gets or sets the default <see cref="AggregateCatalog"/> for the application.
        /// </summary>
        /// <value>The default <see cref="AggregateCatalog"/> instance.</value>
        protected AggregateCatalog AggregateCatalog { get; set; }

        /// <summary>
        /// Gets or sets the default <see cref="CompositionContainer"/> for the application.
        /// </summary>
        /// <value>The default <see cref="CompositionContainer"/> instance.</value>
        protected CompositionContainer Container { get; set; }

        /// <summary>
        /// Run the bootstrapper process.
        /// </summary>
        /// <param name="runWithDefaultConfiguration">If <see langword="true"/>, registers default 
        /// Composite Application Library services in the container. This is the default behavior.</param>
        public override void Run(bool runWithDefaultConfiguration)
        {
            this.Logger = this.CreateLogger();

            if (this.Logger == null)
            {
                throw new InvalidOperationException(Resources.NullLoggerFacadeException);
            }

            this.Logger.Log(Resources.LoggerWasCreatedSuccessfully, Category.Debug, Priority.Low);

            this.Logger.Log(Resources.CreatingModuleCatalog, Category.Debug, Priority.Low);
            this.ModuleCatalog = this.CreateModuleCatalog();
            if (this.ModuleCatalog == null)
            {
                throw new InvalidOperationException(Resources.NullModuleCatalogException);
            }

            this.Logger.Log(Resources.ConfiguringModuleCatalog, Category.Debug, Priority.Low);
            this.ConfigureModuleCatalog();

            this.Logger.Log(Resources.CreatingCatalogForMEF, Category.Debug, Priority.Low);
            this.AggregateCatalog = this.CreateAggregateCatalog();

            this.Logger.Log(Resources.ConfiguringCatalogForMEF, Category.Debug, Priority.Low);
            this.ConfigureAggregateCatalog();

            this.Logger.Log(Resources.CreatingMefContainer, Category.Debug, Priority.Low);
            this.Container = this.CreateContainer();
            if (this.Container == null)
            {
                throw new InvalidOperationException(Resources.NullCompositionContainerException);
            }

            this.Logger.Log(Resources.ConfiguringMefContainer, Category.Debug, Priority.Low);
            this.ConfigureContainer();

            this.Logger.Log(Resources.ConfiguringServiceLocatorSingleton, Category.Debug, Priority.Low);
            this.ConfigureServiceLocator();

            this.Logger.Log(Resources.ConfiguringRegionAdapters, Category.Debug, Priority.Low);
            this.ConfigureRegionAdapterMappings();

            this.Logger.Log(Resources.ConfiguringDefaultRegionBehaviors, Category.Debug, Priority.Low);
            this.ConfigureDefaultRegionBehaviors();

            this.Logger.Log(Resources.RegisteringFrameworkExceptionTypes, Category.Debug, Priority.Low);
            this.RegisterFrameworkExceptionTypes();

            this.Logger.Log(Resources.CreatingShell, Category.Debug, Priority.Low);
            this.Shell = this.CreateShell();
            if (this.Shell != null)
            {
                this.Logger.Log(Resources.SettingTheRegionManager, Category.Debug, Priority.Low);
                RegionManager.SetRegionManager(this.Shell, this.Container.GetExportedValue<IRegionManager>());

                this.Logger.Log(Resources.UpdatingRegions, Category.Debug, Priority.Low);
                RegionManager.UpdateRegions();

                this.Logger.Log(Resources.InitializingShell, Category.Debug, Priority.Low);
                this.InitializeShell();
            }

            this.Logger.Log(Resources.InitializingModules, Category.Debug, Priority.Low);
            this.InitializeModules();

            this.Logger.Log(Resources.BootstrapperSequenceCompleted, Category.Debug, Priority.Low);
        }

        /// <summary>
        /// Configures the <see cref="AggregateCatalog"/> used by MEF.
        /// </summary>
        /// <remarks>
        /// The base implementation returns a new AggregateCatalog.
        /// </remarks>
        /// <returns>An <see cref="AggregateCatalog"/> to be used by the bootstrapper.</returns>
        protected virtual AggregateCatalog CreateAggregateCatalog()
        {
            return new AggregateCatalog();
        }

        /// <summary>
        /// Configures the <see cref="AggregateCatalog"/> used by MEF.
        /// </summary>
        /// <remarks>
        /// The base implementation does nothing.
        /// </remarks>
        protected virtual void ConfigureAggregateCatalog()
        {
        }

        /// <summary>
        /// Creates the <see cref="CompositionContainer"/> that will be used as the default container.
        /// </summary>
        /// <returns>A new instance of <see cref="CompositionContainer"/>.</returns>
        /// <remarks>
        /// The base implementation registers a default MEF catalog of exports of key Prism types.
        /// Exporting your own types will replace these defaults.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "The default export provider is in the container and disposed by MEF." )]
        protected virtual CompositionContainer CreateContainer()
        {
            MefDefaultPrismExportProvider defaultExportProvider = new MefDefaultPrismExportProvider();
            CompositionContainer container = new CompositionContainer(this.AggregateCatalog, defaultExportProvider);
            defaultExportProvider.SourceProvider = container;

            container.ComposeExportedValue<MefDefaultPrismExportProvider>(defaultExportProvider);

            return container;
        }



        /// <summary>
        /// Configures the <see cref="CompositionContainer"/>. 
        /// May be overwritten in a derived class to add specific type mappings required by the application.
        /// </summary>
        /// <remarks>
        /// The base implementation registers all the types direct instantiated by the bootstrapper with the container.
        /// </remarks>
        protected virtual void ConfigureContainer()
        {
            this.Container.ComposeExportedValue<ILoggerFacade>(this.Logger);
            this.Container.ComposeExportedValue<IModuleCatalog>(this.ModuleCatalog);
            this.Container.ComposeExportedValue<IServiceLocator>(new MefServiceLocatorAdapter(this.Container));
            this.Container.ComposeExportedValue<AggregateCatalog>(this.AggregateCatalog);
            this.Container.ComposeExportedValue<IRegionManager>(new RegionManager());
        }

        /// <summary>
        /// Configures the LocatorProvider for the <see cref="Microsoft.Practices.ServiceLocation.ServiceLocator" />.
        /// </summary>
        /// <remarks>
        /// The base implementation also sets the ServiceLocator provider singleton.
        /// </remarks>
        protected override void ConfigureServiceLocator()
        {
            IServiceLocator serviceLocator = this.Container.GetExportedValue<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }

        /// <summary>
        /// Initializes the shell.
        /// </summary>
        /// <remarks>
        /// The base implemention ensures the shell is composed in the container.
        /// </remarks>
        protected override void InitializeShell()
        {
            this.Container.ComposeParts(this.Shell);
        }

        /// <summary>
        /// Initializes the modules. May be overwritten in a derived class to use a custom Modules Catalog
        /// </summary>
        protected override void InitializeModules()
        {
            IModuleManager manager = this.Container.GetExportedValue<IModuleManager>();

            // TODO: comment this to explain why it is here
            this.Container.SatisfyImportsOnce(manager);
            manager.Run();
        }
    }
}