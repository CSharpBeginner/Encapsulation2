using System;
using System.Collections.Generic;
using System.Text;

namespace Store
{
    class Program
    {
        static void Main(string[] args)
        {
            Product iPhone12 = new Product("IPhone 12");
            Product iPhone11 = new Product("IPhone 11");

            Warehouse warehouse = new Warehouse();

            Shop shop = new Shop(warehouse);

            warehouse.Deliver(iPhone12, 10);
            warehouse.Deliver(iPhone11, 1);

            Cart cart = shop.Cart();
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 3);

            Console.WriteLine(cart.Order().Paylink);

            cart.Add(iPhone12, 9);
        }
    }

    class Product
    {
        public string Name { get; private set; }

        public Product(string name)
        {
            Name = name;
        }
    }

    class Warehouse
    {
        private Dictionary<Product, int> _warehouse;

        public Warehouse()
        {
            _warehouse = new Dictionary<Product, int>();
        }

        public void Deliver(Product product, int quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            _warehouse.Add(product, quantity);
        }

        public bool TryRemove(Product product, int quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (_warehouse.ContainsKey(product))
            {
                if (_warehouse[product] >= quantity)
                {
                    Remove(product, quantity);
                    return true;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

            throw new ArgumentException();
        }

        private void Remove(Product product, int quantity)
        {
            _warehouse[product] -= quantity;
        }
    }

    class Shop
    {
        private Warehouse _warehouse;

        public Shop(Warehouse warehouse)
        {
            _warehouse = warehouse;
        }

        public Cart Cart()
        {
            return new Cart(this);
        }

        public bool TryRemove(Product product, int quantity)
        {
            return _warehouse.TryRemove(product, quantity);
        }

        public Order Order()
        {
            return new Order();
        }
    }

    class Order
    {
        public string Paylink { get; private set; }

        public Order()
        {
            GenerateRandomLink();
        }

        private void GenerateRandomLink()
        {
            int length = 20;
            StringBuilder paylink = new StringBuilder(length);
            int firstSymbol = 'a';
            int lastSymbol = 'z' + 1;
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                paylink.Append((char)random.Next(firstSymbol, lastSymbol));
            }

            Paylink = paylink.ToString();
        }
    }

    class Cart
    {
        private Shop _shop;
        private Dictionary<Product, int> _cart;

        public Cart(Shop shop)
        {
            _shop = shop;
            _cart = new Dictionary<Product, int>();
        }

        public void Add(Product product, int quantity)
        {
            if (_shop.TryRemove(product, quantity))
            {
                _cart.Add(product, quantity);
            }
        }

        public Order Order()
        {
            Order newOrder = _shop.Order();
            _cart.Clear();
            return newOrder;
        }
    }
}
