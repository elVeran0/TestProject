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
    public class OrderController : ApiController
    {
        OrdersContext ordersContext = new OrdersContext();

        // GET: api/Order
        public HttpResponseMessage Get()
        {
            IEnumerable<Order> result = ordersContext.GetAll();

            if (result.Any() == false)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // GET: api/Order/5
        public HttpResponseMessage Get(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            Order result = ordersContext.Get(id);

            if (result == null)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);        
        }

        // POST: api/Order
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            var definition = new
            {
                AirTicketId = 0,
                HotelOrderId = 0
            };

            Order order = new Order();

            try
            {
                var or = JsonConvert.DeserializeAnonymousType(request.Content.ReadAsStringAsync().Result, definition);

                if (or == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                order.AirTicket = new AirTicket
                {
                    Id = or.AirTicketId
                };

                order.HotelOrder = new HotelOrder
                {
                    Id = or.HotelOrderId
                };
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (order.AirTicket.Id < 1 || order.HotelOrder.Id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            int id = ordersContext.Add(order);
            if (id == 0)
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            return Request.CreateResponse(HttpStatusCode.Created, id);
        }

        // PUT: api/Order/5
        public HttpResponseMessage Put(int id, HttpRequestMessage request)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            var definition = new
            {
                AirTicketId = 0,
                HotelOrderId = 0
            };

            Order order = new Order
            {
                Id = id
            };

            try
            {
                var or = JsonConvert.DeserializeAnonymousType(request.Content.ReadAsStringAsync().Result, definition);

                if (or == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                order.AirTicket = new AirTicket 
                {
                    Id = or.AirTicketId
                };
                order.HotelOrder = new HotelOrder
                {
                    Id = or.HotelOrderId
                };
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (order.AirTicket.Id < 1 || order.HotelOrder.Id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            ordersContext.Update(order);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        
        // DELETE: api/Order/5
        public HttpResponseMessage Delete(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            ordersContext.Delete(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
