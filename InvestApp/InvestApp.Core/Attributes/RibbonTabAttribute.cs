using System;

namespace InvestApp.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RibbonTabAttribute : DependentViewAttribute
    {
        public RibbonTabAttribute(Type ribbonTabType) : base(RegionNames.RibbonTabRegion, ribbonTabType)
        {
        }
    }
}