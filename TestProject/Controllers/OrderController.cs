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
        OrdersRepository ordersRepository = new OrdersRepository();

        // GET: api/Order
        public HttpResponseMessage Get()
        {
            IEnumerable<Order> result = ordersRepository.GetAll();

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

            Order result = ordersRepository.Get(id);

            if (result == null)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);        
        }

        // POST: api/Order
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            Order order;

            try
            {
                order = JsonConvert.DeserializeObject<Order>(request.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (order == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            if (order.AirTicketId < 1 || order.HotelOrderId < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            int id = ordersRepository.Add(order);
            if (id == 0)
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            return Request.CreateResponse(HttpStatusCode.Created, id);
        }

        // PUT: api/Order/5
        public HttpResponseMessage Put(int id, HttpRequestMessage request)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            Order order;

            try
            {
                order = JsonConvert.DeserializeObject<Order>(request.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (order == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            order.Id = id;

            if (order.AirTicketId < 1 || order.HotelOrderId < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            ordersRepository.Update(order);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        
        // DELETE: api/Order/5
        public HttpResponseMessage Delete(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            ordersRepository.Delete(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
