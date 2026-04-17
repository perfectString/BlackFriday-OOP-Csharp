using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackFriday.Models.Contracts;
using BlackFriday.Repositories.Contracts;

namespace BlackFriday.Repositories
{
    public class UserRepository : IRepository<IUser>
    {
        private readonly List<IUser> listOfUsers;
        public UserRepository()
        {
            listOfUsers = new List<IUser>();
        }
        public IReadOnlyCollection<IUser> Models => listOfUsers.AsReadOnly();

        public void AddNew(IUser model)
        {
            listOfUsers.Add(model);
        }

        public bool Exists(string name)
        {
            return listOfUsers.Any(u => u.UserName == name);
        }

        public IUser GetByName(string name)
        {
            if (Exists(name))
            {
                return listOfUsers.First(u => u.UserName == name);
            }

            return null;
        }
    }
}
