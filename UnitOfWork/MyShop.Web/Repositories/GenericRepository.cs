using MyShop.Infrastructure;
using System.Linq.Expressions;

namespace MyShop.Web.Repositories
{
    public abstract class GenericRepository<T> :
        IRepository<T> where T : class
    {
        protected ShoppingContext context;

        public GenericRepository(ShoppingContext context)
        {
            this.context = context;
        }

        //To override use Virtual. public virtual T Add(T entity)
        public T Add(T entity)
        {
            return context.Add(entity).Entity;
        }

        public IEnumerable<T> All()
        {
            return context.Set<T>().ToList();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>()
                .AsQueryable()
                .Where(predicate)
                .ToList();
        }

        public virtual T Get(Guid id)
        {
            return context.Find<T>(id);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public virtual T Update(T entity)
        {
            return context.Update(entity).Entity;
        }
    }
}
