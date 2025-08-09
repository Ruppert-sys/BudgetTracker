using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BudgetTracker
{
    /// <summary>
    /// Interaction logic for LoadBudgetSummaryWindow.xaml
    /// </summary>
    public partial class LoadBudgetSummaryWindow : Window
    {
        MainWindow main;
        private int month;
        public LoadBudgetSummaryWindow(MainWindow main)
        {
            this.main = main;
            InitializeComponent();
        }

        private void LoadSummaryClick(object sender, RoutedEventArgs e)
        {
            if (ValidInputs())
            {
                main.CurrentMonth = month;
                main.UpdateExpenseDataGrid();
                Close();
            }
        }

        private bool ValidInputs()
        {
            bool isValid = true;

            month = 0;
            int.TryParse(MonthTextBox.Text, out month);

            if (month <= 0 || month >= 13)
            {
                MonthTextBox.Focus();
                MessageBox.Show("Please enter a valid month", "Invalid input");
                isValid = false;
            }

            return isValid;
        }
    }
}
