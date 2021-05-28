using System;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

namespace CSharpToSqlSalesDbLibrary
{

    public class Connection
    {
        public SqlConnection Sqlconn { get; set; }

        public Connection(string server, string database)
        {
            var connStr = $"server={server};" +
            $"database={database};" +
            "trusted_connection=true;";

            Sqlconn = new SqlConnection(connStr);
            Sqlconn.Open();
            if (Sqlconn.State != System.Data.ConnectionState.Open)
            {
                Sqlconn = null;
                throw new Exception("Connection did not open!");
            }
            Console.WriteLine("Open connection successful!");
        }
        public void Disconnect()
        {
            if (Sqlconn == null)
            {
                return;
            }
            Sqlconn.Close();
            Sqlconn = null;
        }

    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public decimal Sales { get; set; }
        public bool Active { get; set; }
    }

    public class CustomersController
    {
        private static Connection connection { get; set; }

        public CustomersController(Connection connection)
        {
            CustomersController.connection = connection;
        }

        //fillbyreader
        private Customer FillCustomerByReader(SqlDataReader reader)
        {
            var customer = new Customer
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = Convert.ToString(reader["Name"]),
                City = Convert.ToString(reader["City"]),
                State = Convert.ToString(reader["State"]),
                Sales = Convert.ToDecimal(reader["Sales"]),
                Active = Convert.ToBoolean(reader["Active"])
            };
            return customer;
        }

        //fillbyparam
        private void FillCustomerByParameter(SqlCommand cmd, Customer customer)
        {
            cmd.Parameters.AddWithValue("@name", customer.Name);
            cmd.Parameters.AddWithValue("@city", customer.City);
            cmd.Parameters.AddWithValue("@state", customer.State);
            cmd.Parameters.AddWithValue("@sales", customer.Sales);
            cmd.Parameters.AddWithValue("@active", customer.Active);
        }

