using Microsoft.AspNetCore.Mvc;
using MyShop.Domain.Models;
using MyShop.Web.Models;
using MyShop.Web.Repositories;
using System.Diagnostics;

namespace MyShop.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;        

        public OrderController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;            
        }

        public IActionResult Index()
        {
            var orders = unitOfWork.OrderRepository
                         .Find(order => order.OrderDate > DateTime.UtcNow.AddDays(-1));

            return View(orders);
        }

        public IActionResult Create()
        {
            var products = unitOfWork.ProductRepository.All();

            return View(products);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderModel model)
        {
            #region ModelValidation
            
            if (!model.LineItems.Any()) return BadRequest("Please submit line items");

            if (string.IsNullOrWhiteSpace(model.Customer.Name)) return BadRequest("Customer needs a name");

            #endregion

            var customer = unitOfWork.CustomerRepository.Find(c => c.Name == model.Customer.Name).FirstOrDefault(); ;
            if (customer != null)
            {
                customer.ShippingAddress = model.Customer.ShippingAddress;
                customer.City = model.Customer.City;
                customer.Country = model.Customer.Country;

                unitOfWork.CustomerRepository.Update(customer);
                unitOfWork.CustomerRepository.SaveChanges();
            }
            else
            {
                customer = new Customer
                {
                    Name = model.Customer.Name,
                    ShippingAddress = model.Customer.ShippingAddress,
                    City = model.Customer.City,
                    PostalCode = model.Customer.PostalCode,
                    Country = model.Customer.Country
                };
            }
            

            var order = new Order
            {
                LineItems = model.LineItems
                    .Select(line => new LineItem { ProductId = line.ProductId, Quantity = line.Quantity })
                    .ToList(),

                Customer = customer
            };

            unitOfWork.OrderRepository.Add(order);

            unitOfWork.OrderRepository.SaveChanges();

            return Ok("Order Created");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
