using System;
using CSharpToSqlSalesDbLibrary;

namespace CSharpToSqlSalesDb
{
    public class TestSalesLibrary
    {
        static void Main(string[] args)
        {
            var connection = new Connection("localhost\\sqlexpress01", "SalesDb");

            var octrl = new OrdersController(connection);
            var newO = new Order()
            {
                
                Date = DateTime.Now,
                Description = "Testing"
            };
            var newOC = octrl.CreateOrderForCustomer(newO, "Kroger");

           // var orderCtrl = new OrdersController(connection);
           // //var orders = orderCtrl.GetAll();
           // //var order = orderCtrl.GetByPK(2);
           // //var success = orderCtrl.Create(newO);
           // var delo = orderCtrl.GetByPK(27);
           // var success = orderCtrl.Remove(delo);


           // var olCtrl = new OrderLinesController(connection);
           // //var orderlines = olCtrl.GetAll();
           // //var orderline = olCtrl.GetByPK(3);
           // //var newOL = new OrderLine()
           // //{
           // //    OrdersId = 3,
           // //    Product = "Test",
           // //    Description = "test",
           // //    Quantity = 2,
           // //    Price = 57.42m
           // //};
           // //var olsuccess = olCtrl.Create(newOL);
           // var delol = olCtrl.GetByPK(82);
           // var olsuccess = olCtrl.Remove(delol);

           //var custCtrl = new CustomersController(connection);

           // //var customers = custCtrl.GetAll();
           // //var customer = custCtrl.GetByPK(4);
           // //var newC = new Customer()
           // //{
           // //    Name = "Tester",
           // //    City = "England",
           // //    State = "AL",
           // //    Active = true
           // //};
           // //var csuccess = custCtrl.Create(newC);
           // var delc = custCtrl.GetByPK(37);
           // var csuccess = custCtrl.Remove(delc);

            connection.Disconnect();
            var stopper = true;
        }

    }
}
