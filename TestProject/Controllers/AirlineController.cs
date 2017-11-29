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
    public class AirlineController : ApiController
    {
        AirlinesRepository airlinesRepository = new AirlinesRepository();

        // GET: api/Airline
        public HttpResponseMessage Get()
        {
            IEnumerable<Airline> result = airlinesRepository.GetAll();

            if(result.Any() == false)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // GET: api/Airline/5
        public HttpResponseMessage Get(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            Airline result = airlinesRepository.Get(id);

            if (result == null)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // POST: api/Airline
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            Airline airline;

            try
            {
                airline = JsonConvert.DeserializeObject<Airline>(request.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            
            if (airline == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            int id = airlinesRepository.Add(airline);

            if (id == 0)
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            return Request.CreateResponse(HttpStatusCode.Created, id);
        }

        // PUT: api/Airline/5
        public HttpResponseMessage Put(int id, HttpRequestMessage request)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            Airline airline;

            try
            {
                airline = JsonConvert.DeserializeObject<Airline>(request.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (airline == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            
            airline.Id = id;            

            airlinesRepository.Update(airline);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE: api/Airline/5
        public HttpResponseMessage Delete(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            airlinesRepository.Delete(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
