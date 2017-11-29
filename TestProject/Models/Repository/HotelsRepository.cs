using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace TestProject.Models
{
    public class HotelsRepository : Repository
    {
        public HotelsRepository(Context context = null)
            : base(context)
        { }

        public IEnumerable<Hotel> GetAll()
        {
            return Context().hotels.ToList<Hotel>();
        }

        public Hotel Get(int id)
        {
            return Context().hotels.Find(id);
        }

        public int Add(Hotel hotel)
        {
            Context().hotels.Add(hotel);
            Context().SaveChanges();

            return hotel.Id;
        }

        public void Update(Hotel hotel)
        {
            Hotel result = Context().hotels.Find(hotel.Id);

            if (result != null)
            {
                Context().Entry(result).CurrentValues.SetValues(hotel);
                Context().SaveChanges();
            }
        }

        public void Delete(int id)
        {
            Hotel hotel = Context().hotels.Find(id);

            if (hotel != null)
            {
                Context().hotels.Remove(hotel);

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