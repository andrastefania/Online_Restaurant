using System;
using System.ComponentModel.DataAnnotations;

namespace Menu_Restaurant_2.Model
{
    public class Food:ICartItem
    {
        [Key]  
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Portion_Size { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        public int Stock { get; set; }

        [Required]
        public decimal Price { get; set; }

        [StringLength(255)]
        public string Info { get; set; }

        [StringLength(255)]
        public string Image { get; set; }


    }
}
