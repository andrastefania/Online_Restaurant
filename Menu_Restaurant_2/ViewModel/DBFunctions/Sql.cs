using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using Menu_Restaurant_2.Model;

namespace Menu_Restaurant_2.ViewModel.DBFunctions
{
    public static class Sql
    {
        private static readonly string connectionString = "Data Source=0903-ANDRA\\SQLEXPRESS;Initial Catalog=RestaurantMenuDB;Integrated Security=True";

        public static void AddFood(string name, decimal price, string category, string info, string portionSize, string image, bool isMenu, int stock)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("AddFood", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@Info", info ?? "");
                cmd.Parameters.AddWithValue("@PortionSize", portionSize);
                cmd.Parameters.AddWithValue("@Image", image ?? "");
                cmd.Parameters.AddWithValue("@IsMenu", isMenu ? 1 : 0);
                cmd.Parameters.AddWithValue("@Stock", stock);

                cmd.ExecuteNonQuery();
            }
        }

        public static void AddMenu(string name, string category, string portionSize, decimal price, int stock, string image, string info, List<Food> selectedFood)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var insertMenu = new SqlCommand(@"
                    INSERT INTO Menu (name, category, portion_size, price, stock, image, info) 
                    OUTPUT INSERTED.id 
                    VALUES (@name, @category, @portion, @price, @stock, @image, @info)", conn);

                insertMenu.Parameters.AddWithValue("@name", name);
                insertMenu.Parameters.AddWithValue("@category", category);
                insertMenu.Parameters.AddWithValue("@portion", portionSize);
                insertMenu.Parameters.AddWithValue("@price", price);
                insertMenu.Parameters.AddWithValue("@stock", stock);
                insertMenu.Parameters.AddWithValue("@image", image ?? "");
                insertMenu.Parameters.AddWithValue("@info", info ?? "");

                int newMenuId = (int)insertMenu.ExecuteScalar();

                foreach (var food in selectedFood)
                {
                    var linkCmd = new SqlCommand("INSERT INTO FoodMenu (food_id, menu_id) VALUES (@food, @menu)", conn);
                    linkCmd.Parameters.AddWithValue("@food", food.Id);
                    linkCmd.Parameters.AddWithValue("@menu", newMenuId);
                    linkCmd.ExecuteNonQuery();
                }
            }
        }
        public static ObservableCollection<Orders> GetActiveOrders()
        {
            var orders = new ObservableCollection<Orders>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT id, order_number, status FROM Orders WHERE status NOT IN ('canceled')", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new Orders
                        {
                            Id = (int)reader["id"],
                            OrderNumber = reader["order_number"].ToString(),
                            Status = reader["status"].ToString()
                        });
                    }
                }
            }
            return orders;
        }

        public static void UpdateOrderStatus(int orderId, string newStatus)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("UPDATE Orders SET status = @Status WHERE id = @Id", conn);
                cmd.Parameters.AddWithValue("@Status", newStatus);
                cmd.Parameters.AddWithValue("@Id", orderId);
                cmd.ExecuteNonQuery();
            }
        }
        public static ObservableCollection<Food> GetAllDishes()
        {
            var dishes = new ObservableCollection<Food>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT Name FROM Food", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dishes.Add(new Food
                        {
                            Name = reader["Name"].ToString()
                        });
                    }
                }
            }
            return dishes;
        }
        public static void DeleteDishByName(string name)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("DeleteFoodByName", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.ExecuteNonQuery();
            }
        }
        public static void DeleteMenuById(int menuId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM Menu WHERE id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", menuId);
                cmd.ExecuteNonQuery();
            }
        }
        public static ObservableCollection<Menu> GetMenus()
        {
            var menus = new ObservableCollection<Menu>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT id, name FROM Menu", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        menus.Add(new Menu
                        {
                            Id = (int)reader["id"],
                            Name = reader["name"].ToString()
                        });
                    }
                }
            }
            return menus;
        }
        public static List<Orders> LoadOrders()
        {
            var ordersList = new List<Orders>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT order_number, status, date, total_price FROM Orders WHERE status NOT IN ('delivered', 'canceled')", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ordersList.Add(new Orders
                        {
                            OrderNumber = reader["order_number"].ToString(),
                            Status = reader["status"].ToString(),
                            Date = (DateTime)reader["date"],
                            TotalPrice = (decimal)reader["total_price"]
                        });
                    }
                }
            }

            return ordersList;
        }
        public static List<OrderItem> GenerateOrderReport(DateTime startDate, DateTime endDate)
        {
            var orders = new List<OrderItem>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand("GenerateOrderReport", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new OrderItem
                    {
                        Id = (int)reader["id"],
                        CustomerName = reader["CustomerName"].ToString(),
                        Date = (DateTime)reader["date"],
                        TotalPrice = (decimal)reader["total_price"],
                        Status = reader["status"].ToString()
                    });
                }
            }
            return orders; 
        }

        public static void AddOrder(int customerId, string name, string phone, string address, decimal foodPrice, decimal transportPrice, decimal totalPrice, string orderNumber, DateTime estimatedTime)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand("AddOrder", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@FoodPrice", foodPrice);
                cmd.Parameters.AddWithValue("@TransportPrice", transportPrice);
                cmd.Parameters.AddWithValue("@TotalPrice", totalPrice);
                cmd.Parameters.AddWithValue("@OrderNumber", orderNumber);
                cmd.Parameters.AddWithValue("@EstimatedTime", estimatedTime);

                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateStock(int foodId, int menuId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                if (foodId > 0)
                {
                    // Actualizare stoc pentru produsul Food
                    using (var cmd = new SqlCommand("UPDATE Food SET Stock = Stock - 1 WHERE Id = @Id", conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", foodId);
                        cmd.ExecuteNonQuery();
                    }
                }
                else if (menuId > 0)
                {
                    // Actualizare stoc pentru produsul Menu
                    using (var cmd = new SqlCommand("UPDATE Menu SET Stock = Stock - 1 WHERE Id = @Id", conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", menuId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static string GenerateUniqueOrderNumber()
        {
            string orderNumber;
            bool isUnique = false;

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                do
                {
                    orderNumber = "ORD" + new Random().Next(1000, 999999);
                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Orders WHERE order_number = @OrderNumber", conn))
                    {
                        cmd.Parameters.AddWithValue("@OrderNumber", orderNumber);
                        int count = (int)cmd.ExecuteScalar();
                        isUnique = count == 0;
                    }

                } while (!isUnique);
            }

            return orderNumber;
        }
        public static ObservableCollection<StockItem> GetStockReport(int threshold)
        {
            var stockItems = new ObservableCollection<StockItem>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand("GetStockReport", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Threshold", threshold);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    stockItems.Add(new StockItem
                    {
                        Name = reader["Name"].ToString(),
                        Stock = (int)reader["Stock"],
                        StockStatus = reader["StockStatus"].ToString()
                    });
                }
            }

            return stockItems;
        }
        public static ObservableCollection<Food> GetAllDishesWithDetails()
        {
            var dishes = new ObservableCollection<Food>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT Id, Name, Price, Portion_Size, Info FROM Food", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dishes.Add(new Food
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Price = (decimal)reader["Price"],
                        Portion_Size = reader["Portion_Size"].ToString(),
                        Info = reader["Info"]?.ToString()
                    });
                }
            }
            return dishes;
        }
        public static void UpdateFood(int id, decimal price, string portionSize, string info)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("UpdateFood", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@PortionSize", portionSize);
                cmd.Parameters.AddWithValue("@Info", info ?? "");
                cmd.ExecuteNonQuery();
            }
        }
        public static ObservableCollection<Food> GetAllFood()
        {
            var foodList = new ObservableCollection<Food>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Food", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    foodList.Add(new Food
                    {
                        Id = (int)reader["id"],
                        Name = reader["name"].ToString(),
                        Portion_Size = reader["portion_size"].ToString(),
                        Category = reader["category"].ToString(),
                        Price = (decimal)reader["price"],
                        Stock = (int)reader["stock"]
                    });
                }
            }
            return foodList;
        }
        public static ObservableCollection<Food> GetFoodsForMenu(int menuId)
        {
            var selected = new ObservableCollection<Food>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "SELECT f.* FROM Food f INNER JOIN FoodMenu fm ON f.id = fm.food_id WHERE fm.menu_id = @id", conn);
                cmd.Parameters.AddWithValue("@id", menuId);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    selected.Add(new Food
                    {
                        Id = (int)reader["id"],
                        Name = reader["name"].ToString(),
                        Portion_Size = reader["portion_size"].ToString(),
                        Category = reader["category"].ToString(),
                        Price = (decimal)reader["price"],
                        Stock = (int)reader["stock"]
                    });
                }
            }
            return selected;
        }
        public static void UpdateMenu(int menuId, string name, string portionSize, decimal price, int stock, List<int> foodIds)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var updateCmd = new SqlCommand(@"
            UPDATE Menu SET 
                name = @name,
                portion_size = @portion,
                price = @price,
                stock = @stock
            WHERE id = @id", conn);

                updateCmd.Parameters.AddWithValue("@name", name);
                updateCmd.Parameters.AddWithValue("@portion", portionSize);
                updateCmd.Parameters.AddWithValue("@price", price);
                updateCmd.Parameters.AddWithValue("@stock", stock);
                updateCmd.Parameters.AddWithValue("@id", menuId);
                updateCmd.ExecuteNonQuery();

                var deleteCmd = new SqlCommand("DELETE FROM FoodMenu WHERE menu_id = @id", conn);
                deleteCmd.Parameters.AddWithValue("@id", menuId);
                deleteCmd.ExecuteNonQuery();

                foreach (var foodId in foodIds)
                {
                    var linkCmd = new SqlCommand("INSERT INTO FoodMenu (food_id, menu_id) VALUES (@food, @menu)", conn);
                    linkCmd.Parameters.AddWithValue("@food", foodId);
                    linkCmd.Parameters.AddWithValue("@menu", menuId);
                    linkCmd.ExecuteNonQuery();
                }
            }
        }
        public static List<Orders> GetOrdersForCustomer(int customerId)
        {
            var orders = new List<Orders>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT order_number, status, date, total_price FROM Orders WHERE customer_id = @CustomerId", conn);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new Orders
                    {
                        OrderNumber = reader["order_number"].ToString(),
                        Status = reader["status"].ToString(),
                        Date = (DateTime)reader["date"],
                        TotalPrice = (decimal)reader["total_price"]
                    });
                }
            }

            return orders;
        }
        public static bool CancelOrder(string orderNumber, int customerId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Verifică dacă aparține utilizatorului
                var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Orders WHERE order_number = @OrderNumber AND customer_id = @CustomerId", conn);
                checkCmd.Parameters.AddWithValue("@OrderNumber", orderNumber);
                checkCmd.Parameters.AddWithValue("@CustomerId", customerId);

                int count = (int)checkCmd.ExecuteScalar();
                if (count == 0)
                    return false;

                // Schimbă statusul
                var updateCmd = new SqlCommand("UPDATE Orders SET status = 'canceled' WHERE order_number = @OrderNumber AND customer_id = @CustomerId", conn);
                updateCmd.Parameters.AddWithValue("@OrderNumber", orderNumber);
                updateCmd.Parameters.AddWithValue("@CustomerId", customerId);
                updateCmd.ExecuteNonQuery();

                return true;
            }
        }


    }


}
