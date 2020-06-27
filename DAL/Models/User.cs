using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace api.DAL.Models
{
    public class User : CoreModel
    {
        [EmailAddress] [MaxLength(150)] public string Email { get; set; }

        [MaxLength(64)] public string Password { get; set; }

        [MaxLength(200)] public string Name { get; set; }

        public Group Group { get; set; }

        public ICollection<UserRole> Roles { get; set; }
        public ICollection<Invite> Invites { get; set; }
    }

    public class Role : CoreModel
    {
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }

        public ICollection<UserRole> Users { get; set; }
    }

    public class UserRole
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
