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
    public class HotelOrderController : ApiController
    {
        HotelOrdersRepository hotelOrdersRepository = new HotelOrdersRepository();

        // GET: api/HotelOrder
        public HttpResponseMessage Get()
        {
            IEnumerable<HotelOrder> result = hotelOrdersRepository.GetAll();

            if (result.Any() == false)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // GET: api/HotelOrder/5
        public HttpResponseMessage Get(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            HotelOrder result = hotelOrdersRepository.Get(id);

            if (result == null)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // POST: api/HotelOrder
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            HotelOrder hotelOrder;

            try
            {
                hotelOrder = JsonConvert.DeserializeObject<HotelOrder>(request.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (hotelOrder == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            if (hotelOrder.CheckIn < DateTime.MinValue || hotelOrder.CheckOut < DateTime.MinValue ||
                hotelOrder.CheckIn > hotelOrder.CheckOut ||
                hotelOrder.HotelId < 1 || hotelOrder.Price < 0)

                return Request.CreateResponse(HttpStatusCode.BadRequest);

            int id = hotelOrdersRepository.Add(hotelOrder);
            if (id == 0)
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            return Request.CreateResponse(HttpStatusCode.Created, id);
        }

        // POST:
        [Route("api/HotelOrder/search")]
        [HttpPost]
        public HttpResponseMessage Search(HttpRequestMessage request)
        {
            HotelOrder hotelOrder;

            try
            {
                hotelOrder = JsonConvert.DeserializeObject<HotelOrder>(request.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (hotelOrder == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            if (hotelOrder.CheckIn < DateTime.MinValue || hotelOrder.CheckOut < DateTime.MinValue ||
                hotelOrder.CheckIn > hotelOrder.CheckOut ||
                hotelOrder.HotelId < 1 || hotelOrder.Price < 0)
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            
            IEnumerable<HotelOrder> result = hotelOrdersRepository.Get(hotelOrder);

            if (result.Any() == false)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // PUT: api/HotelOrder/5
        public HttpResponseMessage Put(int id, HttpRequestMessage request)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            HotelOrder hotelOrder;

            try
            {
                hotelOrder = JsonConvert.DeserializeObject<HotelOrder>(request.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (hotelOrder == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            hotelOrder.Id = id;

            if (hotelOrder.CheckIn < DateTime.MinValue || hotelOrder.CheckOut < DateTime.MinValue ||
                hotelOrder.HotelId < 1 || hotelOrder.Price < 0)
             
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            hotelOrdersRepository.Update(hotelOrder);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE: api/HotelOrder/5
        public HttpResponseMessage Delete(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            hotelOrdersRepository.Delete(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
