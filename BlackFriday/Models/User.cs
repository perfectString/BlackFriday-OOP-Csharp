using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BlackFriday.Models.Contracts;
using BlackFriday.Utilities.Messages;

namespace BlackFriday.Models
{
    public abstract class User : IUser
    {
        private string _username;
        private string _email;

        protected User(string userName, string email, bool hasDataAccess)
        {
            UserName = userName;
            Email = email;
            HasDataAccess = hasDataAccess;
        }

        public string UserName
        {
            get => _username;

            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.UserNameRequired);
                }

                _username = value;
            }
        }

        public bool HasDataAccess { get; private set; }

        public string Email
        {
            get 
            {
                return HasDataAccess ? "hidden" : _email;
            }

            private set
            {
                if (!HasDataAccess)
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException(ExceptionMessages.EmailRequired);
                    }
                }

                _email = value;
            }
        }

        public override string ToString()
        {
            return $"{UserName} - Status: {GetType().Name}, Contact Info: {Email}";
        }
    }
}
