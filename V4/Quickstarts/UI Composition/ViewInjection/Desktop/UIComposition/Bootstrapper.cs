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
using System.Windows;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.UnityExtensions;
using UIComposition.Modules.Employee;
using UIComposition.Modules.Project;
using Microsoft.Practices.Unity;

namespace UIComposition
{
    internal class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return this.Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

#if SILVERLIGHT
            Application.Current.RootVisual = (UIElement)this.Shell;
#else
            App.Current.MainWindow = (Window)this.Shell;
            App.Current.MainWindow.Show();
#endif
        }

        protected override void InitializeModules()
        {
            IModule employeeModule = this.Container.Resolve<EmployeeModule>();
            employeeModule.Initialize();

            IModule projectModule = this.Container.Resolve<ProjectModule>();
            projectModule.Initialize();
        }
    }
}