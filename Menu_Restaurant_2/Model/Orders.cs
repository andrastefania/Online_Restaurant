using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Menu_Restaurant_2.Model
{
    public class Orders
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }  // Nullable, la fel ca în SQL
        public virtual Person Customer { get; set; }

        public DateTime? Date { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        //[Column(TypeName = "decimal(10, 2)")]
        public decimal? FoodPrice { get; set; }

        //[Column(TypeName = "decimal(10, 2)")]
        public decimal? TransportPrice { get; set; }

       // [Column(TypeName = "decimal(10, 2)")]
        public decimal? TotalPrice { get; set; }

        public DateTime? EstimatedTime { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(50)]
        public string OrderNumber { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
    }
}
