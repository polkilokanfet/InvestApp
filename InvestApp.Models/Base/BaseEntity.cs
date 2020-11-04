using System;
using InvestApp.Models.Interfaces;

namespace InvestApp.Models.Base
{
    public abstract class BaseEntity : IBaseEntity, IComparable
    {
        public Guid Id { get; set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        public override bool Equals(object otherObject)
        {
            if (!(otherObject is BaseEntity other)) return false;
            return Equals(this.Id, other.Id) || base.Equals(otherObject);
        }

        protected bool Equals(BaseEntity other)
        {
            return Id.Equals(other.Id);
        }

        public virtual int CompareTo(object other)
        {
            return string.Compare(this.ToString(), other.ToString(), StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}