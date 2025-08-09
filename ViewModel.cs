using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker
{
    internal class ViewModel
    {
        public ObservableCollection<Expense> ExpenseItems { get; set; }
    }
}
