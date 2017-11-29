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
    public class CartItemController : ApiController
    {
        CartItemsRepository cartsItemRepository = new CartItemsRepository();

        // GET: api/CartItem
        public HttpResponseMessage Get()
        {
            IEnumerable<CartItem> result = cartsItemRepository.GetAll();

            if (result.Any() == false)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // GET: api/CartItem/5
        public HttpResponseMessage Get(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            CartItem result = cartsItemRepository.Get(id);

            if (result == null)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // POST: api/CartItem
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            CartItem cartItem;

            try
            {
                cartItem  = JsonConvert.DeserializeObject<CartItem>(request.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (cartItem == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            if (cartItem.CartId < 1 || cartItem.OrderId < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            int id = cartsItemRepository.Add(cartItem);
            if (id == 0)
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            return Request.CreateResponse(HttpStatusCode.Created, id);
        }

        // POST:
        [Route("api/CartItem/search")]
        [HttpPost]
        public HttpResponseMessage Search(HttpRequestMessage request)
        {
            CartItem cartItem;

            try
            {
                cartItem = JsonConvert.DeserializeObject<CartItem>(request.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (cartItem == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            if (cartItem.CartId < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            IEnumerable<CartItem> result = cartsItemRepository.Get(cartItem);

            if (result.Any() == false)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // PUT: api/CartItem/5
        public HttpResponseMessage Put(int id, HttpRequestMessage request)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            CartItem cartItem;

            try
            {
                cartItem = JsonConvert.DeserializeObject<CartItem>(request.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (cartItem == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            cartItem.Id = id;

            if (cartItem.CartId < 1 || cartItem.OrderId < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            cartsItemRepository.Update(cartItem);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE: api/CartItem/5
        public HttpResponseMessage Delete(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            cartsItemRepository.Delete(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
