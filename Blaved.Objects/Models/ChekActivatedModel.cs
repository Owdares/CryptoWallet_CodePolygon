using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blaved.Models
{
    public class CheckActivatedModel
    {
        [Key]
        public long Id { get; set; }

        public long UserId { get; set; }

        [JsonIgnore]
        public UserModel User { get; set; }

        public long CheckId { get; set; }

        [JsonIgnore]
        public CheckModel Check { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
