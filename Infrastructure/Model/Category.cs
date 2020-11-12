using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManager.Infrastructure.Model
{
    public class Category
    {
        public long CategoryId { get; set; }
        public User CategoryOwner { get; set; }
        public string CategoryName { get; set; }
    }
}
