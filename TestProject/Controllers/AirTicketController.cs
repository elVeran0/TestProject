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
        AirTicketsRepository airTicketsRepository = new AirTicketsRepository();

        // GET: api/AirTicket
        public HttpResponseMessage Get()
        {
            IEnumerable<AirTicket> result = airTicketsRepository.GetAll();

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

            AirTicket result = airTicketsRepository.Get(id);

            if (result == null)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // POST: api/AirTicket
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            AirTicket airTicket;

            try
            {
                airTicket = JsonConvert.DeserializeObject<AirTicket>(request.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (airTicket == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            if (airTicket.Id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            int newId = airTicketsRepository.Add(airTicket.Id);
            if (newId == 0)
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            return Request.CreateResponse(HttpStatusCode.Created, newId);
        }

        // POST:
        [Route("api/AirTicket/search")]
        [HttpPost]
        public HttpResponseMessage Search(HttpRequestMessage request)
        {
            AirTicket airTicket;

            try
            {
                airTicket = JsonConvert.DeserializeObject<AirTicket>(request.Content.ReadAsStringAsync().Result);                
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (airTicket == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            if (airTicket.Depart < DateTime.Now || airTicket.Arrive < DateTime.Now ||
                airTicket.Depart > airTicket.Arrive ||
                airTicket.FromCityId < 1 || airTicket.ToCityId < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            IEnumerable<AirTicket> result = airTicketsRepository.Get(airTicket);

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

            AirTicket airTicket;

            try
            {
                airTicket = JsonConvert.DeserializeObject<AirTicket>(request.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (airTicket == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            airTicket.Id = id;

            if (airTicket.Depart < DateTime.MinValue || airTicket.Arrive < DateTime.MinValue ||
                airTicket.Depart > airTicket.Arrive ||
                airTicket.FromCityId < 1 || airTicket.ToCityId < 1 ||
                airTicket.AirlineId < 1 || airTicket.Price < 0)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            airTicketsRepository.Update(airTicket);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE: api/AirTicket/5
        public HttpResponseMessage Delete(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            airTicketsRepository.Delete(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
