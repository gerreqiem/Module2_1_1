using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
namespace CarRentalApp
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Server=GK216_9\\SQLEXPRESS;Database=CarRentalDB;Trusted_Connection=True;";

        public MainWindow()
        {
            InitializeComponent();
        }
        private void LoadRentals_Click(object sender, RoutedEventArgs e)
        {
            LoadRentals();
        }

        private void LoadRentals()
        {
            List<Rental> rentals = new List<Rental>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        Rentals.RentalID,
                        Customers.Name AS CustomerName,
                        Cars.Make AS CarMake,
                        Rentals.RentalDate,
                        Rentals.ReturnDate,
                        Rentals.TotalCost
                    FROM Rentals
                    JOIN Customers ON Rentals.CustomerID = Customers.CustomerID
                    JOIN Cars ON Rentals.CarID = Cars.CarID";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    rentals.Add(new Rental
                    {
                        RentalID = reader.GetInt32(0),
                        CustomerName = reader.GetString(1),
                        CarMake = reader.GetString(2),
                        RentalDate = reader.GetDateTime(3),
                        ReturnDate = reader.GetDateTime(4),
                        TotalCost = reader.GetDecimal(5)
                    });
                }
            }
            dataGrid.ItemsSource = rentals;
        }
    }
    public class Rental
    {
        public int RentalID { get; set; }
        public string CustomerName { get; set; }
        public string CarMake { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalCost { get; set; }
    }
}
