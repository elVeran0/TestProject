using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestProject.Models;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace TestProject.WebSocketControllers
{
    //WebSocketSharp
    public class CartWebSocketController : WebSocketBehavior
    {
        CartItemsRepository cartsItemRepository = new CartItemsRepository();

        protected override void OnMessage(MessageEventArgs e)
        {
            int cartId = 0;

            cartId = Convert.ToInt32(e.Data);

            if (cartId < 1)
                return;

            int count = cartsItemRepository.Count(cartId);

            Send(count.ToString());
        }
    }
}