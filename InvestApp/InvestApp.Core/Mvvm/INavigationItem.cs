using System;

namespace InvestApp.Core.Mvvm
{
    public interface INavigationItem
    {
        Uri NavigationUri { get; }
    }
}