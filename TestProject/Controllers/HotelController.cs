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
        HotelsContext hotelsContext = new HotelsContext();

        // GET: api/Hotel
        public HttpResponseMessage Get()
        {
            IEnumerable<Hotel> result = hotelsContext.GetAll();

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

            Hotel result = hotelsContext.Get(id);

            if (result == null)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // POST: api/Hotel
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            var definition = new
            {
                Name = "",
                CityId = 0,
                Address = ""
            };

            Hotel hotel = new Hotel();

            try
            {
                var temp = JsonConvert.DeserializeAnonymousType(request.Content.ReadAsStringAsync().Result, definition);
                
                if (temp == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                hotel.Name = temp.Name;
                hotel.City = new City
                {
                    Id = temp.CityId
                };
                hotel.Address = temp.Address;
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (hotel.Name == null || hotel.City.Id < 1 || hotel.Address == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);


            int id = hotelsContext.Add(hotel);
            if (id == 0)
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            return Request.CreateResponse(HttpStatusCode.Created, id);
        }

        // PUT: api/Hotel/5
        public HttpResponseMessage Put(int id, HttpRequestMessage request)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            var definition = new
            {
                Name = "",
                CityId = 0,
                Address = ""
            };

            Hotel hotel = new Hotel
            {
                Id = id
            };
            hotel.City = new City();

            try
            {
                var temp = JsonConvert.DeserializeAnonymousType(request.Content.ReadAsStringAsync().Result, definition);

                if (temp == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                hotel.Name = temp.Name;
                hotel.City.Id = temp.CityId;
                hotel.Address = temp.Address;
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (hotel.Name == null || hotel.City.Id < 1 || hotel.Address == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            hotelsContext.Update(hotel);

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        // DELETE: api/Hotel/5
        public HttpResponseMessage Delete(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            hotelsContext.Delete(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
