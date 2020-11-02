using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dal;

namespace Bll
{
    public class BaseRepository<T>:List<T> where T : class
    {
        //private ShopWayEntities _context;
        //private DbSet<T> _set;


        //public BaseRepository()
        //{
        //    SetContext();
        //}

        //private void SetContext()
        //{
        //    _context = new ShopWayEntities();
        //    _set = _context.Set<T>();
        //}


        //public T GetById(int? id)
        //{
        //    if (id != null)
        //        return _set.Find(id);
        //    else
        //        return null;
        //}

        //public IQueryable<T> All()
        //{
        //    return _set;
        //}

        //public IQueryable<T> All(string include)
        //{
        //    return _set.Include(include);
        //}

        //public void Insert(T o)
        //{
        //    _set.Add(o);
        //}

        //public void Delete(T o)
        //{
        //    _set.Remove(o);
        //}

        //public void Save()
        //{
        //    try
        //    {
        //        _context.SaveChanges();
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        foreach (var eve in e.EntityValidationErrors)
        //        {



        //            foreach (var ve in eve.ValidationErrors)
        //            {

        //            }

        //        }
        //        RollBack();
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        RollBack();
        //        throw;
        //    }
        //}


        //public void RollBack()
        //{
        //    var context = _context;
        //    var changedEntries = context.ChangeTracker.Entries().Where(x => x.State != System.Data.Entity.EntityState.Unchanged).ToList();

        //    foreach (var entry in changedEntries.Where(x => x.State == System.Data.Entity.EntityState.Modified))
        //    {
        //        entry.CurrentValues.SetValues(entry.OriginalValues);
        //        entry.State = System.Data.Entity.EntityState.Unchanged;

        //    }

        //    foreach (var entry in changedEntries.Where(x => x.State == System.Data.Entity.EntityState.Added))
        //    {
        //        entry.State = System.Data.Entity.EntityState.Detached;
        //    }

        //    foreach (var entry in changedEntries.Where(x => x.State == System.Data.Entity.EntityState.Deleted))
        //    {
        //        entry.State = System.Data.Entity.EntityState.Unchanged;
        //    }

        //}
        //public ShopWayEntities Query()
        //{
        //    return _context;
        //}


        //public IEnumerable<T> SearchFor(Expression<Func<T, bool>> predicate)
        //{
        //    return _set.Where(predicate);
        //}

        //public DbSqlQuery<T> Sql(string sql, params object[] parameters)
        //{
        //    return _set.SqlQuery(sql, parameters);
        //}
    }

}
