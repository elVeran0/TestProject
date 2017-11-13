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
        AirlinesContext airlinesContext = new AirlinesContext();

        // GET: api/Airline
        public HttpResponseMessage Get()
        {
            IEnumerable<Airline> result = airlinesContext.GetAll();

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

            Airline result = airlinesContext.Get(id);

            if (result == null)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // POST: api/Airline
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            var definition = new 
            { 
                Name = "" 
            };

            String name;

            try
            {
                name = JsonConvert.DeserializeAnonymousType(request.Content.ReadAsStringAsync().Result, definition).Name;
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (name == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            int id = airlinesContext.Add(name);
            if (id == 0)
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            return Request.CreateResponse(HttpStatusCode.Created, id);
        }

        // PUT: api/Airline/5
        public HttpResponseMessage Put(int id, HttpRequestMessage request)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            var definition = new 
            { 
                Name = "" 
            };
            String name;

            try
            {
                name = JsonConvert.DeserializeAnonymousType(request.Content.ReadAsStringAsync().Result, definition).Name;
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (name == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            airlinesContext.Update(new Airline
            {
                Id = id,
                Name = name
            });

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE: api/Airline/5
        public HttpResponseMessage Delete(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            airlinesContext.Delete(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
