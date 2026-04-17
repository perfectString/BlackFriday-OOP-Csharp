using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackFriday.Models
{
    public class Client : User
    {
        private readonly Dictionary<string,bool> _purchases;

        public Client(string userName, string email) : base(userName, email, false)
        {
            _purchases = new Dictionary<string,bool>();
        }

        public IReadOnlyDictionary<string, bool> Purchases => _purchases;

        public void PurchaseProduct(string productName, bool blackFridayFlag)
        {
            _purchases[productName] = blackFridayFlag;
        }
    }
}
