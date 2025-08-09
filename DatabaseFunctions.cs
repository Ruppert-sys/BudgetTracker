using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker
{
    class DatabaseFunctions
    {
        public string ConnString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Users\josem\Desktop\BudgetTracker\BudgetTracker\BT_Database.mdf';Integrated Security=True";

        public void InsertWeeklyExpense(int month, int week, string item, decimal price)
        {
            using (var conn = new SqlConnection(ConnString))
            {
                conn.Open();
                var query = "INSERT INTO WeeklyExpenses VALUES (@month, @week, @item, @price)";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@month", month);
                    cmd.Parameters.AddWithValue("@week", week);
                    cmd.Parameters.AddWithValue("@item", item);
                    cmd.Parameters.AddWithValue("@price", price);

                    cmd.ExecuteNonQuery();
                }

            }
        }

        public void InsertMonthlyExpense(int month, string TypeOfBill, decimal price)
        {
            using (var conn = new SqlConnection(ConnString))
            {
                conn.Open();
                var query = "INSERT INTO MonthlyExpenses VALUES (@month, @bill, @price)";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@month", month);
                    cmd.Parameters.AddWithValue("@bill", TypeOfBill);
                    cmd.Parameters.AddWithValue("@price", price);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<object> GetWeeklyExpensesData(int Month, int WeekNum, string ColumnName)
        {
            var WeeklyData = new List<object>();

            using (var conn = new SqlConnection(ConnString))
            {
                conn.Open();
                var query = $"SELECT {ColumnName} FROM WeeklyExpenses WHERE Month=@month AND Week=@WeekNum";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@month", Month);
                    cmd.Parameters.AddWithValue("@WeekNum", WeekNum);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var instance = reader.GetValue(0);
                            WeeklyData.Add(instance);
                        }
                    }
                }
            }

            return WeeklyData;
        }

        public List<object> GetMonthlyExpensesData(int Month, string ColumnName)
        {
            var MonthlyData = new List<object>();

            using (var conn = new SqlConnection(ConnString))
            {
                conn.Open();
                var query = $"SELECT {ColumnName} FROM MonthlyExpenses WHERE Month=@month";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@month", Month);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var instance = reader.GetValue(0);
                            MonthlyData.Add(instance);
                        }
                    }
                }
            }

            return MonthlyData;
        }

        public void DeleteExpenseData(int Month, string Item, decimal Price)
        {
            using (var conn =  new SqlConnection(ConnString))
            {
                conn.Open();
                var query = "DELETE FROM WeeklyExpenses WHERE Month=@month AND Item=@item AND Price=@price";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@month", Month);
                    cmd.Parameters.AddWithValue("@item", Item);
                    cmd.Parameters.AddWithValue("@price", Price);

                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE FROM MonthlyExpenses WHERE Month=@month AND Item=@item AND Price=@price";

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public ExpensesBreakdown GetExpensesBreakdown(int Month)
        {
            ExpensesBreakdown breakdown = new ExpensesBreakdown();

            using (var conn = new SqlConnection(ConnString))
            {
                conn.Open();
                var query = "SELECT SUM(Price) FROM WeeklyExpenses WHERE Month=@month";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@month", Month);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var instance = reader.IsDBNull(0) ? 0 : reader.GetDecimal(0);
                            breakdown.WeeklyTotal = instance;
                        }
                    }

                    cmd.CommandText = "SELECT SUM(PRICE) FROM MonthlyExpenses WHERE Month=@month";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var instance = reader.IsDBNull(0) ? 0 : reader.GetDecimal(0);
                            breakdown.MonthlyTotal = instance;
                        }
                    }

                    for (int i = 1; i < 4; i++)
                    {
                        cmd.CommandText = $"SELECT SUM(PRICE) FROM WeeklyExpenses WHERE Month=@month AND Week={i}";

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var instance = reader.IsDBNull(0) ? 0 : reader.GetDecimal(0);
                                breakdown.SetWeekTotal(i, instance);
                            }
                        }
                    }
                }
            }

            return breakdown;
        }

        public void UpdateUser(string Name, int Age, decimal TargetValue)
        {
            using(var conn = new SqlConnection(ConnString))
            {
                conn.Open();
                var query = "DELETE FROM [User]";

                using(var cmd = new SqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "INSERT INTO [User] VALUES (@name, @age, @targetval)";

                    cmd.Parameters.AddWithValue("@name", Name);
                    cmd.Parameters.AddWithValue("@age", Age);
                    cmd.Parameters.AddWithValue("@targetval", TargetValue);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public User GetUser()
        {
            User user = new User();

            using(var conn = new SqlConnection(ConnString))
            {
                conn.Open();
                var query = "Select * from [User]";

                using(var cmd = new SqlCommand(query, conn))
                {
                    using(var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.Name = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            user.Age = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                            user.TargetExpense = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3);
                            Debug.WriteLine("test");
                        }
                    }
                }
            }

            return user;
        }
    }
}
