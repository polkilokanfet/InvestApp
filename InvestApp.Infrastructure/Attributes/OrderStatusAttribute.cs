using System;

namespace InvestApp.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OrderStatusAttribute : Attribute
    {
        public int OrderStatus { get; }

        public OrderStatusAttribute(int orderStatus)
        {
            OrderStatus = orderStatus;
        }
    }
}