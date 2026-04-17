using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackFriday.Models.Contracts;
using BlackFriday.Utilities.Messages;

namespace BlackFriday.Models
{
    public abstract class Product : IProduct
    {
        private string _productName;
        private double _basePrice;

        public Product(string productName, double basePrice) //WAS PROTECTED SWITCH JUST FOR SAKE OF TEST
        {
            ProductName = productName;
            BasePrice = basePrice;
            IsSold = false;
        }

        public string ProductName
        {
            get => _productName;

            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.ProductNameRequired);
                }

                _productName = value;
            }
        }

        public double BasePrice
        {
            get => _basePrice;
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(ExceptionMessages.ProductPriceConstraints);
                }
                _basePrice = value;

            } 
                
        }

        public abstract double BlackFridayPrice { get; } //WAS VIRTUAL CHANGE FOR THE SAKE OF TEST

        public bool IsSold { get; set; } //promenih tova na prosto set vnimavai

        public void ToggleStatus()
        {
            if (!IsSold)
            {
                IsSold = true;
            }
            else
            {
                IsSold = false;
            }
        }

        public void UpdatePrice(double newPriceValue)
        {
            this.BasePrice = newPriceValue;
        }

        public override string ToString()
        {
            return $"Product: {ProductName}, Price: {BasePrice:F2}, You Save: {(BasePrice - BlackFridayPrice):F2}";
        }
    }
}
