using System.ComponentModel.DataAnnotations;

namespace api.DAL.Models
{
    public class Invite : CoreModel
    {
        public User Sender { get; set; }

        [MaxLength(150)]
        public string ReceiverEmail { get; set; }

        public Group Group { get; set; }
    }
}
