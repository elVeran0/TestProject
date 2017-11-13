using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestProject.Models;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;
//using Fleck;

namespace TestProject.WebSocketControllers
{
    //WebSocketSharp
    public class CartWebSocketController : WebSocketBehavior
    {
        CartItemsContext cartsItemContext = new CartItemsContext();

        protected override void OnMessage(MessageEventArgs e)
        {
            int cartId = 0;

            cartId = Convert.ToInt32(e.Data);

            if (cartId < 1)
                return;

            int count = cartsItemContext.Count(cartId);

            Send(count.ToString());
        }

        //protected override void OnOpen()
        //{
        //    Send("Hello");
        //}

        //protected virtual void OnClose(CloseEventArgs e)
        //{
        //    Send("Bye");
        //}
    }

    ////Fleck
    //public class CartWebSocketController
    //{
    //    CartItemsContext cartsItemContext = new CartItemsContext();

    //    WebSocketServer wsServ = null; 

    //    public CartWebSocketController()
    //    {
    //        wsServ = new WebSocketServer("ws://127.0.0.1:49358");

    //        if(wsServ != null)
    //        {
    //            wsServ.Start(socket =>
    //            {
    //                //socket.OnOpen = () => 
    //                //{
    //                //    socket.Send("Hello");
    //                //};

    //                //socket.OnClose = () =>
    //                //{
    //                //    socket.Send("Bye");
    //                //};

    //                socket.OnMessage = (message) =>
    //                {
    //                    int cartId = 0;

    //                    cartId = Convert.ToInt32(message);

    //                    if (cartId < 1)
    //                        return;

    //                    int count = cartsItemContext.Count(cartId);

    //                    socket.Send(count.ToString());
    //                };
    //            }
    //            );
    //        }
    //    }
    //}
}