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
    public class AirTicketBaseController : ApiController
    {
        AirTicketsBaseRepository airTicketsBaseRepository = new AirTicketsBaseRepository();

        // GET: api/AirTicketBase
        public HttpResponseMessage Get()
        {
            IEnumerable<AirTicketBase> result = airTicketsBaseRepository.GetAll();

            if (result.Any() == false)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // GET: api/AirTicketBase/5
        public HttpResponseMessage Get(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            AirTicketBase result = airTicketsBaseRepository.Get(id);

            if (result == null)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // POST: api/AirTicketBase
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            AirTicketBase airTicket;

            try
            {
                airTicket = JsonConvert.DeserializeObject<AirTicketBase>(request.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (airTicket == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            if (airTicket.Depart < DateTime.MinValue || airTicket.Arrive < DateTime.MinValue ||
                airTicket.Depart > airTicket.Arrive ||
                airTicket.FromCityId < 1 || airTicket.ToCityId < 1 ||
                airTicket.AirlineId < 1 || airTicket.Price < 0)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            int id = airTicketsBaseRepository.Add(airTicket);
            if (id == 0)
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            return Request.CreateResponse(HttpStatusCode.Created, id);
        }

        // POST:
        [Route("api/AirTicketBase/search")]
        [HttpPost]
        public HttpResponseMessage Search(HttpRequestMessage request)
        {
            AirTicketBase airTicket;

            try
            {
                airTicket = JsonConvert.DeserializeObject<AirTicketBase>(request.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (airTicket == null)
                return Request.CreateResponse(HttpStatusCode.NoContent);

            if (airTicket.Depart < DateTime.Now || 
                airTicket.FromCityId < 1 || airTicket.ToCityId < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            
            IEnumerable<AirTicketBase> result = airTicketsBaseRepository.Get(airTicket);

            if (result.Any() == false)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // PUT: api/AirTicketBase/5
        public HttpResponseMessage Put(int id, HttpRequestMessage request)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            AirTicketBase airTicket;
            try
            {
                airTicket = JsonConvert.DeserializeObject<AirTicketBase>(request.Content.ReadAsStringAsync().Result);
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

            airTicketsBaseRepository.Update(airTicket);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE: api/AirTicketBase/5
        public HttpResponseMessage Delete(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            airTicketsBaseRepository.Delete(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
