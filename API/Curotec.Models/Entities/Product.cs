using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Curotec.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}