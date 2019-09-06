using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace WebSocketJS
{
    public class Global : System.Web.HttpApplication
    {
        public static ConcurrentDictionary<string, WebSocket> sockets;
        protected void Application_Start(object sender, EventArgs e)
        {
            sockets = new ConcurrentDictionary<string, WebSocket>();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            if (sockets.ContainsKey(Session.SessionID))
            {
                bool result = sockets.TryRemove(Session.SessionID, out _);
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}