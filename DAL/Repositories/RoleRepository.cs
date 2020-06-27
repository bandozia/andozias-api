using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace api.DAL.Repositories
{
    public class RoleRepository : BaseRepository<Role>
    {
        public RoleRepository(CoreContext context) : base(context)
        {
        }

        public async Task<IList<Role>> GetByNamesAsync(string[] names)
        {
            return await DbSet.Where(r => names.Contains(r.Name)).ToListAsync();
        }

    }
}
