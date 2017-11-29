using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace TestProject.Models
{
    public class CartsRepository : Repository
    {
        public CartsRepository(Context context = null)
            : base(context)
        { }

        public IEnumerable<Cart> GetAll()
        {
            return Context().carts.ToList<Cart>();
        }

        public Cart Get(int id)
        {
            return Context().carts.Find(id);
        }

        public int Add(Cart cart)
        {
            Context().carts.Add(cart);
            Context().SaveChanges();

            return cart.Id;
        }

        public void Update(Cart cart)
        {
            Cart result = Context().carts.Find(cart.Id);

            if (result != null)
            {
                Context().Entry(result).CurrentValues.SetValues(cart);
                Context().SaveChanges();
            }
        }

        public void Delete(int id)
        {
            Cart cart = Context().carts.Find(id);

            if (cart != null)
            {
                Context().cartItems.RemoveRange(Context().cartItems.Where(ci => ci.CartId == cart.Id));
                Context().carts.Remove(cart);

                try
                {
                    Context().SaveChanges();
                }
                catch (Exception) //обработка ситуации когда нельзя удалить запись из-за наличия ссылок на неё в других таблицах
                { }
            }
        }
    }
}