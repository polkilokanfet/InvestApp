using System;
using InvestApp.Domain.Models.Base;

namespace InvestApp.Domain.Models
{
    public class FinancialRatio : BaseEntity
    {
        //https://financialmodelingprep.com/developer/docs/formula/

        public string Symbol { get; set; }
        public DateTime Date { get; set; }

        public decimal? CurrentRatio { get; set; }                       
        public decimal? QuickRatio { get; set; }                         
        public decimal? CashRatio { get; set; }                          
        public decimal? DaysOfSalesOutstanding { get; set; }             
        public decimal? DaysOfInventoryOutstanding { get; set; }         
        public decimal? OperatingCycle { get; set; }                     
        public decimal? DaysOfPayablesOutstanding { get; set; }          
        public decimal? CashConversionCycle { get; set; }                
        public decimal? GrossProfitMargin { get; set; }                  
        public decimal? OperatingProfitMargin { get; set; }              
        public decimal? PretaxProfitMargin { get; set; }                 
        public decimal? NetProfitMargin { get; set; }                    
        public decimal? EffectiveTaxRate { get; set; }                   
        public decimal? ReturnOnAssets { get; set; }                     
        public decimal? ReturnOnEquity { get; set; }                     
        public decimal? ReturnOnCapitalEmployed { get; set; }            
        public decimal? NetIncomePerEBT { get; set; }                    
        public decimal? EbtPerEbit { get; set; }                         
        public decimal? EbitPerRevenue { get; set; }                     
        public decimal? DebtRatio { get; set; }                          
        public decimal? DebtEquityRatio { get; set; }                    
        public decimal? LongTermDebtToCapitalization { get; set; }       
        public decimal? TotalDebtToCapitalization { get; set; }          
        public decimal? InterestCoverage { get; set; }                   
        public decimal? CashFlowToDebtRatio { get; set; }                
        public decimal? CompanyEquityMultiplier { get; set; }            
        public decimal? ReceivablesTurnover { get; set; }                
        public decimal? PayablesTurnover { get; set; }                   
        public decimal? InventoryTurnover { get; set; }                  
        public decimal? FixedAssetTurnover { get; set; }                 
        public decimal? AssetTurnover { get; set; }                      
        public decimal? OperatingCashFlowPerShare { get; set; }          
        public decimal? FreeCashFlowPerShare { get; set; }               
        public decimal? CashPerShare { get; set; }                       
        public decimal? PayoutRatio { get; set; }                        
        public decimal? OperatingCashFlowSalesRatio { get; set; }        
        public decimal? FreeCashFlowOperatingCashFlowRatio { get; set; } 
        public decimal? CashFlowCoverageRatios { get; set; }             
        public decimal? ShortTermCoverageRatios { get; set; }            
        public decimal? CapitalExpenditureCoverageRatio { get; set; }    
        public decimal? DividendPaidAndCapexCoverageRatio { get; set; }  
        public decimal? DividendPayoutRatio { get; set; }                
        public decimal? PriceBookValueRatio { get; set; }                
        public decimal? PriceToBookRatio { get; set; }                   
        public decimal? PriceToSalesRatio { get; set; }                  
        public decimal? PriceEarningsRatio { get; set; }                 
        public decimal? PriceToFreeCashFlowsRatio { get; set; }          
        public decimal? PriceToOperatingCashFlowsRatio { get; set; }     
        public decimal? PriceCashFlowRatio { get; set; }                 
        public decimal? PriceEarningsToGrowthRatio { get; set; }         
        public decimal? PriceSalesRatio { get; set; }                    
        public decimal? DividendYield { get; set; }                      
        public decimal? EnterpriseValueMultiple { get; set; }            
        public decimal? PriceFairValue { get; set; }                     
    }
}