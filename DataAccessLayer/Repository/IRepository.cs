using System.Linq.Expressions;

namespace DataAccessLayer.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IQueryable<T> GetAllAsQueryable();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindAsQueryable(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        void SaveChanges();
    }
}
