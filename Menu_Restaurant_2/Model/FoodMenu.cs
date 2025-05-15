using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu_Restaurant_2.Model
{
    public class FoodMenu
    {
        [Key]
        public int Id { get; set; }

        public int FoodId { get; set; }
        public int MenuId { get; set; }

        public virtual Food Food { get; set; }
        public virtual Menu Menu { get; set; }
    }

}
