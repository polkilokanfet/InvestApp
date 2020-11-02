using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InvestApp.Core.Mvvm;

namespace InvestApp.Modules.ModuleName.Menus
{
    public partial class ModuleMenu : IOutlookBarGroup
    {
        public ModuleMenu(ModuleMenuViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            XamDataTree.Loaded += XamDataTreeOnLoaded;
        }

        private void XamDataTreeOnLoaded(object sender, RoutedEventArgs e)
        {
            XamDataTree.Loaded -= XamDataTreeOnLoaded;
            if (XamDataTree.Nodes.Any())
            {
                XamDataTree.ActiveNode = XamDataTree.Nodes.First();
                XamDataTree.Nodes.First().IsSelected = true;

                //var parentNode = XamDataTree.Nodes[0];
                //var nodeToSelect = parentNode.Nodes[0];
                //nodeToSelect.IsSelected = true;
            }
        }

        public Uri DefaultViewUri
        {
            get
            {
                if (XamDataTree.Nodes.Any() && !XamDataTree.SelectionSettings.SelectedNodes.Any())
                    XamDataTree.SelectionSettings.SelectedNodes.Add(XamDataTree.Nodes[0]);

                var node = XamDataTree.SelectionSettings.SelectedNodes[0];
                var navigationItem = node?.Data as INavigationItem;
                return navigationItem?.NavigationUri;
            }
        }

    }
}
