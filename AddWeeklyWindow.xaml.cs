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
    /// Interaction logic for AddWeeklyWindow.xaml
    /// </summary>
    public partial class AddWeeklyWindow : Window
    {
        DatabaseFunctions db = new DatabaseFunctions();
        public MainWindow main;
        public decimal result;
        public RadioButton radio;

        public AddWeeklyWindow(MainWindow main)
        {
            this.main = main;
            InitializeComponent();
        }

        private void AddItemClick(object sender, RoutedEventArgs e)
        {
            if (ValidInputs())
            {
                int Month = main.CurrentMonth;
                int WeekNum = GetRadioValue(radio);
                string ItemName = ItemNameTextBox.Text;
                decimal Price = Math.Round(result, 2);

                string expense = $"{ItemName} - ₱{Price}";

                db.InsertWeeklyExpense(Month, WeekNum, ItemName, Price);
                main.AddToExpenseData(WeekNum, expense);

                Close();
            }

        }

        private bool ValidInputs()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(ItemNameTextBox.Text))
            {
                ItemNameTextBox.Focus();
                MessageBox.Show("Please enter an Item", "Invalid Input");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(PriceTextBox.Text) || !decimal.TryParse(PriceTextBox.Text, out result))
            {
                PriceTextBox.Focus();
                MessageBox.Show("Please enter a Price", "Invalid Input");
                isValid = false;
            }

            radio = GetCheckedRadio();
            if (radio == null)
            {
                MessageBox.Show("Please select a Week", "Invalid Input");
                isValid = false;
            }

            return isValid;
        }

        private RadioButton GetCheckedRadio()
        {
            RadioButton[] radios = { Week1Radio, Week2Radio, Week3Radio };

            RadioButton checked_radio = null;

            foreach(RadioButton radio in radios)
            {
                if ((bool)radio.IsChecked)
                {
                    checked_radio = radio;
                    break;
                }
            }

            return checked_radio;
        }

        private int GetRadioValue(RadioButton radio)
        {
            switch (radio.Name)
            {
                case "Week1Radio":
                    return 1;

                case "Week2Radio":
                    return 2;

                case "Week3Radio":
                    return 3;

                default:
                    return -1;
            }

        }
    }
}
