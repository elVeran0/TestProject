using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace TestProject.Models
{
    public class AirTicketsRepository : Repository
    {
        public AirTicketsRepository(Context context = null)
            : base(context)
        { }

        public IEnumerable<AirTicket> GetAll()
        {
            return Context().airTickets.ToList<AirTicket>();
        }

        public AirTicket Get(int id)
        {
            return Context().airTickets.Find(id);
        }

        public IEnumerable<AirTicket> Get(AirTicket searchAirTicket)
        {
            return Context().airTickets.Where(a => a.FromCityId == searchAirTicket.FromCityId)
                                       .Where(a => a.ToCityId == searchAirTicket.ToCityId)
                                       .Where(a => DbFunctions.TruncateTime(a.Depart) == searchAirTicket.Depart.Date)
                                       .ToList();
        }

        public int Add(int id)
        {
            AirTicket airTicket = Context().airTicketsBase.Find(id);

            if (airTicket != null)
            { 
                Context().airTickets.Add(airTicket);
                Context().SaveChanges();
            }

            return airTicket.Id;
        }

        public void Update(AirTicket airTicket)
        {
            AirTicket result = Context().airTickets.Find(airTicket.Id);

            if (result != null)
            {
                Context().Entry(result).CurrentValues.SetValues(airTicket);
                Context().SaveChanges();
            }
        }

        public void Delete(int id)
        {
            AirTicket airTicket = Context().airTickets.Find(id);

            if (airTicket != null)
            { 
                Context().airTickets.Remove(airTicket);

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