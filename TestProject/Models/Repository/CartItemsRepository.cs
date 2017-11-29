using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace TestProject.Models
{
    public class CartItemsRepository : Repository
    {
        public CartItemsRepository(Context context = null)
            : base(context)
        { }

        public IEnumerable<CartItem> GetAll()
        {
            return Context().cartItems.ToList<CartItem>();
        }

        public CartItem Get(int id)
        {
            return Context().cartItems.Find(id);
        }
        
        public IEnumerable<CartItem> Get(CartItem searchCart)
        {
            return Context().cartItems.Where(ci => ci.CartId == searchCart.CartId)
                                       .ToList();
        }

        public int Add(CartItem cartItem)
        {
            Context().cartItems.Add(cartItem);
            Context().SaveChanges();

            return cartItem.Id;
        }

        public void Update(CartItem cartItem)
        {
            CartItem result = Context().cartItems.Find(cartItem.Id);

            if (result != null)
            {
                Context().Entry(result).CurrentValues.SetValues(cartItem);
                Context().SaveChanges();
            }
        }

        public void Delete(int id)
        {
            CartItem cartItem = Context().cartItems.Find(id);

            if (cartItem != null)
            {
                int orderId = cartItem.OrderId;

                Context().cartItems.Remove(cartItem);

                try
                {
                    Context().SaveChanges();
                }
                catch (Exception) //обработка ситуации когда нельзя удалить запись из-за наличия ссылок на неё в других таблицах
                {
                    return;
                }

                OrdersRepository oRepository = new OrdersRepository();
                oRepository.Delete(orderId);                
            }
        }
        
        public int Count(int id)
        {
            return Context().cartItems.Count(ci => ci.CartId == id);
        }
    }
}