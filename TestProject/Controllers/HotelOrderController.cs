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
        HotelOrdersContext hotelOrdersContext = new HotelOrdersContext();

        // GET: api/HotelOrder
        public HttpResponseMessage Get()
        {
            IEnumerable<HotelOrder> result = hotelOrdersContext.GetAll();

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

            HotelOrder result = hotelOrdersContext.Get(id);

            if (result == null)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // POST: api/HotelOrder
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            var definition = new
            {
                CheckIn = DateTime.MinValue,
                CheckOut = DateTime.MinValue,
                HotelId = 0,
                Price = 0
            };

            HotelOrder hotelOrder = new HotelOrder();

            try
            {
                var ho = JsonConvert.DeserializeAnonymousType(request.Content.ReadAsStringAsync().Result, definition);

                if (ho == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                hotelOrder.CheckIn = ho.CheckIn;
                hotelOrder.CheckOut = ho.CheckOut;
                hotelOrder.Hotel = new Hotel();
                hotelOrder.Hotel.Id = ho.HotelId;
                hotelOrder.Price = ho.Price;
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (hotelOrder.CheckIn < DateTime.MinValue || hotelOrder.CheckOut < DateTime.MinValue ||
                hotelOrder.CheckIn > hotelOrder.CheckOut ||
                hotelOrder.Hotel.Id < 1 || hotelOrder.Price < 0)

                return Request.CreateResponse(HttpStatusCode.BadRequest);

            int id = hotelOrdersContext.Add(hotelOrder);
            if (id == 0)
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            return Request.CreateResponse(HttpStatusCode.Created, id);
        }

        // POST: api/HotelOrder
        [Route("api/HotelOrder/search")]
        [HttpPost]
        public HttpResponseMessage Search(HttpRequestMessage request)
        {
            var definition = new
            {
                CheckIn = DateTime.MinValue,
                CheckOut = DateTime.MinValue,
                HotelId = 0//,
                //Price = 0
            };

            HotelOrder hotelOrder = new HotelOrder();

            try
            {
                var ho = JsonConvert.DeserializeAnonymousType(request.Content.ReadAsStringAsync().Result, definition);

                if (ho == null)
                    return Request.CreateResponse(HttpStatusCode.NoContent);

                hotelOrder.CheckIn = ho.CheckIn;
                hotelOrder.CheckOut = ho.CheckOut;
                hotelOrder.Hotel = new Hotel();
                hotelOrder.Hotel.Id = ho.HotelId;
                //hotelOrder.Price = ho.Price;
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (hotelOrder.CheckIn < DateTime.MinValue || hotelOrder.CheckOut < DateTime.MinValue ||
                hotelOrder.CheckIn > hotelOrder.CheckOut ||
                hotelOrder.Hotel.Id < 1 || hotelOrder.Price < 0)
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            
            IEnumerable<HotelOrder> result = hotelOrdersContext.Get(hotelOrder);

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

            var definition = new
            {
                CheckIn = DateTime.MinValue,
                CheckOut = DateTime.MinValue,
                HotelId = 0,
                Price = 0
            };

            HotelOrder hotelOrder = new HotelOrder
            {
                Id = id
            };

            try
            {
                var ho = JsonConvert.DeserializeAnonymousType(request.Content.ReadAsStringAsync().Result, definition);

                if (ho == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                hotelOrder.CheckIn = ho.CheckIn;
                hotelOrder.CheckOut = ho.CheckOut;
                hotelOrder.Hotel = new Hotel();
                hotelOrder.Hotel.Id = ho.HotelId;
                hotelOrder.Price = ho.Price;
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (hotelOrder.CheckIn < DateTime.MinValue || hotelOrder.CheckOut < DateTime.MinValue ||
                hotelOrder.Hotel.Id < 1 || hotelOrder.Price < 0)
             
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            hotelOrdersContext.Update(hotelOrder);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE: api/HotelOrder/5
        public HttpResponseMessage Delete(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            hotelOrdersContext.Delete(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
