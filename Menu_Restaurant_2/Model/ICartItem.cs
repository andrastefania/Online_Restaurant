using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu_Restaurant_2.Model
{
    public interface ICartItem
    {
        string Name { get; }
        string Portion_Size { get; }
        string Info { get; }
        string Image { get; }
        decimal Price { get; }
    }
}
