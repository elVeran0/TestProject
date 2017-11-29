using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace TestProject.Models
{
    public class AirTicketsBaseRepository : Repository
    {
        public AirTicketsBaseRepository(Context context = null)
            : base(context)
        { }

        public IEnumerable<AirTicketBase> GetAll()
        {
            return Context().airTicketsBase.ToList<AirTicketBase>();
        }

        public AirTicketBase Get(int id)
        {
            return Context().airTicketsBase.Find(id);
        }

        public IEnumerable<AirTicketBase> Get(AirTicketBase searchAirTicket)
        {
            return Context().airTicketsBase.Where(a => a.FromCityId == searchAirTicket.FromCityId)
                                           .Where(a => a.ToCityId == searchAirTicket.ToCityId)
                                           .Where(a => DbFunctions.TruncateTime(a.Depart) == searchAirTicket.Depart.Date)
                                           .ToList();
        }

        public int Add(AirTicketBase airTicket)
        {
            Context().airTicketsBase.Add(airTicket);
            Context().SaveChanges();

            return airTicket.Id;
        }

        public void Update(AirTicketBase airTicket)
        {
            AirTicketBase result = Context().airTicketsBase.Find(airTicket.Id);

            if (result != null)
            {
                Context().Entry(result).CurrentValues.SetValues(airTicket);
                Context().SaveChanges();
            }
        }

        public void Delete(int id)
        {
            AirTicketBase airTicket = Context().airTicketsBase.Find(id);

            if (airTicket != null)
            { 
                Context().airTicketsBase.Remove(airTicket);

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