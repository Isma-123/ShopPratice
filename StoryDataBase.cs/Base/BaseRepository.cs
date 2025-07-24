using Microsoft.EntityFrameworkCore; // ✅ correcto
using StoryDataBase.cs.Context;
using StoryDates.cs.Repository;
using StoryDates.cs.ResultOperations;
using System.Linq.Expressions;


namespace StoryDataBase.cs.Base
{
    public abstract class BaseRepository<TEntities> :
        IBaseRepository<TEntities>
        where TEntities : class

    {
        private readonly StoryContext _storyContext;
        private DbSet<TEntities> _entities;

        public BaseRepository(StoryContext storyContext)
        {
            _storyContext = storyContext;
            _entities = _storyContext.Set<TEntities>();
        }

        public virtual async Task<SucefullyResult> Add(TEntities entity)
        {
            SucefullyResult result = new SucefullyResult();



            try
            {
                await _entities.AddAsync(entity);   
                await _storyContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                result.status = false;
                result.message = $"erro try to save the enttites type: {ex.Message}";

            }
            return result;  

        }

        public virtual async Task<bool> Exist(Expression<Func<TEntities, bool>> predicate)
        {
            return await _entities.AnyAsync(predicate); 
        }

        public virtual async Task<SucefullyResult> GetAll()
        {
            SucefullyResult result = new SucefullyResult();


            try
            {
                await _entities.ToListAsync();
                result.message = "the List of entities has been sucefully disclose on the system!";

            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"erro try to list the enttites type: {ex.Message}";
            }


            return result;
        }

        public virtual async Task<SucefullyResult> GetById(int id)
        {

            SucefullyResult result = new SucefullyResult();

            try {


                await _entities.FindAsync(id);
                result.message = "Entitie has been found on the system";
            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"error getting the enttites type: {ex.Message}";
            }


            return result;
        }

        public virtual async Task<SucefullyResult> Update(TEntities entity)
        {
            SucefullyResult result = new SucefullyResult();

            try
            {
                _entities.Update(entity);
                await _storyContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"error updating the enttites type: {ex.Message}";
            }


            return result;
        }
    }
}
