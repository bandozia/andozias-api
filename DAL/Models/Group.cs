using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace api.DAL.Models
{
    public class Group : CoreModel
    {
        [MaxLength(50)]
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<User> Users { get; set; }

        [JsonIgnore]
        public ICollection<Invite> Invites { get; set; }

    }

}
