using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManager.Infrastructure.Model
{
    public class Category
    {
        [Key]
        public long CategoryId { get; set; }
        [ForeignKey("UserId")]
        public User CategoryOwner { get; set; }
        public string CategoryName { get; set; }
    }
}
