
using StoryDates.cs.Repository;
using System.Linq.Expressions;

namespace StoryDates.cs.ResultOperations
{
    public interface IBaseRepository<TEntities> where TEntities : class
    {
        Task<SucefullyResult> Add(TEntities entity);
        Task<SucefullyResult> Update(TEntities entity);
        Task<SucefullyResult> GetAll();
        Task<SucefullyResult> GetById(int id);   
        Task<bool> Exist(Expression<Func<TEntities, bool>> predicate);
    }
}
