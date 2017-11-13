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
    public class CityController : ApiController
    {
        CitiesContext citiesContext = new CitiesContext();

        // GET: api/City
        public HttpResponseMessage Get()
        {
            IEnumerable<City> result = citiesContext.GetAll();

            if (result.Any() == false)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // GET: api/City/5
        public HttpResponseMessage Get(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            City result = citiesContext.Get(id);

            if (result == null)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // POST: api/City        
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

            int id = citiesContext.Add(name);
            if (id == 0)
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            return Request.CreateResponse(HttpStatusCode.Created, id);
        }

        // PUT: api/City/5
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

            citiesContext.Update(new City 
            { 
                Id = id,
                Name = name
            });

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE: api/City/5
        public HttpResponseMessage Delete(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            citiesContext.Delete(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
