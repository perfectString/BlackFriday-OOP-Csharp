using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackFriday.Models;
using BlackFriday.Models.Contracts;
using BlackFriday.Repositories.Contracts;

namespace BlackFriday.Repositories
{
    public class ProductRepository : IRepository<IProduct>
    {
        private readonly List<IProduct> listOfProducts;
        public ProductRepository()
        {
            listOfProducts = new List<IProduct>();
        }
        public IReadOnlyCollection<IProduct> Models => listOfProducts.AsReadOnly();

        public void AddNew(IProduct model)
        {
            listOfProducts.Add(model);
        }

        public bool Exists(string name)
        {
            return listOfProducts.Any(p => p.ProductName == name);
        }

        public IProduct GetByName(string name)
        {
            if (listOfProducts.Any(p => p.ProductName == name))
            {
                return listOfProducts.First(p => p.ProductName == name);
            }

            return null;
        }
    }
}
