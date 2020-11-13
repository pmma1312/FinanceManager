using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FinanceManager.Infrastructure.Model
{
    public class User
    {
        [Key]
        public long UserId { get; set; }
        public string Username { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
