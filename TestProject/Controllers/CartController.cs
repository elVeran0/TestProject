﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestProject.Models;
using Newtonsoft.Json;

namespace TestProject.Controllers
{
    public class CartController : ApiController
    {
        CartsContext cartsContext = new CartsContext();

        // GET: api/Cart
        public HttpResponseMessage Get()
        {
            IEnumerable<Cart> result = cartsContext.GetAll();

            if (result.Any() == false)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // GET: api/Cart/5
        public HttpResponseMessage Get(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            Cart result = cartsContext.Get(id);

            if (result == null)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // POST: api/Cart
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            var definition = new
            {
                userId = 0,
                datetime = DateTime.MinValue
            };

            Cart cart = new Cart();

            try
            {
                var temp = JsonConvert.DeserializeAnonymousType(request.Content.ReadAsStringAsync().Result, definition);

                if (temp == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                cart.UserId = temp.userId;
                cart.Datetime = temp.datetime;
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (cart.UserId < 1 || cart.Datetime < DateTime.MinValue)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            int id = cartsContext.Add(cart);
            if (id == 0)
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            return Request.CreateResponse(HttpStatusCode.Created, id);
        }

        // PUT: api/Cart/5
        public HttpResponseMessage Put(int id, HttpRequestMessage request)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            Cart cart;

            try
            {
                cart = JsonConvert.DeserializeObject<Cart>(request.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (cart == null || cart.UserId < 1 || cart.Datetime < DateTime.MinValue)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            cart.Id = id;

            cartsContext.Update(cart);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE: api/Cart/5
        public HttpResponseMessage Delete(int id)
        {
            if (id < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            cartsContext.Delete(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
