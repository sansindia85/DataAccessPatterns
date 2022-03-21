using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Models;
using MyShop.Infrastructure;
using System.Linq.Expressions;

namespace MyShop.Web.Repositories
{
    public class OrderRepository : GenericRepository<Order>
    {        
        public OrderRepository(ShoppingContext context) : base(context)
        {
           
        }

        public override IEnumerable<Order> Find(Expression<Func<Order, bool>> predicate)
        {
            return context.Orders
                .Include(o => o.LineItems)
                .ThenInclude( l => l.Product)
                .Where(predicate)
                .ToList();
        }

        public override Order Update(Order entity)
        {
            var order = context.Orders
                .Include(o => o.LineItems)
                .ThenInclude(l => l.Product)
                .Single(o => o.OrderId == entity.OrderId);

            order.OrderDate = entity.OrderDate;
            order.LineItems = entity.LineItems;
            return base.Update(entity);

        }
    }
}
