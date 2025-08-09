using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker
{
    public class ExpensesBreakdown
    {
        public decimal Week1Total { get; set; }
        public decimal Week2Total { get; set; }
        public decimal Week3Total { get; set; }
        public decimal MonthlyTotal { get; set; }
        public decimal WeeklyTotal { get; set; }

        public void SetWeekTotal(int WeekNum, decimal value)
        {
            switch (WeekNum)
            {
                case 1:
                    Week1Total = value;
                    break;
                case 2:
                    Week2Total = value;
                    break;
                case 3:
                    Week3Total = value;
                    break;
            }
        }
    }
}
