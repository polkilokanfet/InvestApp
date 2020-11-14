using System;
using System.Linq;
using InvestApp.Domain.Models;
using InvestApp.Domain.Wrappers.Base;
using InvestApp.Domain.Wrappers.Base.TrackingCollections;

//namespace InvestApp.Models.Wrappers.Base
//{

//    public partial class MoneyAmountWrapper : WrapperBase<MoneyAmount>
//	{
//	    public MoneyAmountWrapper(MoneyAmount model) : base(model) { }

//        #region SimpleProperties

//        //Currency
//        public Currency Currency
//        {
//          get { return GetValue<Currency>(); }
//          set { SetValue(value); }
//        }
//        public Currency CurrencyOriginalValue => GetOriginalValue<Currency>(nameof(Currency));
//        public bool CurrencyIsChanged => GetIsChanged(nameof(Currency));

//        //Value
//        public System.Decimal Value
//        {
//          get { return GetValue<System.Decimal>(); }
//          set { SetValue(value); }
//        }
//        public System.Decimal ValueOriginalValue => GetOriginalValue<System.Decimal>(nameof(Value));
//        public bool ValueIsChanged => GetIsChanged(nameof(Value));

//        //Id
//        public System.Guid Id
//        {
//          get { return GetValue<System.Guid>(); }
//          set { SetValue(value); }
//        }
//        public System.Guid IdOriginalValue => GetOriginalValue<System.Guid>(nameof(Id));
//        public bool IdIsChanged => GetIsChanged(nameof(Id));

//        #endregion
//	}

	
//    public partial class OperationWrapper : WrapperBase<Transaction>
//	{
//	    public OperationWrapper(Transaction model) : base(model) { }

//        #region SimpleProperties

//        //IdTcs
//        public System.String IdTcs
//        {
//          get { return GetValue<System.String>(); }
//          set { SetValue(value); }
//        }
//        public System.String IdTcsOriginalValue => GetOriginalValue<System.String>(nameof(IdTcs));
//        public bool IdTcsIsChanged => GetIsChanged(nameof(IdTcs));

//        //Status
//        public OperationStatus Status
//        {
//          get { return GetValue<OperationStatus>(); }
//          set { SetValue(value); }
//        }
//        public OperationStatus StatusOriginalValue => GetOriginalValue<OperationStatus>(nameof(Status));
//        public bool StatusIsChanged => GetIsChanged(nameof(Status));

//        //Currency
//        public Currency Currency
//        {
//          get { return GetValue<Currency>(); }
//          set { SetValue(value); }
//        }
//        public Currency CurrencyOriginalValue => GetOriginalValue<Currency>(nameof(Currency));
//        public bool CurrencyIsChanged => GetIsChanged(nameof(Currency));

//        //Payment
//        public System.Decimal Payment
//        {
//          get { return GetValue<System.Decimal>(); }
//          set { SetValue(value); }
//        }
//        public System.Decimal PaymentOriginalValue => GetOriginalValue<System.Decimal>(nameof(Payment));
//        public bool PaymentIsChanged => GetIsChanged(nameof(Payment));

//        //Price
//        public System.Decimal Price
//        {
//          get { return GetValue<System.Decimal>(); }
//          set { SetValue(value); }
//        }
//        public System.Decimal PriceOriginalValue => GetOriginalValue<System.Decimal>(nameof(Price));
//        public bool PriceIsChanged => GetIsChanged(nameof(Price));

//        //Quantity
//        public System.Int32 Quantity
//        {
//          get { return GetValue<System.Int32>(); }
//          set { SetValue(value); }
//        }
//        public System.Int32 QuantityOriginalValue => GetOriginalValue<System.Int32>(nameof(Quantity));
//        public bool QuantityIsChanged => GetIsChanged(nameof(Quantity));

//        //Figi
//        public System.String Figi
//        {
//          get { return GetValue<System.String>(); }
//          set { SetValue(value); }
//        }
//        public System.String FigiOriginalValue => GetOriginalValue<System.String>(nameof(Figi));
//        public bool FigiIsChanged => GetIsChanged(nameof(Figi));

//        //InstrumentType
//        public InstrumentType InstrumentType
//        {
//          get { return GetValue<InstrumentType>(); }
//          set { SetValue(value); }
//        }
//        public InstrumentType InstrumentTypeOriginalValue => GetOriginalValue<InstrumentType>(nameof(InstrumentType));
//        public bool InstrumentTypeIsChanged => GetIsChanged(nameof(InstrumentType));

//        //IsMarginCall
//        public System.Boolean IsMarginCall
//        {
//          get { return GetValue<System.Boolean>(); }
//          set { SetValue(value); }
//        }
//        public System.Boolean IsMarginCallOriginalValue => GetOriginalValue<System.Boolean>(nameof(IsMarginCall));
//        public bool IsMarginCallIsChanged => GetIsChanged(nameof(IsMarginCall));

//        //Date
//        public System.DateTime Date
//        {
//          get { return GetValue<System.DateTime>(); }
//          set { SetValue(value); }
//        }
//        public System.DateTime DateOriginalValue => GetOriginalValue<System.DateTime>(nameof(Date));
//        public bool DateIsChanged => GetIsChanged(nameof(Date));

//        //OperationType
//        public ExtendedOperationType OperationType
//        {
//          get { return GetValue<ExtendedOperationType>(); }
//          set { SetValue(value); }
//        }
//        public ExtendedOperationType OperationTypeOriginalValue => GetOriginalValue<ExtendedOperationType>(nameof(OperationType));
//        public bool OperationTypeIsChanged => GetIsChanged(nameof(OperationType));

//        //Id
//        public System.Guid Id
//        {
//          get { return GetValue<System.Guid>(); }
//          set { SetValue(value); }
//        }
//        public System.Guid IdOriginalValue => GetOriginalValue<System.Guid>(nameof(Id));
//        public bool IdIsChanged => GetIsChanged(nameof(Id));

//        #endregion

//        #region ComplexProperties

//	    public MoneyAmountWrapper Commission 
//        {
//            get { return GetWrapper<MoneyAmountWrapper>(); }
//            set { SetComplexValue<MoneyAmount, MoneyAmountWrapper>(Commission, value); }
//        }

//        #endregion

//        #region CollectionProperties

//        #endregion

//        public override void InitializeComplexProperties()
//        {
//            InitializeComplexProperty<MoneyAmountWrapper>(nameof(Commission), Model.Commission == null ? null : new MoneyAmountWrapper(Model.Commission));
//        }
//        protected override void InitializeCollectionProperties()
//        {
//        }
//	}

	
	
//}
