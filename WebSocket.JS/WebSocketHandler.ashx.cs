using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.WebSockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketJS
{
    /// <summary>
    /// Summary description for WebSocketHandler
    /// </summary>
    public class WebSocketHandler : IHttpHandler
    {        
        public void ProcessRequest(HttpContext context)
        {

            if (context.IsWebSocketRequest)
            {
                context.AcceptWebSocketRequest(DoRespond);
            }
        }

        private async Task DoRespond(WebSocketContext context)
        {
            if (!Global.sockets.ContainsKey(context.SecWebSocketKey))
            {
                Global.sockets.TryAdd(context.SecWebSocketKey, context.WebSocket);
            }

            //foreach (var item in Global.sockets)
            //{
                System.Net.WebSockets.WebSocket socket = context.WebSocket;
                while (true)
                {
                    try
                    {
                        ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);

                        WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                            string userMessage = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                        if (socket.State == WebSocketState.Open)
                        {
                        if (!userMessage.StartsWith("b:"))
                        {
                            userMessage = string.Format("Message : {0}", userMessage);

                            buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(userMessage));
                            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                        else
                        {
                            userMessage = string.Format("Message : {0}", userMessage);
                            await Broadcast(userMessage);
                        }
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }
            //}
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        async Task Broadcast(string msg)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg));
            foreach (var item in Global.sockets.Values)
            {
                if (item.State == WebSocketState.Open)
                {
                    await item.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}