using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackFriday.Models
{
    public class Item : Product
    {
        public Item(string productName, double basePrice) : base(productName, basePrice)
        {
            
        }

        public override double BlackFridayPrice { get => BasePrice * 0.70;}
    }
}
