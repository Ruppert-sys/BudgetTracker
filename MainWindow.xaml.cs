using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace BudgetTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel ViewModel;
        DatabaseFunctions db = new DatabaseFunctions();

        public ShowTipsWindow ShowTipsWindow = null;
        public BudgetSummaryWindow BudgetSummaryWindow = null;
        public AddWeeklyWindow AddWeeklyWindow = null;
        public AddMonthlyWindow AddMonthlyWindow = null;
        public LoadBudgetSummaryWindow LoadBudgetSummaryWindow = null;
        public TransportationFeesWindow TransportationFeesWindow = null;
        public int CurrentMonth = DateTime.Now.Month;
        public decimal TotalExpenses;
        public decimal TargetValue;
        public int age;
        public MainWindow()
        {
            InitializeComponent();

            this.ViewModel = new ViewModel
            {
                ExpenseItems = new ObservableCollection<Expense>()
            };
            this.DataContext = this.ViewModel;

            InitUser();
            UpdateExpenseDataGrid();
            
        }

        private void InitUser()
        {
            User user = db.GetUser();
            if (string.IsNullOrWhiteSpace(user.Name)) { return; }

            NameTextBox.Text = user.Name;
            AgeTextBox.Text = user.Age.ToString();
            TargetTextBox.Text = user.TargetExpense.ToString();

        }

        private void ShowTipsButton(object sender, RoutedEventArgs e)
        {
            if (ShowTipsWindow == null)
            {
                ShowTipsWindow = new ShowTipsWindow();
                ShowTipsWindow.Closed += ShowTipsWindowClosed;
                ShowTipsWindow.Show();
            }
            ShowTipsWindow.Activate();
        }

        private void ShowTipsWindowClosed(object sender, EventArgs e)
        {
            ShowTipsWindow.Close();
            ShowTipsWindow = null;
        }

        private void CalculatorButton(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("Calc");
        }

        private void BudgetSummaryButton(object sender, RoutedEventArgs e)
        {
            if(BudgetSummaryWindow == null)
            {
                BudgetSummaryWindow = new BudgetSummaryWindow(this);
                BudgetSummaryWindow.Closed += BudgetSummaryWindowClosed;
                BudgetSummaryWindow.Show();
            }
            BudgetSummaryWindow.Activate();
        }

        private void BudgetSummaryWindowClosed(object sender, EventArgs e)
        {
            BudgetSummaryWindow = null;
        }

        private void MainClosed(object sender, EventArgs e)
        {
            Close();
            System.Environment.Exit(0);
        }

        private void AddWeeklyExpenseButton(object sender, RoutedEventArgs e)
        {
            if (AddWeeklyWindow == null)
            {
                AddWeeklyWindow = new AddWeeklyWindow(this);
                AddWeeklyWindow.Closed += AddWeeklyWindowClosed;
                AddWeeklyWindow.Show();
            }
            AddWeeklyWindow.Activate();
        }

        private void AddWeeklyWindowClosed(object sender, EventArgs e)
        {
            AddWeeklyWindow = null;
        }

        private void AddMonthlyWindowButton(object sender, RoutedEventArgs e)
        {
            if (AddMonthlyWindow == null)
            {
                AddMonthlyWindow = new AddMonthlyWindow(this);
                AddMonthlyWindow.Closed += AddMonthlyWindowClosed;
                AddMonthlyWindow.Show();
            }
            AddMonthlyWindow.Activate();
        }

        private void AddMonthlyWindowClosed(object sender, EventArgs e)
        {
            AddMonthlyWindow.Close();
            AddMonthlyWindow = null;
        }

        private void LoadBudgetSummaryClick(object sender, RoutedEventArgs e)
        {
            if (LoadBudgetSummaryWindow == null)
            {
                LoadBudgetSummaryWindow = new LoadBudgetSummaryWindow(this);
                LoadBudgetSummaryWindow.Closed += LoadBudgetSummaryWindowClosed;
                LoadBudgetSummaryWindow.Show();
            }
            LoadBudgetSummaryWindow.Activate();
        }

        private void LoadBudgetSummaryWindowClosed(object sender, EventArgs e)
        {
            LoadBudgetSummaryWindow = null;
        }

        public void AddToExpenseData(int column, string expense)
        {
            bool NewRow = true;
            string PropertyName = "";

            switch (column)
            {
                case 1:
                    PropertyName = "Week1";
                    break;

                case 2:
                    PropertyName = "Week2";
                    break;

                case 3:
                    PropertyName = "Week3";
                    break;

                case 4:
                    PropertyName = "Monthly";
                    break;
            }

            foreach (Expense item in ViewModel.ExpenseItems)
            {
                var property = item.GetType().GetProperty(PropertyName);

                object value = property.GetValue(item);
                if (value == null)
                {
                    property.SetValue(item, expense);
                    NewRow = false;
                }
            }

            if (NewRow)
            {
                Expense item = new Expense();
                var property = item.GetType().GetProperty(PropertyName);
                property.SetValue(item, expense);
                ViewModel.ExpenseItems.Add(item);
            }

            UpdateExpenseDataGrid();
        }

        public void UpdateExpenseDataGrid()
        {
            ViewModel.ExpenseItems.Clear();
            ExpenseDataGrid.Items.Refresh();

            List<object> Week1Items = db.GetWeeklyExpensesData(CurrentMonth, WeekNum: 1, ColumnName: "Item");
            List<object> Week1Price = db.GetWeeklyExpensesData(CurrentMonth, WeekNum: 1, ColumnName: "Price");

            List<object> Week2Items = db.GetWeeklyExpensesData(CurrentMonth, WeekNum: 2, ColumnName: "Item");
            List<object> Week2Price = db.GetWeeklyExpensesData(CurrentMonth, WeekNum: 2, ColumnName: "Price");

            List<object> Week3Items = db.GetWeeklyExpensesData(CurrentMonth, WeekNum: 3, ColumnName: "Item");
            List<object> Week3Price = db.GetWeeklyExpensesData(CurrentMonth, WeekNum: 3, ColumnName: "Price");

            List<object> MonthlyItems = db.GetMonthlyExpensesData(CurrentMonth, ColumnName: "Item");
            List<object> MonthlyPrice = db.GetMonthlyExpensesData(CurrentMonth, ColumnName: "Price");

            int[] items = { Week1Items.Count, Week2Items.Count, Week3Items.Count, MonthlyItems.Count};
            int max_len = items.Max();

            for (int i = 0; i < max_len; i++)
            {
                Expense expense = new Expense();
                bool test = i < max_len;
                if (Week1Items.Count > i)
                {
                    expense.Week1 = $"{Week1Items[i]} - ₱{Week1Price[i]}";
                }
                
                if(Week2Items.Count > i)
                {
                    expense.Week2 = $"{Week2Items[i]} - ₱{Week2Price[i]}";
                }

                if(Week3Items.Count > i)
                {
                    expense.Week3 = $"{Week3Items[i]} - ₱{Week3Price[i]}";
                }

                if(MonthlyItems.Count > i)
                {
                    expense.Monthly = $"{MonthlyItems[i]} - ₱{MonthlyPrice[i]}";
                }

                ViewModel.ExpenseItems.Add(expense);
            }

            ExpenseDataGrid.Items.Refresh();

            //refresh monthly target and current expense

            ExpensesBreakdown breakdown = db.GetExpensesBreakdown(CurrentMonth);
            TotalExpenses = breakdown.MonthlyTotal + breakdown.WeeklyTotal;

            UpdateTracker();
        }
        private void TargetTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!decimal.TryParse(TargetTextBox.Text, out TargetValue) || TargetTextBox.Text == "0")
            {
                return;
            }

            TargetTrackerLabel.Content = $"{TotalExpenses}/{TargetValue}";

            UpdateTracker();

        }

        public void UpdateTracker()
        {
            if (TargetValue == 0) { return; }

            decimal percentage = TotalExpenses / TargetValue;
            double Offset = Convert.ToDouble(percentage.ToString("0.###"));
            Color Color = Colors.Green;

            if (percentage > 1) { Offset = 1; }

            if (Offset >= 0.5) { Color = Colors.Yellow; }
            if (Offset >= 0.8) { Color = Colors.Red; }

            LinearGradientBrush gradient = new LinearGradientBrush();
            gradient.StartPoint = new Point(0, 0);
            gradient.EndPoint = new Point(1, 0);

            GradientStop CurrentBarStart = new GradientStop();
            CurrentBarStart.Color = Color;
            CurrentBarStart.Offset = 0.0;
            gradient.GradientStops.Add(CurrentBarStart);

            GradientStop CurrentBarEnd = new GradientStop();
            CurrentBarEnd.Color = Color;
            CurrentBarEnd.Offset = Offset;
            gradient.GradientStops.Add(CurrentBarEnd);

            GradientStop TotalBarStart = new GradientStop();
            TotalBarStart.Color = Colors.White;
            TotalBarStart.Offset = Offset;
            gradient.GradientStops.Add(TotalBarStart);

            GradientStop TotalBarEnd = new GradientStop();
            TotalBarEnd.Color = Colors.White;
            TotalBarEnd.Offset = 1.0;
            gradient.GradientStops.Add(TotalBarEnd);

            TargetTrackerRect.Fill = gradient;

            TargetTrackerLabel.Content = $"{TotalExpenses}/{TargetValue}";
        }

        private void RemoveExpenseClick(object sender, RoutedEventArgs e)
        {
            DataGridCellInfo cell = ExpenseDataGrid.SelectedCells.FirstOrDefault();
            if (cell.Item == null)
            {
                return;
            }
            var data_to_delete = (cell.Column.GetCellContent(cell.Item) as TextBlock).Text;

            if (String.IsNullOrWhiteSpace(data_to_delete)) { return; }

            var expense_split = data_to_delete.Split(new string[] { " - ₱" }, StringSplitOptions.None);
            string item_name = expense_split[0].Trim();
            decimal price = decimal.Parse(expense_split[1].Trim());

            db.DeleteExpenseData(CurrentMonth, item_name, price);

            UpdateExpenseDataGrid();
        }

        private void SaveBudgetSummaryClick(object sender, RoutedEventArgs e)
        {
            if (ValidInputs())
            {
                string name = NameTextBox.Text;
                db.UpdateUser(name, age, TargetValue);
                MessageBox.Show("Budget Summary Saved!", "Save Budget Summary");
            }
        }

        private bool ValidInputs()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                NameTextBox.Focus();
                MessageBox.Show("Please enter a valid name", "Invalid Input");
                isValid = false;
            }
            bool isValidAge = int.TryParse(AgeTextBox.Text, out age);
            if (string.IsNullOrWhiteSpace(AgeTextBox.Text) || !isValidAge || age <= 0)
            {
                AgeTextBox.Focus();
                MessageBox.Show("Please enter a valid age", "Invalid Input");
                isValid = false;
            }

            if(!decimal.TryParse(TargetTextBox.Text, out TargetValue))
            {
                TargetTextBox.Focus();
                MessageBox.Show("Please enter a valid Target Value", "Invalid Input");
                isValid = false;
            }

            return isValid;
        }

        private void TransportFeesClick(object sender, RoutedEventArgs e)
        {
            if (TransportationFeesWindow == null)
            {
                TransportationFeesWindow = new TransportationFeesWindow(this);
                TransportationFeesWindow.Closed += TransportationFeesClosed;
                TransportationFeesWindow.Show();
            }
            TransportationFeesWindow.Activate();
        }

        private void TransportationFeesClosed(object sender, EventArgs e)
        {
            TransportationFeesWindow.Close();
            TransportationFeesWindow = null;
        }
    }
}
