using System.Collections.Generic;
using api.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace api.DAL
{
    public class CoreContext : DbContext
    {
        public CoreContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(user =>
            {
                user.HasKey(u => u.Id);
                user.HasIndex(u => u.Email).IsUnique();
            });

            modelBuilder.Entity<Role>(role =>
            {
                role.HasKey(r => r.Id);
                role.HasIndex(r => r.Name).IsUnique();
                role.HasData(new List<Role>
                {
                    new Role {Id = 1, Name = "group_m", Description = "Administrar o grupo"},
                    new Role {Id = 2, Name = "finan_w", Description = "Criar e editar entradas financeiras"},
                    new Role {Id = 3, Name = "finan_r", Description = "Ler entradas financeiras"},
                    new Role {Id = 4, Name = "finan_a", Description = "Auditar entradas financeiras de outros membros"},
                    new Role {Id = 5, Name = "msg", Description = "Ler e criar mensagens para o grupo"}
                });
            });

            modelBuilder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
                userRole.HasOne(ur => ur.User)
                    .WithMany(u => u.Roles)
                    .HasForeignKey(ur => ur.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(ur => ur.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Group>(group =>
            {
                group.HasKey(g => g.Id);
                group.HasMany(g => g.Users).WithOne(u => u.Group).OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Invite>(invite =>
            {
                invite.HasKey(i => i.Id);

                invite.HasOne(i => i.Sender)
                    .WithMany(u => u.Invites)
                    .OnDelete(DeleteBehavior.Cascade);

                invite.HasOne(i => i.Group)
                    .WithMany(g => g.Invites)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
