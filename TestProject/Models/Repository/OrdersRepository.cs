using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace TestProject.Models
{
    public class OrdersRepository : Repository
    {
        public OrdersRepository(Context context = null)
            : base(context)
        { }

        public IEnumerable<Order> GetAll()
        {
            return Context().orders.ToList<Order>();
        }

        public Order Get(int id)
        {
            return Context().orders.Find(id);
        }

        public int Add(Order order)
        {
            Context().orders.Add(order);
            Context().SaveChanges();

            return order.Id;
        }

        public void Update(Order order)
        {
            Order result = Context().orders.Find(order.Id);

            if (result != null)
            {
                Context().Entry(result).CurrentValues.SetValues(order);
                Context().SaveChanges();
            }
        }

        public void Delete(int id)
        {
            Order order = Context().orders.Find(id);

            if (order != null)
            {
                HotelOrder tempHO = order.HotelOrder;
                AirTicket tempAT = order.AirTicket;

                Context().orders.Remove(order);
                Context().airTickets.Remove(tempAT);
                Context().hotelOrders.Remove(tempHO);

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