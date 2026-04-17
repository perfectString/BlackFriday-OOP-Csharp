using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackFriday.Models;
using BlackFriday.Models.Contracts;
using BlackFriday.Utilities.Messages;

namespace BlackFriday.Core.Contracts
{
    public class Controller : IController
    {
        private Application application;
        private readonly string[] productTypes = new[]
        {
            nameof(Item) ,
            nameof(Service)
        };

        public Controller()
        {
            application = new Application();
        }
        public string AddProduct(string productType, string productName, string userName, double basePrice)
        {
            if (!productTypes.Contains(productType))
            {
                return string.Format(OutputMessages.ProductIsNotPresented, productType);
            }
            if (application.Products.Exists(productName))
            {
                return string.Format(OutputMessages.ProductNameDuplicated, productName);

            }

            var user = application.Users.GetByName(userName);
            if (user is Client || user is null)
            {
                return string.Format(OutputMessages.UserIsNotAdmin, userName);
            }

            IProduct product = null;


            if (productType == nameof(Service))
            {
                product = new Service(productName, basePrice);
                application.Products.AddNew(product);

            }
            else if (productType == nameof(Item))
            {
                product = new Item(productName, basePrice);
                application.Products.AddNew(product);

            }

            return string.Format(OutputMessages.ProductAdded, productType, productName, $"{basePrice:F2}");
        }

        public string ApplicationReport()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Application administration:");
            foreach (var admin in application.Users.Models.OfType<Admin>().OrderBy(u => u.UserName).ToList())
            {
                stringBuilder.AppendLine(admin.ToString());
            }
            stringBuilder.AppendLine("Clients:");
            foreach (var user in application.Users.Models.OfType<Client>().OrderBy(u => u.UserName).ToList())
            {
                stringBuilder.AppendLine(user.ToString());
                int bfPurchase = 0;



                var blackFridayProducts = user.Purchases
        .Where(kvp => kvp.Value == true)
        .Select(kvp => kvp.Key)
        .ToList();

                if (blackFridayProducts.Count > 0)
                {
                    stringBuilder.AppendLine($"-Black Friday Purchases: {blackFridayProducts.Count}");

                    foreach (var product in blackFridayProducts)
                    {
                        stringBuilder.AppendLine($"--{product}");
                    }
                }
            }
            return stringBuilder.ToString().TrimEnd();


        }

        public string PurchaseProduct(string userName, string productName, bool blackFridayFlag)
        {
            var user = application.Users.GetByName(userName);
            if (user is Admin || user is null)
            {
                return string.Format(OutputMessages.UserIsNotClient, userName);
            }

            if (!application.Products.Exists(productName))
            {
                return string.Format(OutputMessages.ProductDoesNotExist, productName);

            }
            var item = application.Products.GetByName(productName);

            if (item != null && item.IsSold)
            {
                return string.Format(OutputMessages.ProductOutOfStock, productName);

            }
            ((Client)user).PurchaseProduct(productName, blackFridayFlag);
            item.ToggleStatus();

            double price = 0;
            if (blackFridayFlag)
            {
                price = item.BlackFridayPrice;
            }
            else
            {
                price = item.BasePrice;
            }
            return string.Format(OutputMessages.ProductPurchased, userName, productName, $"{price:F2}");
        }

        public string RefreshSalesList(string userName)
        {
            var user = application.Users.GetByName(userName);
            if (user is Client || user is null)
            {
                return string.Format(OutputMessages.UserIsNotAdmin, userName);
            }

            int count = 0;
            foreach (var products in application.Products.Models.Where(p => p.IsSold))
            {
                products.ToggleStatus();
                count++;
            }

            return string.Format(OutputMessages.SalesListRefreshed, count);
        }

        public string RegisterUser(string userName, string email, bool hasDataAccess)
        {
            if (application.Users.Exists(userName))
            {
                return string.Format(OutputMessages.UserAlreadyRegistered, userName);
            }

            bool takenEmail = false;
            foreach (var user in application.Users.Models)
            {
                if (user.Email == email)
                {
                    takenEmail = true;
                }
            }

            if (takenEmail)
            {
                return string.Format(OutputMessages.SameEmailIsRegistered, email);
            }

            if (hasDataAccess)
            {

                int adminCount = application.Users.Models.Count(u => u is Admin);
                if (adminCount == 2)
                {
                    return OutputMessages.AdminCountLimited;
                }
                else
                {
                    IUser admin = new Admin(userName, email);
                    application.Users.AddNew(admin);
                    return string.Format(OutputMessages.AdminRegistered, userName);
                }
            }
            else
            {
                IUser user = new Client(userName, email);
                application.Users.AddNew(user);
                return string.Format(OutputMessages.ClientRegistered, userName);
            }

        }

        public string UpdateProductPrice(string productName, string userName, double newPriceValue)
        {
            if (!application.Products.Exists(productName))
            {
                return string.Format(OutputMessages.ProductDoesNotExist, productName);
            }

            var user = application.Users.GetByName(userName);
            if (user is Client || user is null)
            {
                return string.Format(OutputMessages.UserIsNotAdmin, userName);
            }

            var item = application.Products.GetByName(productName);

            var currentPrice = item.BasePrice;
            item.UpdatePrice(newPriceValue);

            return string.Format(OutputMessages.ProductPriceUpdated, productName, $"{currentPrice:F2}", $"{newPriceValue:F2}");
            //moje nqkude neshto s cenite da e greshno nz

        }
    }
}
