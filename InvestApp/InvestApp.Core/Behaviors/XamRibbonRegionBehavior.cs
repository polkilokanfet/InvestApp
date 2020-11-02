using System;
using System.Collections.Specialized;
using InvestApp.Core.Attributes;
using InvestApp.Core.Mvvm;
using Prism.Regions;
using InvestApp.Infrastructure.Extansions;

namespace InvestApp.Core.Behaviors
{
    public class XamRibbonRegionBehavior : RegionBehavior
    {
        /// <summary>
        /// The key of this behavior.
        /// </summary>
        public const string BehaviorKey = "XamRibbonRegionBehavior";

        protected override void OnAttach()
        {
            if (Region.Name == RegionNames.ContentRegion)
                Region.ActiveViews.CollectionChanged += ActiveViewsOnCollectionChanged;
        }

        private void ActiveViewsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                bool isFirst = true;
                foreach (var newView in e.NewItems)
                {
                    var view = newView as IViewBase;
                    if (view == null) continue; //имеем ли дело с нужным типом?

                    if (view.RibbonTabs.Count > 0) continue; //возможно список уже сформирован

                    foreach (var ribbonTabAttribute in (newView.GetType()).GetCustomAttributes<RibbonTabAttribute>())
                    {
                        var ribbonTabItem = (IRibbonTabItem)Activator.CreateInstance(ribbonTabAttribute.RibbonTabType);
                        ribbonTabItem.ViewModel = view.ViewModel;

                        view.RibbonTabs.Add(ribbonTabItem);
                        ribbonTabItem.IsSelected = isFirst;

                        if (isFirst) isFirst = false;
                    }

                }
            }
        }
    }
}