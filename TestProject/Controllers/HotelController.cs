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
    public class HotelController : ApiController
    {
        HotelsRepository hotelsRepository = new HotelsRepository();

        // GET: api/Hotel
        public HttpResponseMessage Get()
        {
            IEnumerable<Hotel> result = hotelsRepository.GetAll();

            if (result.Any() == false)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // GET: api/Hotel/5
        public HttpResponseMessage Get(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            Hotel result = hotelsRepository.Get(id);

            if (result == null)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // POST: api/Hotel
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            Hotel hotel;

            try
            {
                hotel = JsonConvert.DeserializeObject<Hotel>(request.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (hotel == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            if (hotel.Name == null || hotel.CityId < 1 || hotel.Address == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);


            int id = hotelsRepository.Add(hotel);
            if (id == 0)
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            return Request.CreateResponse(HttpStatusCode.Created, id);
        }

        // PUT: api/Hotel/5
        public HttpResponseMessage Put(int id, HttpRequestMessage request)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            
            Hotel hotel;

            try
            {
                hotel = JsonConvert.DeserializeObject<Hotel>(request.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (hotel == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            hotel.Id = id;

            if (hotel.Name == null || hotel.CityId < 1 || hotel.Address == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            hotelsRepository.Update(hotel);

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        // DELETE: api/Hotel/5
        public HttpResponseMessage Delete(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            hotelsRepository.Delete(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
