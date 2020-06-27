using api.DAL.Models;

namespace api.DAL.Repositories
{
    public class GroupRepository : BaseRepository<Group>
    {
        public GroupRepository(CoreContext context) : base(context)
        {
        }
    }
}
