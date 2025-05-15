using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Menu_Restaurant_2.Model
{
    public class Menu:ICartItem
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

        [Required]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        [StringLength(1000)]
        public string Info { get; set; }

        public virtual ICollection<FoodMenu> FoodMenus { get; set; }
    }
}
