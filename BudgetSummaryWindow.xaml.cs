using System;
using System.Diagnostics;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;


namespace BudgetTracker
{
    /// <summary>
    /// Interaction logic for BudgetSummaryWindow.xaml
    /// </summary>
    public partial class BudgetSummaryWindow : Window
    {
        public MainWindow main;
        public ExpensesBreakdown Breakdown;
        DatabaseFunctions db = new DatabaseFunctions();
        public BudgetSummaryWindow(MainWindow main)
        {
            this.main = main;

            Breakdown = db.GetExpensesBreakdown(main.CurrentMonth);
            InitializeComponent();
            PiechartData.Series.Add(new PieSeries { Title = "Week 1 Total", Values = new ChartValues<decimal> { Breakdown.Week1Total } });
            PiechartData.Series.Add(new PieSeries { Title = "Week 2 Total", Values = new ChartValues<decimal> { Breakdown.Week2Total } });
            PiechartData.Series.Add(new PieSeries { Title = "Week 3 Total", Values = new ChartValues<decimal> { Breakdown.Week3Total } });

            TotalMonthlyExpenses.Content = $"₱{Breakdown.MonthlyTotal}";
            TotalExpenses.Content = $"₱{Breakdown.MonthlyTotal + Breakdown.WeeklyTotal}";
            TargetExpenses.Content = $"₱{main.TargetValue}";
            decimal net = main.TargetValue - main.TotalExpenses;
            NetProfit.Content = net > 0 ? $"+ ₱{Math.Abs(net)}" : $"- ₱{Math.Abs(net)}";
        }

    }
}
