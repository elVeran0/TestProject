using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestProject.Models;
using Newtonsoft.Json;

namespace TestProject.Controllers
{
    public class AirTicketController : ApiController
    {
        AirTicketsContext airTicketsContext = new AirTicketsContext();

        // GET: api/AirTicket
        public HttpResponseMessage Get()
        {
            IEnumerable<AirTicket> result = airTicketsContext.GetAll();

            if (result.Any() == false)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // GET: api/AirTicket/5
        public HttpResponseMessage Get(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            AirTicket result = airTicketsContext.Get(id);

            if (result == null)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // POST: api/AirTicket
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            var definition = new
            {
                Id = 0
            };

            int id = 0;

            try
            {
                var at = JsonConvert.DeserializeAnonymousType(request.Content.ReadAsStringAsync().Result, definition);

                if (at == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                id = at.Id;
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if(id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            int newId = airTicketsContext.Add(id);
            if (newId == 0)
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            return Request.CreateResponse(HttpStatusCode.Created, newId);
        }

        // POST: api/AirTicket
        [Route("api/AirTicket/search")]
        [HttpPost]
        public HttpResponseMessage Search(HttpRequestMessage request)
        {            
            var definition = new
            {
                Depart = DateTime.MinValue,
                Arrive = DateTime.MinValue,
                FromCityId = 0,
                ToCityId = 0//,
                //AirlineId = 0,
                //Price = 0
            };

            AirTicket airTicket = new AirTicket();

            try
            {
                var at = JsonConvert.DeserializeAnonymousType(request.Content.ReadAsStringAsync().Result, definition);

                if (at == null)
                    return Request.CreateResponse(HttpStatusCode.NoContent);

                airTicket.Depart = at.Depart;
                airTicket.Arrive = at.Arrive;
                airTicket.FromCity = new City 
                { 
                    Id = at.FromCityId, 
                    Name = null
                };
                airTicket.ToCity = new City
                { 
                    Id = at.ToCityId, 
                    Name = null 
                };
                //airTicket.Airline = new Airline { Id = at.AirlineId, Name = null };
                //airTicket.Price = at.Price;
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (airTicket.Depart < DateTime.Now || airTicket.Arrive < DateTime.Now ||
                airTicket.Depart > airTicket.Arrive ||
                airTicket.FromCity.Id < 1 || airTicket.ToCity.Id < 1)/* ||
                airTicket.Airline.Id < 1 || airTicket.Price < 0)*/
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            IEnumerable<AirTicket> result = airTicketsContext.Get(airTicket);

            if (result.Any() == false)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // PUT: api/AirTicket/5
        public HttpResponseMessage Put(int id, HttpRequestMessage request)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            var definition = new
            {
                Depart = DateTime.MinValue,
                Arrive = DateTime.MinValue,
                FromCityId = 0,
                ToCityId = 0,
                AirlineId = 0,
                Price = 0
            };

            AirTicket airTicket = new AirTicket
            { 
                Id = id
            };

            try
            {
                var at = JsonConvert.DeserializeAnonymousType(request.Content.ReadAsStringAsync().Result, definition);

                if (at == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                airTicket.Depart = at.Depart;
                airTicket.Arrive = at.Arrive;
                airTicket.FromCity = new City
                {
                    Id = at.FromCityId,
                    Name = null
                };
                airTicket.ToCity = new City
                {
                    Id = at.ToCityId,
                    Name = null
                };
                airTicket.Airline = new Airline
                {
                    Id = at.AirlineId,
                    Name = null
                };
                airTicket.Price = at.Price;
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (airTicket.Depart < DateTime.MinValue || airTicket.Arrive < DateTime.MinValue ||
                airTicket.Depart > airTicket.Arrive ||
                airTicket.FromCity.Id < 1 || airTicket.ToCity.Id < 1 ||
                airTicket.Airline.Id < 1 || airTicket.Price < 0)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            airTicketsContext.Update(airTicket);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE: api/AirTicket/5
        public HttpResponseMessage Delete(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            airTicketsContext.Delete(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
