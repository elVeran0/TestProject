using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace TestProject.Models
{
    public class HotelOrdersRepository : Repository
    {
        public HotelOrdersRepository(Context context = null)
            : base(context)
        { }

        public IEnumerable<HotelOrder> GetAll()
        {
            return Context().hotelOrders.ToList<HotelOrder>();
        }

        public HotelOrder Get(int id)
        {
            return Context().hotelOrders.Find(id);
        }

        public IEnumerable<HotelOrder> Get(HotelOrder searchHotelOrder)
        {
            return Context().hotelOrders.Where(ho => ho.CheckIn == searchHotelOrder.CheckIn)
                                        .Where(ho => ho.CheckOut == searchHotelOrder.CheckOut)
                                        .Where(ho => ho.HotelId == searchHotelOrder.HotelId)
                                        .ToList();
        }

        public int Add(HotelOrder hotelOrder)
        {
            Context().hotelOrders.Add(hotelOrder);
            Context().SaveChanges();

            return hotelOrder.Id;
        }
        
        public void Update(HotelOrder hotelOrder)
        {
            HotelOrder result = Context().hotelOrders.Find(hotelOrder.Id);

            if (result != null)
            {
                Context().Entry(result).CurrentValues.SetValues(hotelOrder);
                Context().SaveChanges();
            }
        }

        public void Delete(int id)
        {
            HotelOrder hotelOrder = Context().hotelOrders.Find(id);

            if (hotelOrder != null)
            {
                Context().hotelOrders.Remove(hotelOrder);

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