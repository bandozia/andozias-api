using System;
using System.Threading.Tasks;
using api.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.DAL.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(CoreContext context) : base(context)
        {
        }

        public override async Task<User> GetByIdAsync(int id)
        {
            return await DbSet
                .Include(u => u.Group)
                .SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await DbSet
                .Include(u => u.Roles).ThenInclude(r => r.Role)
                .Include(u => u.Group)
                .SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> CheckEmailExists(string email)
        {
            User user = await DbSet.SingleOrDefaultAsync(u => u.Email == email);
            return user != null;
        }

        public async Task UpdateGroupAsync(User user, Group group)
        {
            user.Group = group;
            await Context.SaveChangesAsync();
        }

    }
}
