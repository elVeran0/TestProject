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
        CitiesRepository citiesRepository = new CitiesRepository();

        // GET: api/City
        public HttpResponseMessage Get()
        {
            IEnumerable<City> result = citiesRepository.GetAll();

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

            City result = citiesRepository.Get(id);

            if (result == null)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // POST: api/City        
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            City city;

            try
            {
                city = JsonConvert.DeserializeObject<City>(request.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (city == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            int id = citiesRepository.Add(city);

            if (id == 0)
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            return Request.CreateResponse(HttpStatusCode.Created, id);
        }

        // PUT: api/City/5
        public HttpResponseMessage Put(int id, HttpRequestMessage request)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            City city;

            try
            {
                city = JsonConvert.DeserializeObject<City>(request.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (city == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            city.Id = id;

            citiesRepository.Update(city);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE: api/City/5
        public HttpResponseMessage Delete(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            citiesRepository.Delete(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
