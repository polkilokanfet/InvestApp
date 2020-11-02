using System;
using System.Collections.ObjectModel;

namespace InvestApp.Core.Mvvm
{
    public class NavigationItem : INavigationItem
    {
        public string Caption { get; }
        public bool IsExpended { get; set; } = true;
        public Uri NavigationUri { get; }
        public ObservableCollection<NavigationItem> Items { get; } = new ObservableCollection<NavigationItem>();

        /// <summary>
        /// Навигационный айтем
        /// </summary>
        /// <param name="caption">Название</param>
        /// <param name="viewType">Вид</param>
        public NavigationItem(string caption, Type viewType)
        {
            if (string.IsNullOrEmpty(caption)) 
                throw new ArgumentNullException(nameof(caption));

            if (viewType == null)
                throw new ArgumentNullException(nameof(viewType));

            Caption = caption;
            NavigationUri = new Uri(viewType.Name, UriKind.Relative);
        }
    }
}