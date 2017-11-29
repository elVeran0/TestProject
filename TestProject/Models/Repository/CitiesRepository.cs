using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace TestProject.Models
{
    public class CitiesRepository : Repository
    {
        public CitiesRepository(Context context = null)
            : base(context)
        { }

        public IEnumerable<City> GetAll()
        {
            return Context().cities.ToList<City>();
        }

        public City Get(int id)
        {
            return Context().cities.Find(id);
        }

        public int Add(City city)
        {
            Context().cities.Add(city);
            Context().SaveChanges();

            return city.Id;
        }

        public void Update(City city)
        {
            City result = Context().cities.Find(city.Id);

            if (result != null)
            {
                Context().Entry(result).CurrentValues.SetValues(city);
                Context().SaveChanges();
            }
        }

        public void Delete(int id)
        {
            City city = Context().cities.Find(id);

            if (city != null)
            {       
                Context().cities.Remove(city);

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