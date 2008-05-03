//===============================================================================
// Microsoft patterns & practices
// Composite WPF (PRISM)
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================

namespace UIComposition.Modules.Employee
{
    using System;
    using System.Windows;
    using Microsoft.Practices.Unity;
    using Prism.Interfaces;
    using Prism.Utility;
    using UIComposition.Infrastructure;
    using UIComposition.Modules.Project;
    using UIComposition.Modules.Employee.Controllers;

    public class EmployeesPresenter
    {
        private IEmployeesListPresenter listPresenter;
        private IEmployeesController employeeController;

        public EmployeesPresenter(
            IEmployeesView view, 
            IEmployeesListPresenter listPresenter,
            IEmployeesController employeeController)
        {
            this.View = view;
            this.listPresenter = listPresenter;
            this.listPresenter.EmployeeSelected += new EventHandler<DataEventArgs<BusinessEntities.Employee>>(this.OnEmployeeSelected);
            this.employeeController = employeeController;

            View.SetHeader(listPresenter.View);
        }

        public IEmployeesView View { get; set; }

        private void OnEmployeeSelected(object sender, DataEventArgs<BusinessEntities.Employee> e)
        {
            employeeController.OnEmployeeSelected(View, e.Value);
        }
    }
}