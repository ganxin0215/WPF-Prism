//===============================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation
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

using System.Windows.Controls;

namespace StockTraderRI.Modules.Watch.WatchList
{
    /// <summary>
    /// Interaction logic for WatchListView.xaml
    /// </summary>
    public partial class WatchListView : UserControl, IWatchListView
    {
        public WatchListView()
        {
            InitializeComponent();
        }

        #region IWatchListView Members


        public WatchListPresentationModel Model
        {
            set { DataContext = value; }
        }

        #endregion
    }
}