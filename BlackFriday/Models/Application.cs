using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using BlackFriday.Models.Contracts;
using BlackFriday.Repositories;
using BlackFriday.Repositories.Contracts;

namespace BlackFriday.Models
{
    public class Application : IApplication
    {
        private readonly IRepository<IProduct> _productRepository;
        private readonly IRepository<IUser> _userRepository;

        public Application()
        {
            _productRepository = new ProductRepository();
            _userRepository = new UserRepository();
        }

        public IRepository<IProduct> Products => _productRepository;

        public IRepository<IUser> Users => _userRepository;
    }
}
