using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;


namespace TestProject.Models
{
    public class AirlinesRepository : Repository
    {
        public AirlinesRepository(Context context = null)
            : base(context)
        { }

        public IEnumerable<Airline> GetAll()
        {
            return Context().airlines.ToList<Airline>();
        }

        public Airline Get(int id)
        {
            return Context().airlines.Find(id);
        }

        public int Add(Airline airline)
        {
            Context().airlines.Add(airline);
            Context().SaveChanges();

            return airline.Id;
        }

        public void Update(Airline airline)
        {
            Airline result = Context().airlines.Find(airline.Id);

            if (result != null)
            {
                Context().Entry(result).CurrentValues.SetValues(airline);
                Context().SaveChanges();
            }
        }

        public void Delete(int id)
        {
            Airline airline = Context().airlines.Find(id);

            if (airline != null)
            {
                Context().airlines.Remove(airline);

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