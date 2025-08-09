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
    /// Interaction logic for AddMonthlyWindow.xaml
    /// </summary>
    public partial class AddMonthlyWindow : Window
    {
        DatabaseFunctions db = new DatabaseFunctions();
        public MainWindow main;
        public decimal result;
        public RadioButton radio;

        public AddMonthlyWindow(MainWindow main)
        {
            this.main = main;
            InitializeComponent();
        }

        private void AddItemClick(object sender, RoutedEventArgs e)
        {
            if (ValidInputs())
            {
                int Month = main.CurrentMonth;
                string MonthBill = GetRadioValue(radio);
                decimal Price = Math.Round(result, 2);

                string expense = $"{MonthBill} - ₱{Price}";
 
                db.InsertMonthlyExpense(Month, MonthBill, Price);
                main.AddToExpenseData(4, expense);

                Close();
            }
        }

        private bool ValidInputs()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(PriceTextBox.Text) || !decimal.TryParse(PriceTextBox.Text, out result))
            {
                PriceTextBox.Focus();
                MessageBox.Show("Please enter a Price", "Invalid Input");
                isValid = false;
            }

            radio = GetCheckedRadio();
            if (radio == null)
            {
                MessageBox.Show("Please select a Bill", "Invalid Input");
                isValid = false;
            }

            return isValid;
        }

        private RadioButton GetCheckedRadio()
        {
            RadioButton[] radios = { ElectricBillRadio, WaterBillRadio, InternetBillRadio };

            RadioButton checked_radio = null;

            foreach (RadioButton radio in radios)
            {
                if ((bool)radio.IsChecked)
                {
                    checked_radio = radio;
                    break;
                }
            }

            return checked_radio;
        }

        private string GetRadioValue(RadioButton radio)
        {
            switch (radio.Name)
            {
                case "ElectricBillRadio":
                    return "Electric Bill";

                case "WaterBillRadio":
                    return "Water Bill";

                case "InternetBillRadio":
                    return "Internet Bill";

                default:
                    return "";
            }

        }
    }
}
