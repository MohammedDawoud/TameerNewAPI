using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IRepository<T>  
    {
        //T Add(T entity);
        //IEnumerable<T> AddRange(IEnumerable<T> entities);
       // bool Exists(int Id);
        IEnumerable<T> GetAll();
        T GetById(int Id);
        IEnumerable<T> GetMatching(Func<T, bool> where);
        //void Remove(T entity);
        //void Remove(int Id);
        //void RemoveMatching(Func<T, bool> where);
        //void RemoveRange(IEnumerable<T> entities);
        //void Update(T entityToUpdate);
        //IQueryable<T> Queryable();

    }
}