        //selectall
        public List<Customer> GetAll()
        {
            var sql = "SELECT * From Customers;";
            var cmd = new SqlCommand(sql, CustomersController.connection.Sqlconn);
            var reader = cmd.ExecuteReader();
            var customers = new List<Customer>();

            while (reader.Read())
            {
                var customer = FillCustomerByReader(reader);
                customers.Add(customer);
            }
            reader.Close();
            return customers;
        }
        //selectbypk
        public Customer GetByPK(int id)
        {
            var sql = $"SELECT * From Customers where Id = {id};";
            var cmd = new SqlCommand(sql, CustomersController.connection.Sqlconn);
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                return null;
            }
            reader.Read();
            var customer = FillCustomerByReader(reader);
            reader.Close();
            return customer;
        }
        //selectbyname
        public Customer GetByName(string name)
        {
            var sql = $"SELECT * From Customers where Name = @name;";
            var cmd = new SqlCommand(sql, CustomersController.connection.Sqlconn);
            cmd.Parameters.AddWithValue("@name", name);
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                return null;
            }
            reader.Read();
            var customer = FillCustomerByReader(reader);
            reader.Close();
            return customer;
        }
        //create
        public bool Create(Customer customer)
        {
            var sql = "INSERT into Customers " +
                        "VALUES " +
                        "(@name, @city, @state, @sales, @active);";
            var cmd = new SqlCommand(sql, CustomersController.connection.Sqlconn);
            FillCustomerByParameter(cmd, customer);
            var rowsAffected = cmd.ExecuteNonQuery();
            return (rowsAffected == 1);
        }
        //update
        public bool Change(Customer customer)
        {
            var sql = "UPDATE Customers set " +
                        "Name = @name, " +
                        "City = @city, " +
                        "State = @state, " +
                        "Sales = @sales, " +
                        "Active = @active " +
                        "Where Id = @id;";
            var cmd = new SqlCommand(sql, CustomersController.connection.Sqlconn);
            FillCustomerByParameter(cmd, customer);
            cmd.Parameters.AddWithValue("@id", customer.Id);
            var rowsAffected = cmd.ExecuteNonQuery();
            return (rowsAffected == 1);
        }
        //delete
        public bool Remove(Customer customer)
        {
            var sql = "DELETE from Customers where Id = @id";
            var cmd = new SqlCommand(sql, CustomersController.connection.Sqlconn);
            cmd.Parameters.AddWithValue("@id", customer.Id);
            var rowsAffected = cmd.ExecuteNonQuery();
            return (rowsAffected == 1);
        }
    }
    public class OrderLine
    {
        public int Id { get; set; }
        public int OrdersId { get; set; }
        public string Product { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderLinesController
    {
        private static Connection connection { get; set; }

        public OrderLinesController(Connection connection)
        {
            OrderLinesController.connection = connection;
        }

        //fillbyreader
        private OrderLine FillOrderLineByReader(SqlDataReader reader)
        {
            var orderline = new OrderLine
            {
                Id = Convert.ToInt32(reader["Id"]),
                OrdersId = Convert.ToInt32(reader["OrdersId"]),
                Product = Convert.ToString(reader["Product"]),
                Description = Convert.ToString(reader["Description"]),
                Quantity = Convert.ToInt32(reader["Quantity"]),
                Price = Convert.ToDecimal(reader["Price"])
            };
            return orderline;
        }

        //fillbyparam
        private void FillOrderLineByParameter(SqlCommand cmd, OrderLine orderline)
        {
            cmd.Parameters.AddWithValue("@ordersid", orderline.OrdersId);
            cmd.Parameters.AddWithValue("@product", orderline.Product);
            cmd.Parameters.AddWithValue("@state", orderline.Description);
            cmd.Parameters.AddWithValue("@quantity", orderline.Quantity);
            cmd.Parameters.AddWithValue("@price", orderline.Price);
        }

        //selectall
        public List<OrderLine> GetAll()
        {
            var sql = "SELECT * From OrderLines;";
            var cmd = new SqlCommand(sql, OrderLinesController.connection.Sqlconn);
            var reader = cmd.ExecuteReader();
            var orderlines = new List<OrderLine>();

            while (reader.Read())
            {
                var orderline = FillOrderLineByReader(reader);
                orderlines.Add(orderline);
            }
            reader.Close();
            return orderlines;
        }


        //selectbypk
        public OrderLine GetByPK(int id)
        {
            var sql = $"SELECT * From OrderLines where Id = {id};";
            var cmd = new SqlCommand(sql, OrderLinesController.connection.Sqlconn);
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                return null;
            }
            reader.Read();
            var orderline = FillOrderLineByReader(reader);
            reader.Close();
            return orderline;
        }
        //create
        public bool Create(OrderLine orderline)
        {
            var sql = "INSERT into OrderLines " +
                        "VALUES " +
                        "(@ordersid, @product, @state, @quantity, @price);";
            var cmd = new SqlCommand(sql, OrderLinesController.connection.Sqlconn);
            FillOrderLineByParameter(cmd, orderline);
            var rowsAffected = cmd.ExecuteNonQuery();
            return (rowsAffected == 1);
        }
        //update
        public bool Change(OrderLine orderline)
        {
            var sql = "UPDATE OrderLines set " +
                        "OrdersId = @ordersid, " +
                        "Product = @product, " +
                        "Description = @state, " +
                        "Quantity = @quantity, " +
                        "Price = @price " +
                        "Where Id = @id;";
            var cmd = new SqlCommand(sql, OrderLinesController.connection.Sqlconn);
            FillOrderLineByParameter(cmd, orderline);
            cmd.Parameters.AddWithValue("@id", orderline.Id);
            var rowsAffected = cmd.ExecuteNonQuery();
            return (rowsAffected == 1);
        }
        //delete
        public bool Remove(OrderLine orderline)
        {
            var sql = "DELETE from OrderLines where Id = @id";
            var cmd = new SqlCommand(sql, OrderLinesController.connection.Sqlconn);
            cmd.Parameters.AddWithValue("@id", orderline.Id);
            var rowsAffected = cmd.ExecuteNonQuery();
            return (rowsAffected == 1);
        }
    }
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public Customer Customer { get; set; }
    }

    public class OrdersController
    {
        private static Connection connection { get; set; }

        public OrdersController(Connection connection)
        {
            OrdersController.connection = connection;
        }

        //fillbyreader
        private Order FillOrderByReader(SqlDataReader reader)
        {
            var order = new Order
            {
                Id = Convert.ToInt32(reader["Id"]),
                CustomerId = Convert.ToInt32(reader["CustomerId"]),
                Date = Convert.ToDateTime(reader["Date"]),
                Description = Convert.ToString(reader["Description"])
            };
            return order;
        }

        //fillbyparam
        private void FillOrderByParameter(SqlCommand cmd, Order order)
        {
            cmd.Parameters.AddWithValue("@customerid", order.CustomerId);
            cmd.Parameters.AddWithValue("@date", order.Date);
            cmd.Parameters.AddWithValue("@description", order.Description);
        }

        //selectall
        public List<Order> GetAll()
        {
            var sql = "SELECT * From Orders;";
            var cmd = new SqlCommand(sql, OrdersController.connection.Sqlconn);
            var reader = cmd.ExecuteReader();
            var orders = new List<Order>();

            while (reader.Read())
            {
                var order = FillOrderByReader(reader);
                orders.Add(order);
            }
            reader.Close();
            return orders;
        }
        //selectbypk
        public Order GetByPK(int id)
        {
            var sql = $"SELECT * From Orders where Id = {id};";
            var cmd = new SqlCommand(sql, OrdersController.connection.Sqlconn);
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                return null;
            }
            reader.Read();
            var order = FillOrderByReader(reader);
            reader.Close();
            GetCustomerforOrders(order);
            return order;
        }

        //getcustomerfororder
        private void GetCustomerforOrders(Order order)
        {
            var custCtrl = new CustomersController(connection);
            order.Customer = custCtrl.GetByPK(order.CustomerId);
        }

        //create
        public bool Create(Order order)
        {
            var sql = "INSERT into Orders " +
                        "VALUES " +
                        "(@customerid, @date, @description);";
            var cmd = new SqlCommand(sql, OrdersController.connection.Sqlconn);
            FillOrderByParameter(cmd, order);
            var rowsAffected = cmd.ExecuteNonQuery();
            return (rowsAffected == 1);
        }
        //createforcustomer
        public bool CreateOrderForCustomer(Order order, string CustomerName)
        {
            var custCtrl = new CustomersController(connection);
            var customer = custCtrl.GetByName(CustomerName);
            order.CustomerId = customer.Id;
            return Create(order);
        }

        //update
        public bool Change(Order order)
        {
            var sql = "UPDATE Orders set " +
                        "CustomerId = @customerid, " +
                        "Date = @date, " +
                        "Description = @description, " +
                        "Where Id = @id;";
            var cmd = new SqlCommand(sql, OrdersController.connection.Sqlconn);
            FillOrderByParameter(cmd, order);
            cmd.Parameters.AddWithValue("@id", order.Id);
            var rowsAffected = cmd.ExecuteNonQuery();
            return (rowsAffected == 1);
        }

        //delete
        public bool Remove(Order order)
        {
            var sql = "DELETE from Orders where Id = @id";
            var cmd = new SqlCommand(sql, OrdersController.connection.Sqlconn);
            cmd.Parameters.AddWithValue("@id", order.Id);
            var rowsAffected = cmd.ExecuteNonQuery();
            return (rowsAffected == 1);
        }
    }

}