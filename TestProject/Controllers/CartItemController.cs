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
        CartItemsContext cartsItemContext = new CartItemsContext();

        // GET: api/CartItem
        public HttpResponseMessage Get()
        {
            IEnumerable<CartItem> result = cartsItemContext.GetAll();

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

            CartItem result = cartsItemContext.Get(id);

            if (result == null)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // POST: api/CartItem
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            var definition = new
            {
                CartId = 0,
                OrderId = 0
            };

            CartItem cartItem = new CartItem();

            try
            {
                var ci = JsonConvert.DeserializeAnonymousType(request.Content.ReadAsStringAsync().Result, definition);

                if (ci == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                cartItem.Cart = new Cart
                {
                    Id = ci.CartId
                };
                cartItem.Order = new Order
                {
                    Id = ci.OrderId
                };
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (cartItem.Cart.Id < 1 || cartItem.Order.Id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            int id = cartsItemContext.Add(cartItem);
            if (id == 0)
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            return Request.CreateResponse(HttpStatusCode.Created, id);
        }

        // POST: api/CartItem
        [Route("api/CartItem/search")]
        [HttpPost]
        public HttpResponseMessage Search(HttpRequestMessage request)
        {
            var definition = new
            {
                CartId = 0
            };

            CartItem cartItem = new CartItem();

            try
            {
                var ct = JsonConvert.DeserializeAnonymousType(request.Content.ReadAsStringAsync().Result, definition);

                if (ct == null)
                    return Request.CreateResponse(HttpStatusCode.NoContent);

                cartItem.Cart = new Cart 
                { 
                    Id = ct.CartId
                };                    
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (cartItem.Cart.Id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            IEnumerable<CartItem> result = cartsItemContext.Get(cartItem);

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

            var definition = new
            {
                CartId = 0,
                OrderId = 0
            };

            CartItem cartItem = new CartItem
            {
                Id = id
            };

            try
            {
                var ci = JsonConvert.DeserializeAnonymousType(request.Content.ReadAsStringAsync().Result, definition);

                if (ci == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                cartItem.Cart = new Cart
                {
                    Id = ci.CartId
                };
                cartItem.Order = new Order
                {
                    Id = ci.OrderId
                };
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (cartItem.Cart.Id < 1 || cartItem.Order.Id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            cartsItemContext.Update(cartItem);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE: api/CartItem/5
        public HttpResponseMessage Delete(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            cartsItemContext.Delete(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
