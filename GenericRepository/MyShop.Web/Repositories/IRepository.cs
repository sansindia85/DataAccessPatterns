using System.Linq.Expressions;

namespace MyShop.Web.Repositories
{
    public interface IRepository<T>
    {
        T Add(T entity);
        T Update(T entity);
        T Get(Guid id);
        IEnumerable<T> All();

        //Find(Customer => customer.Age > 20)
        IEnumerable<T> Find(Expression<Func<T,bool>> predicate);

        void SaveChanges();

    }
}
