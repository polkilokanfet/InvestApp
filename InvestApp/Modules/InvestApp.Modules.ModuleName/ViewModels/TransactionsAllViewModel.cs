using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using InvestApp.Domain.Models;
using InvestApp.Domain.Services.DataBaseAccess;
using Prism.Mvvm;

namespace InvestApp.Modules.ModuleName.ViewModels
{
    public class TransactionsAllViewModel : BindableBase
    {
        private readonly ObservableCollection<Transaction> _transactions = new ObservableCollection<Transaction>();
        public IEnumerable<Transaction> Transactions => _transactions;

        public Transaction ActiveTransaction { get; set; }

        public TransactionsAllViewModel(IUnitOfWork unitOfWork)
        {
            _transactions = new ObservableCollection<Transaction>(unitOfWork.Repository<Transaction>().GetAll().OrderByDescending(transaction => transaction.Date));
        }
    }
}