using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Task.Models
{
    public class CustomerProduct
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}