using System.Threading.Tasks;
using api.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace api.DAL.Repositories
{
    public class BaseRepository<T> where T : CoreModel
    {
        protected readonly CoreContext Context;
        protected readonly DbSet<T> DbSet;

        public BaseRepository(CoreContext context)
        {
            Context = context;
            this.DbSet = Context.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await DbSet.SingleOrDefaultAsync(u => u.Id == id);
        }

        public virtual async Task<T> InsertAsync(T item)
        {
            DbSet.Add(item);
            await Context.SaveChangesAsync();
            return item;
        }
    }
}
